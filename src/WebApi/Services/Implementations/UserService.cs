using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Entities.Users.Auth;
using Repository.Interfaces;
using WebApi.Services.Implementations;

namespace Services.Implementations;

public class UserService
{
    private readonly IRepository<User> _userRepo;
    private readonly IValidator<User> _validator;
    private readonly ApprovalService _approvalService;

    public UserService(IRepository<User> repository, IValidator<User> validator, ApprovalService approvalService)
    {
        _userRepo = repository;
        _validator = validator;
        _approvalService = approvalService;
    }

    public async Task<bool> Register(User user, int approvalCode, CancellationToken cancellationToken = default)
    {
#warning проверить работу
        if (_validator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching")).IsValid is false)
        {
            return false;
        }

        if ((await _userRepo.Entites.AnyAsync(x => x.Email == user.Email, cancellationToken)) is true)
        {
            return false;
        }

        if (await _approvalService.VerifyCodeAsync(user, approvalCode, ApprovalCode.ApprovalCodeType.Registration, cancellationToken) is true)
        {
            user.Password = HashPassword(user.Password!);
            await _userRepo.InsertAsync(user, cancellationToken);
            return true;
        }
        else return false;
    }

    public async Task<bool> Unregister(User user, int approvalCode, CancellationToken cancellationToken = default)
    {
#warning проверить работу
        if (_validator.Validate(user, o => o.IncludeRuleSets("default")).IsValid is false)
        {
            return false;
        }

        if (await _approvalService.VerifyCodeAsync(user, approvalCode, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken) is true)
        {
            await _userRepo.DeleteAsync(user, cancellationToken);
            return true;
        }
        else return false;
    }

    public async Task<bool> UpdateAsync(User newUser, int approvalCode, CancellationToken cancellationToken = default)
    {
#warning проверить надо будет
        if (_validator.Validate(newUser, o => o.IncludeRuleSets("default")).IsValid is false)
        {
            return false;
        }

        var validUser = await _userRepo.Entites.FirstOrDefaultAsync(e => e.UserId == newUser.UserId, cancellationToken);
        if (validUser is null || (_validator.Validate(validUser).IsValid is false))
        {
            return false;
        }

        if (newUser.Email != validUser.Email
            && (await _approvalService.VerifyCodeAsync(validUser, approvalCode, ApprovalCode.ApprovalCodeType.UpdateMail, cancellationToken) is true))
        {
            validUser.Email = newUser.Email;
            await _userRepo.UpdateAsync(validUser, cancellationToken);
            return true;
        }

        if (ValidatePassword(validUser.Password!, newUser.Password!) is false)
        {
            if (_validator.Validate(validUser, o => o.IncludeRuleSets("default", "password_regex")).IsValid is false)
            {
                return false;
            }

            if (await _approvalService.VerifyCodeAsync(validUser, approvalCode, ApprovalCode.ApprovalCodeType.UpdatePassword, cancellationToken) is true)
            {
                validUser.Password = HashPassword(newUser.Password!);
                await _userRepo.UpdateAsync(validUser, cancellationToken);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> CheckLoginDataAsync(User user, CancellationToken cancellationToken = default)
    {
        if (_validator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching")).IsValid is false) return false;

        string? hashFromRepo = await _userRepo.Entites.Where(e => e.Email == user.Email).Select(e => e.Password).FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(hashFromRepo)) return false;

        return ValidatePassword(hashFromRepo, user.Password!);
    }

    private static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    private static bool ValidatePassword(string hash, string password) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}
