using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Repository;
using System.Net.Mail;

namespace WebApi.Services.Account.Implementations;

public class RegisterService
{
    private readonly SqlDbContext _dbContext;
    private readonly IValidator<User> _userValidator;
    private readonly DbSet<User> _users;

    public RegisterService(SqlDbContext dbContext, IValidator<User> validator)
    {
        _dbContext = dbContext;
        _userValidator = validator;
        _users = _dbContext.Set<User>();
    }
    public async Task<ServiceResult> RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
        var userVal = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex_matching"));
        if (userVal.IsValid is false)
        {
            return new ServiceResult(false, userVal.ToString());
        }

        if (await _users.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == true, cancellationToken) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд.");
        }

        if (await _users.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == false, cancellationToken) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд, но Email не подтвержден.");
        }

        user.IsEmailConfirmed = false;
        user.Password = PasswordService.HashPassword(user.Password!);
        await _users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Пользователь добавлен в базу, но имеет не подтвержденный Email. Запросите отправку email.");
    }

    public async Task<ServiceResult> ConfirmEmailAsync(string userEmail, int approvalCode, ApprovalService approvalService, CancellationToken cancellationToken = default)
    {
        if (approvalCode == default)
        {
            return new ServiceResult(false, "Некорректный approvalCode пользователя.");
        }

        var emailOk = MailAddress.TryCreate(userEmail, out _);
        if (emailOk is false)
        {
            return new ServiceResult(false, "Email имеет неправильный формат.");
        }

        var validUser = await _users.Where(e => e.Email == userEmail).SingleOrDefaultAsync(cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь для валидации не был найден в бд.");
        }

        var approvalServiceResult = await approvalService.VerifyCodeAsync(validUser.UserId, approvalCode, ApprovalCode.ApprovalCodeType.Registration, cancellationToken: cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения регистрации не принят.", approvalServiceResult);
        }

        validUser.IsEmailConfirmed = true;
        _dbContext.Set<User>().Update(validUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Email пользователя подтвержден.");
    }

    public async Task<ServiceResult> UnregisterAsync(int userId, int approvalCode, ApprovalService approvalService, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        var validUser = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        var approvalServiceResult = await approvalService.VerifyCodeAsync(validUser.UserId, approvalCode, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken: cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для удаления аккаунта не принят.", approvalServiceResult);
        }

        _users.Remove(validUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Аккаунт пользователя удален.");
    }
}
