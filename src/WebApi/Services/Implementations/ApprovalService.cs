using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Entities.Users.Auth;
using Repository.Interfaces;
using Services.Interfaces;

namespace WebApi.Services.Implementations;

public class ApprovalService
{
    private readonly IRepository<ApprovalCode> _approvalCodeRepository;
    private readonly IValidator<User> _userValidator;
    private readonly IEmailClient _emailClient;

    public ApprovalService(IRepository<ApprovalCode> approvalCodeRepository, IValidator<User> validator, IEmailClient emailClient)
    {
        _approvalCodeRepository = approvalCodeRepository;
        _userValidator = validator;
        _emailClient = emailClient;
    }

    public async Task<ServiceResult> VerifyCodeAsync(User user, int approvalCode, ApprovalCode.ApprovalCodeType approvalCodeType, CancellationToken cancellationToken = default)
    {
        var valResult = _userValidator.Validate(user);
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        var approval = await _approvalCodeRepository.Entites
            .Where(e => e.User!.UserId == user.UserId 
            && e.Code == approvalCode && e.CodeType == approvalCodeType)
            .FirstOrDefaultAsync(cancellationToken);

        if (approval == null)
        {
            return new ServiceResult(false, "Код подтверждения не был найден в бд.");
        }

        if (approval.IsNotExpired() is false)
        {
            return new ServiceResult(false, "Код подтверждения просрочен.");
        }

        await RevokeAsync(approval, cancellationToken: cancellationToken);
        return new ServiceResult(true, "Код подтверждения был подтвержден.");
        #warning метод не проверен
    }

    private async Task<ServiceResult> RevokeAsync(ApprovalCode approvalCode, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        if (approvalCode is null) 
        {
            return new ServiceResult(false, "approvalCode is null.");
        }

        if (deleteRequired)
        {
            await _approvalCodeRepository.DeleteAsync(approvalCode, cancellationToken);
            return new ServiceResult(true, "Код подтверждения был удален из бд.");
        }
        else
        {
            approvalCode.SetRevoked();
            await _approvalCodeRepository.UpdateAsync(approvalCode, cancellationToken);
#warning проверить нормально ли обновляется
            return new ServiceResult(true, "Код подтверждения был помечен как использованный в бд.");
        }
    }

    public async Task<ServiceResult> SendCodeAsync(User user, ApprovalCode.ApprovalCodeType approvalCodeType)
    {
        var valResult = _userValidator.Validate(user);
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        var approval = new ApprovalCode(user, approvalCodeType);
        var sendTask = _approvalCodeRepository.InsertAsync(approval, cancellationToken);

        string message = approvalCodeType switch
        {
            ApprovalCode.ApprovalCodeType.UpdatePassword => $"Код подтверждения для обновления пароля: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.UpdateMail => $"Код подтверждения для смены адреса почты: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Registration => $"Код подтверждения для регистрации: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Unregistration => $"Код подтверждения для удаления аккаунта: {approval.Code}.",
            _ => throw new NotImplementedException()
        };

        _emailClient.SendEmail(message, user.Email!);
        await sendTask;
        return new ServiceResult(true, "Код подтверждения должен будет отправится.");
    }
}
