﻿using Models.Entities.Identity;
using Models.Entities.Identity.Users;

namespace WebApi.Services.Identity.Interfaces
{
    public interface IRegistrationService
    {
        /// <summary>
        /// Заносит пользователя в базу данных с неподтвержденным Email для дальнейшего подтверждения. Email отправляется другим методом.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResult> AddUserToRepoAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверяет код полученный из Email и делает учетную запись подтвержденной.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="approvalCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResult> ConfirmAsync(string userEmail, int approvalCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Отправляет код подтверждения регистрации на Email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ServiceResult> SendEmailAsync(string userEmail, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создает коды для регистрации указанного количества пользователей с указанной ролью.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="numberOfLinks"></param>
        /// <param name="studentGroupId">Id группы, в которую надо закинуть студента. Если роль != student, но указан id, то id будет проигнорирован.</param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<string?>?>> CreateAndSaveRegisterCodes(RegistrationEntity.Role role, int numberOfLinks = 1, int studentGroupId = 0);
    }
}
