using Models.Entities.Identity;
using static Models.Entities.Identity.Approval;

namespace WebApi.Services.Identity.Interfaces
{
    public interface IApprovalService
    {
        Task<ServiceResult<Approval>> VerifyAndRevokeCodeAsync(int userId, int approvalCode, ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default);
    }
}
