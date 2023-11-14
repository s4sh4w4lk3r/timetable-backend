using Models.Entities.Identity;
using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.Services.Identity.Interfaces
{
    public interface IRegistrationEntityService
    {
        public Task<ServiceResult> RemoveRegistrationEntityAsync(RegistrationEntity registrationEntity, CancellationToken cancellationToken = default);
        public Task<ServiceResult<RegistrationEntity>> CheckRegistrationEntityExistsAsync(string registrationCode, RegistrationEntity.Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создает коды для регистрации указанного количества пользователей с указанной ролью.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="numberOfCodes"></param>
        /// <param name="studentGroupId">Id группы, в которую надо закинуть студента. Если роль != student, но указан id, то id будет проигнорирован.</param>
        /// <param name="subGroup">Enum подругуппы, в которую входит студент, если равен нулю, то остается без подгруппы молодой.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ServiceResult<IEnumerable<string?>?>> CreateAndSaveRegistrationEntitesAsync(RegistrationEntity.Role role, int numberOfCodes = 1, int studentGroupId = 0, SubGroup subGroup = 0, CancellationToken cancellationToken = default);
    }
}
