using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using Repository;
using Validation;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class RegistrationService : IRegistrationService
{
    private readonly TimetableContext _dbContext;
    private readonly DbSet<User> _users;
    private readonly IApprovalService _approvalService;
    private readonly IApprovalSender _approvalSender;

    public RegistrationService(TimetableContext dbContext, IApprovalService approvalService, IApprovalSender approvalSender)
    {
        _dbContext = dbContext;
        _users = _dbContext.Set<User>();
        _approvalService = approvalService;
        _approvalSender = approvalSender;
    }
    public async Task<ServiceResult> AddUserToRepoAsync(User user, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(user.Email) is false)
        {
            return ServiceResult.Fail("Email имеет неверный формат.");
        }

        if (StaticValidator.ValidatePassword(user.Password) is false)
        {
            return ServiceResult.Fail("Пароль не соответствует минимальным требованиям безопасности.");
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

        var emailOk = StaticValidator.ValidateEmail(userEmail);
        if (emailOk is false)
        {
            return new ServiceResult(false, "Email имеет неправильный формат.");
        }

        var validUser = await _users.Where(e => e.Email == userEmail).SingleOrDefaultAsync(cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь для валидации не был найден в бд.");
        }

        var approvalServiceResult = await _approvalService.VerifyAndRevokeCodeAsync(validUser.UserId, approvalCode, Approval.ApprovalCodeType.Registration, cancellationToken: cancellationToken);
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

        var sendApprovalResult = await _approvalSender.SendRegistrationCodeAsync(userEmail, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return ServiceResult.Fail("Письмо подтверждения регистрации не было отправлено.");
        }

        return ServiceResult.Ok(sendApprovalResult.Description);
    }
}
