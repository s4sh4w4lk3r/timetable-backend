using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Repository;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class ApprovalService : IApprovalService
{
    private readonly TimetableContext _dbContext;

    public ApprovalService(TimetableContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResult<Approval>> VerifyAndRevokeCodeAsync(int userId, int approvalCode, Approval.ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        var approval = await _dbContext.Set<Approval>()
            .Where(e => e.User!.UserId == userId
            && e.Code == approvalCode
            && e.CodeType == approvalCodeType
            && e.IsRevoked == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (approval is null)
        {
            return ServiceResult<Approval>.Fail("Код подтверждения не был найден в бд.", null);
        }

        if (approval.IsNotExpired() is false)
        {
            _dbContext.Set<Approval>().Remove(approval);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult<Approval>.Fail("Код подтверждения просрочен.", null);
        }

        await RevokeAsync(approval, deleteRequired: deleteRequired, cancellationToken: cancellationToken);
        return ServiceResult<Approval>.Ok("Код подтверждения был подтвержден.", approval);
    }
    private async Task<ServiceResult> RevokeAsync(Approval approvalCode, bool deleteRequired = true, CancellationToken cancellationToken = default)
    {
        if (approvalCode is null)
        {
            return new ServiceResult(false, "approvalCode is null.");
        }

        if (deleteRequired)
        {
            _dbContext.Set<Approval>().Remove(approvalCode);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResult(true, "Код подтверждения был удален из бд.");
        }
        else
        {
            approvalCode.SetRevoked();
            _dbContext.Set<Approval>().Update(approvalCode);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResult(true, "Код подтверждения был помечен как использованный в бд.");
        }
    }
}
