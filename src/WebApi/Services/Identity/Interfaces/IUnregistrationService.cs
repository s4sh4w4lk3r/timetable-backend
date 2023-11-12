namespace WebApi.Services.Identity.Interfaces
{
    public interface IUnregistrationService
    {
        /// <summary>
        /// Удаляет аккаунт пользователя в случае, если код верный.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="approvalCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResult> ConfirmAsync(int userId, int approvalCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Отправляет пользователю письмо с кодом для удаления аккаунта.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResult> SendEmailAsync(int userId, CancellationToken cancellationToken = default);
    }
}
