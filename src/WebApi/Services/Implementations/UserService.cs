using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Entities.Users.Auth;
using Repository.Interfaces;
using WebApi.Services;
using WebApi.Services.Implementations;

namespace Services.Implementations;

public class UserService
{
    private readonly IRepository<User> _userRepo;
    private readonly IValidator<User> _userValidator;
    private readonly ApprovalService _approvalService;

    public UserService(IRepository<User> repository, IValidator<User> validator, ApprovalService approvalService)
    {
        _userRepo = repository;
        _userValidator = validator;
        _approvalService = approvalService;
    }

    public async Task<ServiceResult> RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
#warning проверить работу
        var userVal = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching"));
        if (userVal.IsValid is false)
        {
            return new ServiceResult(false, userVal.ToString());
        }

        if ((await _userRepo.Entites.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == true, cancellationToken)) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд.");
        }

        if ((await _userRepo.Entites.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == false, cancellationToken)) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд, но Email не подтвержден.");
        }

        user.IsEmailConfirmed = false;
        user.Password = HashPassword(user.Password!);
        await _userRepo.InsertAsync(user, cancellationToken);
        return new ServiceResult(true, "Пользователь добавлен в базу, но Email не подтвержден.");
    }

    public async Task<ServiceResult> ConfirmEmailAsync(User user, int approvalCode, CancellationToken cancellationToken = default)
    {
        var userVal = _userValidator.Validate(user, o => o.IncludeRuleSets("default"));
        if (userVal.IsValid is false)
        {
            return new ServiceResult(false, userVal.ToString());
        }

        var validUser = await _userRepo.Entites.FirstOrDefaultAsync(e => e.Email == e.Email, cancellationToken: cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь для валидации не был найден в бд.");
        }

        var validUserVal = _userValidator.Validate(validUser, o => o.IncludeRuleSets("default"));

        if (validUserVal.IsValid is false)
        {
            throw new Exception($"Пользователь из бд оказался невалидным\n{userVal}");
        }

        var approvalServiceResult = await _approvalService.VerifyCodeAsync(user, approvalCode, ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения регистрации не принят.", approvalServiceResult);
        }

        validUser.IsEmailConfirmed = true;
        await _userRepo.UpdateAsync(validUser, cancellationToken);
        return new ServiceResult(false, "Email пользователя подтвержден.");
    }

    public async Task<ServiceResult> UnregisterAsync(User user, int approvalCode, CancellationToken cancellationToken = default)
    {
#warning проверить работу

        var validUser = await _userRepo.Entites.FirstOrDefaultAsync(e => e.UserId == user.UserId, cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        var approvalServiceResult = await _approvalService.VerifyCodeAsync(validUser, approvalCode, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для удаления аккаунта не принят.", approvalServiceResult);
        }

        await _userRepo.DeleteAsync(validUser, cancellationToken);
        return new ServiceResult(true, "Аккаунт пользователя удален.");
    }
    
    public async Task<ServiceResult> UpdateEmail(User newUser, int approvalCode, CancellationToken cancellationToken = default)
    {
        var valResult = _userValidator.Validate(newUser, o => o.IncludeRuleSets("default"));
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        var approvalServiceResult = await _approvalService.VerifyCodeAsync(newUser, approvalCode, ApprovalCode.ApprovalCodeType.UpdateMail, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для изменения почты не принят.", approvalServiceResult);
        }

        var validUser = await _userRepo.Entites.FirstOrDefaultAsync(e => e.UserId == newUser.UserId, cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь с таким Id не найден.");
        }

        var validUserValResult = _userValidator.Validate(validUser, o => o.IncludeRuleSets("default"));
        if (validUserValResult.IsValid is false)
        {
            throw new Exception($"Пользователь из бд оказался невалидным\n{validUserValResult}");
        }

        validUser.Email = newUser.Email;
        await _userRepo.UpdateAsync(newUser, cancellationToken);
        return new ServiceResult(true, "Email пользователя обновлен.");
    }

    public async Task<ServiceResult> UpdatePassword(User newUser, int approvalCode, CancellationToken cancellationToken = default)
    {
        var valResult = _userValidator.Validate(newUser, o => o.IncludeRuleSets("password_regex"));
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        var approvalServiceResult = await _approvalService.VerifyCodeAsync(newUser, approvalCode, ApprovalCode.ApprovalCodeType.UpdatePassword, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для изменения пароля не принят.", approvalServiceResult);
        }

        var validUser = await _userRepo.Entites.FirstOrDefaultAsync(e => e.UserId == newUser.UserId, cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь с таким Id не найден.");
        }
        var validUserValResult = _userValidator.Validate(validUser, o => o.IncludeRuleSets("default"));
        if (validUserValResult.IsValid is false)
        {
            throw new Exception($"Пользователь из бд оказался невалидным\n{validUserValResult}");
        }

        validUser.Password = HashPassword(newUser.Password!);
        await _userRepo.UpdateAsync(newUser, cancellationToken);
        return new ServiceResult(true, "Пароль пользователя обновлен.");
    }

    public async Task<ServiceResult> CheckLoginDataAsync(User user, CancellationToken cancellationToken = default)
    {
        var valResult = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching"));
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        var userFromRepo = await _userRepo.Entites.FirstOrDefaultAsync(e => e.Email == user.Email, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователя нет в бд.");
        }

        if (userFromRepo.IsEmailConfirmed is false)
        {
            return new ServiceResult(false, "Пользователь найден, но Email не подтвержден.");
        }

        string? hashFromRepo = userFromRepo.Password;
        if (string.IsNullOrWhiteSpace(hashFromRepo))
        {
            return new ServiceResult(false, "Пользователь найден, но его пароль почему-то пуст.");
        }

        if (ValidatePassword(hashFromRepo, user.Password!) is true)
        {
            return new ServiceResult(true, "Пароль подтвержден.");
        }
        else
        {
            return new ServiceResult(false, "Пароль неверный.");
        }
#warning проверить надо метод.
    }

    private static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    private static bool ValidatePassword(string hash, string password) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}
