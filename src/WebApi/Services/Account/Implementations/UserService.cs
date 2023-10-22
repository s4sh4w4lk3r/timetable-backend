using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;

namespace WebApi.Services.Account.Implementations;

public class UserService
{
    private readonly DbContext _dbContext;
    private readonly IValidator<User> _userValidator;
    private readonly DbSet<User> _users;

    public UserService(DbContext dbContext, IValidator<User> validator)
    {
        _dbContext = dbContext;
        _userValidator = validator;
        _users = _dbContext.Set<User>();
    }


    public async Task<ServiceResult> UpdateEmail(User user, int approvalCode, ApprovalService approvalService, CancellationToken cancellationToken = default)
    {
#warning не проверен
        if (user.UserId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        if (StaticValidator.ValidateEmail(user.Email) is false)
        {
            return ServiceResult.Fail("Некорректный формат почты.");
        }

        if (await _users.AnyAsync(e=>e.Email == user.Email) is true)
        {
            return ServiceResult.Fail("Пользователь с таким email уже зарегистрирован.");
        }

        if (approvalCode == default)
        {
            return new ServiceResult(false, "ApprovalCode не может быть равен нулю.");
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == user.UserId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        var approvalServiceResult = await approvalService.VerifyCodeAsync(userFromRepo.UserId, approvalCode, ApprovalCode.ApprovalCodeType.UpdateMail, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для изменения почты не принят.", approvalServiceResult);
        }

        userFromRepo.Email = user.Email;
        _users.Update(userFromRepo);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Email пользователя обновлен.");
    }

    public async Task<ServiceResult> UpdatePassword(int userId, string newPassword, int approvalCode, ApprovalService approvalService,
        CancellationToken cancellationToken = default)
    {
#warning не проверен
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }
        if (approvalCode == default)
        {
            return new ServiceResult(false, "Некорректный approvalCode пользователя.");
        }

        if (StaticValidator.ValidatePassword(newPassword) is false)
        {
            return new ServiceResult(false, "В пароле должно быть не менее 8 символов, большие и маленькие латинские буквы, цифры и спецсимволы #?!@$%^&*-");
        }

        var approvalServiceResult = await approvalService.VerifyCodeAsync(userId, approvalCode, ApprovalCode.ApprovalCodeType.UpdatePassword, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для изменения пароля не принят.", approvalServiceResult);
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь с таким Id не найден.");
        }

        userFromRepo.Password = HashPassword(newPassword);
        _users.Update(userFromRepo);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Пароль пользователя обновлен.");
    }

    public async Task<ServiceResult<User>> CheckLoginDataAsync(User user, CancellationToken cancellationToken = default)
    {
        var valResult = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching"));
        if (valResult.IsValid is false)
        {
            return new ServiceResult<User>(false, valResult.ToString(), null);
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.Email == user.Email, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult<User>(false, "Пользователя нет в бд.", null);
        }

        if (userFromRepo.IsEmailConfirmed is false)
        {
            return new ServiceResult<User>(false, "Пользователь найден, но Email не подтвержден.", null);
        }

        string? hashFromRepo = userFromRepo.Password;
        if (string.IsNullOrWhiteSpace(hashFromRepo))
        {
            return new ServiceResult<User>(false, "Пользователь найден, но его пароль почему-то пуст.", null);
        }

        if (ValidatePassword(hashFromRepo, user.Password!) is true)
        {
            return new ServiceResult<User>(true, "Пароль подтвержден.", userFromRepo);
        }
        else
        {
            return new ServiceResult<User>(false, "Пароль неверный.", null);
        }
    }

    public async Task<ServiceResult> CheckUserExist(int id, CancellationToken cancellationToken = default)
    {
        if (id == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        if (await _users.AnyAsync(e => e.UserId == id, cancellationToken) is false)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        return new ServiceResult(true, "Пользователь успешно найден в бд.");
    }
    public static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public static bool ValidatePassword(string hash, string password) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}
