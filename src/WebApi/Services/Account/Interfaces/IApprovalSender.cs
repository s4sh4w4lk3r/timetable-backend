using Models.Entities.Identity;

namespace WebApi.Services.Account.Interfaces
{
    public interface IApprovalSender
    {
        Task<ServiceResult> SendUnregistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default);
        Task<ServiceResult> SendRegistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default);
        Task<ServiceResult<Approval?>> SendEmailUpdateCodeAsync(int userId, string userEmail, CancellationToken cancellationToken = default);
    }
}
