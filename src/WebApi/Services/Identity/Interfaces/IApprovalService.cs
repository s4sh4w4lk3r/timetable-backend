using Core.Entities.Identity;
using static Core.Entities.Identity.Approval;

namespace WebApi.Services.Identity.Interfaces
{
    public interface IApprovalService
    {
        Task<ServiceResult<Approval>> VerifyAndRevokeCodeAsync(int userId, int approvalCode, ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default);
    }
}
