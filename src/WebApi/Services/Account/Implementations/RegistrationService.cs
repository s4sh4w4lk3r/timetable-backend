using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;
using Repository;
using System.Net.Mail;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class RegistrationService : IRegistrationService
{
    private readonly SqlDbContext _dbContext;
    private readonly IValidator<User> _userValidator;
    private readonly DbSet<User> _users;
    private readonly ApprovalService _approvalService;

    public RegistrationService(SqlDbContext dbContext, IValidator<User> validator, ApprovalService approvalService)
    {
        _dbContext = dbContext;
        _userValidator = validator;
        _users = _dbContext.Set<User>();
        _approvalService = approvalService;
    }
    public async Task<ServiceResult> AddUserToRepoAsync(User user, CancellationToken cancellationToken = default)
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
    public async Task<ServiceResult> ConfirmAsync(string userEmail, int approvalCode, CancellationToken cancellationToken = default)
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

        var approvalServiceResult = await _approvalService.VerifyCodeAsync(validUser.UserId, approvalCode, ApprovalCode.ApprovalCodeType.Registration, cancellationToken: cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения регистрации не принят.", approvalServiceResult);
        }

        validUser.IsEmailConfirmed = true;
        _dbContext.Set<User>().Update(validUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Email пользователя подтвержден.");
    }
    public async Task<ServiceResult> SendEmailAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var emailOk = StaticValidator.ValidateEmail(userEmail);
        if (emailOk is false)
        {
            return new ServiceResult(false, "Email имеет неправильный формат.");
        }

        var sendApprovalResult = await _approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return ServiceResult.Fail("Письмо подтверждения регистрации не было отправлено.");
        }

        return ServiceResult.Ok(sendApprovalResult.Description);
    }
}
