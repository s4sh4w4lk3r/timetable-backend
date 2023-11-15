namespace WebApi.Services.Identity.Interfaces
{
    public interface IEmailUpdater
    {
        Task<ServiceResult> SendUpdateMailAsync(int userId, string newEmail, CancellationToken cancellationToken = default);
        Task<ServiceResult> UpdateEmailAsync(int userId, int approvalCode, CancellationToken cancellationToken = default);
    }
}