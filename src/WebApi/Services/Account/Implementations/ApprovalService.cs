using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class ApprovalService
{
    private readonly DbContext _dbContext;
    private readonly IEmailClient _emailClient;

    public ApprovalService(DbContext dbContext, IEmailClient emailClient)
    {
        _dbContext = dbContext;
        _emailClient = emailClient;
    }

    public async Task<ServiceResult<ApprovalCode>> VerifyCodeAsync(int userId, int approvalCode, ApprovalCode.ApprovalCodeType approvalCodeType, CancellationToken cancellationToken = default)
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

        await RevokeAsync(approval, cancellationToken: cancellationToken);
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

    public async Task<ServiceResult<ApprovalCode?>> SendCodeAsync(string userEmail, ApprovalCode.ApprovalCodeType approvalCodeType, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(userEmail) is false)
        {
            return ServiceResult<ApprovalCode?>.Fail("Email имеет неверный формат.", null);
        }

        var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.Email == userEmail, cancellationToken);
        if (userFromRepo is null)
        {
            return ServiceResult<ApprovalCode?>.Fail("Пользователь не был найден в бд.", null);
        }

        //Нужно только для тех слуаче, если зареганный пользователь пытается подтвердить свою почту.
        if (approvalCodeType is ApprovalCode.ApprovalCodeType.Registration && (userFromRepo.IsEmailConfirmed is true))
        {
            return ServiceResult<ApprovalCode?>.Fail("У пользователя уже подтверждена почта.", null);
        }

        var approval = new ApprovalCode()
        {
            CodeType = approvalCodeType,
            UserId = userFromRepo.UserId
        };
        await _dbContext.Set<ApprovalCode>().AddAsync(approval, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        string message = approvalCodeType switch
        {
            ApprovalCode.ApprovalCodeType.UpdateMail => $"Код подтверждения для смены адреса почты: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Registration => $"Код подтверждения для регистрации: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Unregistration => $"Код подтверждения для удаления аккаунта: {approval.Code}.",
            _ => throw new NotImplementedException()
        };

        _emailClient.SendEmail(message, userFromRepo.Email!);
        return ServiceResult<ApprovalCode?>.Ok("Код подтверждения должен будет отправится.", approval);
    }
}
