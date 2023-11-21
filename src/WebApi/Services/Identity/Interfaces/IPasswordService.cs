using Core.Entities.Identity.Users;

namespace WebApi.Services.Identity.Interfaces
{
    public interface IPasswordService
    {
        Task<ServiceResult<User>> CheckLoginDataAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<ServiceResult> UpdatePassword(int userId, string newPassword, CancellationToken cancellationToken = default);
    }
}