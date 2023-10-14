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

    public ApprovalService(IRepository<ApprovalCode> approvalCodeRepository, IValidator<User> validator)
    {
        _approvalCodeRepository = approvalCodeRepository;
        _userValidator = validator;
    }

    public async Task<bool> VerifyCodeAsync(User user, int approvalCode, ApprovalCode.ApprovalCodeType approvalCodeType, CancellationToken cancellationToken = default)
    {
#warning метод не проверен
        if (_userValidator.Validate(user).IsValid is false)
        { 
            return false;
        }

        var approval = await _approvalCodeRepository.Entites
            .Where(e => e.User!.Email == user.Email && e.User.UserId == user.UserId 
            && e.Code == approvalCode && e.CodeType == approvalCodeType)
            .FirstOrDefaultAsync(cancellationToken);

        if (approval is not null && approval.IsNotExpired())
        {
            await RevokeAsync(approval, cancellationToken: cancellationToken);
            return true;
        }
        else return false;
    }

    public async Task<bool> RevokeAsync(ApprovalCode approvalCode, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        if (approvalCode is null) 
        {
            return false; 
        }

        if (deleteRequired)
        {
            await _approvalCodeRepository.DeleteAsync(approvalCode, cancellationToken); 
            return true;
        }
        else
        {
            approvalCode.SetRevoked();
            await _approvalCodeRepository.UpdateAsync(approvalCode, cancellationToken);
#warning проверить нормально ли обновляется
            return true;
        }
    }

    public bool SendCode(User user, ApprovalCode.ApprovalCodeType approvalCodeType, IEmailClient emailClient)
    {
        if (_userValidator.Validate(user).IsValid is false)
        {
            return false;
        }

        var approval = new ApprovalCode(user, approvalCodeType);

        string message = approvalCodeType switch
        {
            ApprovalCode.ApprovalCodeType.UpdatePassword => $"Код подтверждения для обновления пароля: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.UpdateMail => $"Код подтверждения для смены адреса почты: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Registration => $"Код подтверждения для регистрации: {approval.Code}.",
            ApprovalCode.ApprovalCodeType.Unregistration => $"Код подтверждения для удаления аккаунта: {approval.Code}.",
            _ => throw new NotImplementedException()
        };

        emailClient.SendEmail(message, user.Email!);
        return true;
    }
}
