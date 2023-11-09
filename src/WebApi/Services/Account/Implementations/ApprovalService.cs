using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Repository;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class ApprovalService : IApprovalService
{
    private readonly TimetableContext _dbContext;

    public ApprovalService(TimetableContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResult<ApprovalCode>> VerifyAndRevokeCodeAsync(int userId, int approvalCode, ApprovalCode.ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        var approval = await _dbContext.Set<ApprovalCode>()
            .Where(e => e.User!.UserId == userId
            && e.Code == approvalCode 
            && e.CodeType == approvalCodeType 
            && e.IsRevoked == false)
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
}
