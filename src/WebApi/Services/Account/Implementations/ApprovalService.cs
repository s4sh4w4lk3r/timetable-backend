using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;
using Repository;
using WebApi.Services.Account.Interfaces;
using static Models.Entities.Users.ApprovalCode;

namespace WebApi.Services.Account.Implementations;

public class ApprovalService
{
    private readonly SqlDbContext _dbContext;
    private readonly IEmailClient _emailClient;

    public ApprovalService(SqlDbContext dbContext, IEmailClient emailClient)
    {
        _dbContext = dbContext;
        _emailClient = emailClient;
    }

    public async Task<ServiceResult<ApprovalCode>> VerifyCodeAsync(int userId, int approvalCode, ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        var approval = await _dbContext.Set<ApprovalCode>()
            .Where(e => e.User!.UserId == userId
            && e.Code == approvalCode && e.CodeType == approvalCodeType)
            .FirstOrDefaultAsync(cancellationToken);

        if (approval is null)
        {
            return ServiceResult<ApprovalCode>.Fail("Код подтверждения не был найден в бд.", null);
        }

        if (approval.IsNotExpired() is false)
        {
            _dbContext.Set<ApprovalCode>().Remove(approval);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult<ApprovalCode>.Fail("Код подтверждения просрочен.", null);
        }

        await RevokeAsync(approval, deleteRequired: deleteRequired, cancellationToken: cancellationToken);
        return ServiceResult<ApprovalCode>.Ok("Код подтверждения был подтвержден.", approval);
    }

    private async Task<ServiceResult> RevokeAsync(ApprovalCode approvalCode, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        if (approvalCode is null)
        {
            return new ServiceResult(false, "approvalCode is null.");
        }

        if (deleteRequired)
        {
            _dbContext.Set<ApprovalCode>().Remove(approvalCode);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResult(true, "Код подтверждения был удален из бд.");
        }
        else
        {
            approvalCode.SetRevoked();
            _dbContext.Set<ApprovalCode>().Update(approvalCode);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResult(true, "Код подтверждения был помечен как использованный в бд.");
        }
    }

    public async Task<ServiceResult<ApprovalCode?>> SendCodeAsync(string userEmail, ApprovalCodeType approvalCodeType, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(userEmail) is false)
        {
            return ServiceResult<ApprovalCode?>.Fail("Email имеет неверный формат или неуказан вовсе.", null);
        }

        return approvalCodeType switch
        {
            ApprovalCodeType.Registration => await SendRegistrationCodeAsync(userEmail, cancellationToken),
            ApprovalCodeType.Unregistration => await SendUnregistrationCodeAsync(userEmail, cancellationToken),
            ApprovalCodeType.UpdateMail => ServiceResult<ApprovalCode?>.Fail("Для обновления Email в Approval сервисе есть метод SendUpdateMailCodeAsync", null),
            _ => ServiceResult<ApprovalCode?>.Fail("Выбран неподдерживаемый enum.", null),
        };
    }

    private async Task<ServiceResult<ApprovalCode?>> SendUnregistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default)
    {

        var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.Email == userEmail, cancellationToken);
        if (userFromRepo is null)
        {
            return ServiceResult<ApprovalCode?>.Fail("Пользователь не был найден в бд.", null);
        }

        var approval = new ApprovalCode()
        {
            CodeType = ApprovalCodeType.Unregistration,
            UserId = userFromRepo.UserId
        };
        await _dbContext.Set<ApprovalCode>().AddAsync(approval, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        string message = $"Код подтверждения для удаления аккаунта: {approval.Code}.";

        await _emailClient.SendEmailAsync("Подтверждение удаления аккаунта", message, userFromRepo.Email!, cancellationToken);
        return ServiceResult<ApprovalCode?>.Ok("Код подтверждения должен будет отправится.", approval);
    }

    private async Task<ServiceResult<ApprovalCode?>> SendRegistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default)
    {

        var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.Email == userEmail, cancellationToken);
        if (userFromRepo is null)
        {
            return ServiceResult<ApprovalCode?>.Fail("Пользователь не был найден в бд.", null);
        }

        //Нужно только для тех слуаче, если зареганный пользователь пытается подтвердить свою почту.
        if (userFromRepo.IsEmailConfirmed is true)
        {
            return ServiceResult<ApprovalCode?>.Fail("У пользователя уже подтверждена почта.", null);
        }

        var approval = new ApprovalCode()
        {
            CodeType = ApprovalCodeType.Registration,
            UserId = userFromRepo.UserId
        };
        await _dbContext.Set<ApprovalCode>().AddAsync(approval, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        string message =  $"Код подтверждения для регистрации: {approval.Code}";

        await _emailClient.SendEmailAsync("Подтверждение адреса электронной почты", message, userFromRepo.Email!, cancellationToken);
        return ServiceResult<ApprovalCode?>.Ok("Код подтверждения должен будет отправится.", approval);
    }

    public async Task<ServiceResult<ApprovalCode?>> SendUpdateMailCodeAsync(int userId, string userEmail, CancellationToken cancellationToken = default)
    {

        if (StaticValidator.ValidateEmail(userEmail) is false)
        {
            return ServiceResult<ApprovalCode?>.Fail("Email имеет неверный формат или не указан вовсе.", null);
        }

        if (userId == 0)
        {
            return ServiceResult<ApprovalCode?>.Fail("Было получен UserId равный нулю", null);
        }

        var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken: cancellationToken);
        if (userFromRepo is null)
        {
            return ServiceResult<ApprovalCode?>.Fail("Пользователь не был найден в бд.", null);
        }

        var approval = new ApprovalCode()
        {
            CodeType = ApprovalCodeType.UpdateMail,
            UserId = userFromRepo.UserId
        };
        await _dbContext.Set<ApprovalCode>().AddAsync(approval, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        string message = $"Код подтверждения для смены адреса почты: {approval.Code}.";

        await _emailClient.SendEmailAsync("Подтверждение адреса электронной почты", message, userEmail, cancellationToken);
        return ServiceResult<ApprovalCode?>.Ok("Код подтверждения должен будет отправится.", approval);
    }
}
