using Models.Entities.Users;
using static Models.Entities.Users.ApprovalCode;

namespace WebApi.Services.Account.Interfaces
{
    public interface IApprovalService
    {
        Task<ServiceResult<ApprovalCode>> VerifyAndRevokeCodeAsync(int userId, int approvalCode, ApprovalCodeType approvalCodeType, bool deleteRequired = true, CancellationToken cancellationToken = default);
    }
}
