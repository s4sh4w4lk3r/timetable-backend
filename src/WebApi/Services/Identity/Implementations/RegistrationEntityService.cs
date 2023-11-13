using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Models.Entities.Timetables;
using Repository;
using Throw;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations
{
    public class RegistrationEntityService : IRegistrationEntityService
    {
        private readonly TimetableContext _dbContext;

        public RegistrationEntityService(TimetableContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<IEnumerable<string?>?>> CreateAndSaveRegistrationEntitesAsync(RegistrationEntity.Role role, int numberOfLinks = 1, int studentGroupId = 0, CancellationToken cancellationToken = default)
        {
            if (numberOfLinks < 1 || numberOfLinks > 1000)
            {
                return ServiceResult<IEnumerable<string?>?>.Fail("Количество запрашиваемых ссылок не должно быть меньше 1 и больше 1000.", null);
            }

            if (Enum.IsDefined(role) is false)
            {
                return ServiceResult<IEnumerable<string?>?>.Fail("Получена несуществующая роль.", null);
            }

            if (role is RegistrationEntity.Role.Student)
            {
                if (studentGroupId < 1)
                {
                    return ServiceResult<IEnumerable<string?>?>.Fail("Для создания роли студента, нужно указать studentGroupId, который должен быть больше 0.", null);
                }

                if (await _dbContext.Set<Group>().AnyAsync(e => e.GroupId == studentGroupId, cancellationToken) is false)
                {
                    return ServiceResult<IEnumerable<string?>?>.Fail("Группы с таким studentGroupId не существует.", null);
                }
            }

            var registrationEntities = new List<RegistrationEntity>();
            for (int i = 0; i < numberOfLinks; i++)
            {
                var regEntity = new RegistrationEntity()
                {
                    CodeExpires = DateTime.UtcNow.AddDays(14),
                    DesiredRole = role,
                    SecretKey = RegistrationEntity.GenerateSecretKey(),
                    StudentGroupId = (role is RegistrationEntity.Role.Student) ? studentGroupId : null
                };

                registrationEntities.Add(regEntity);
            }
            await _dbContext.Set<RegistrationEntity>().AddRangeAsync(registrationEntities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult<IEnumerable<string?>?>.Ok("Коды созданы и сохранены. Они действительны в течени 14-ти суток.", registrationEntities.Select(e => e.SecretKey));
        }
        public async Task<ServiceResult<RegistrationEntity>> CheckRegistrationEntityExistsAsync(string registrationCode, RegistrationEntity.Role role, CancellationToken cancellationToken = default)
        {
            var regEntity = await _dbContext.Set<RegistrationEntity>().SingleOrDefaultAsync(e => e.SecretKey == registrationCode && e.DesiredRole == role, cancellationToken);
            if (regEntity is null)
            {
                return ServiceResult<RegistrationEntity>.Fail("Сочетания такого кода и роли нет в бд.", null);
            }

            if (regEntity.IsCodeNotExpired() is false)
            {
                _dbContext.Set<RegistrationEntity>().Remove(regEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return ServiceResult<RegistrationEntity>.Fail("Данный код регистрации уже истек.", null);
            }

            return ServiceResult<RegistrationEntity>.Ok("Код подтверждения правильный", regEntity);
        }
        public async Task<ServiceResult> RemoveRegistrationEntityAsync(RegistrationEntity registrationEntity, CancellationToken cancellationToken = default)
        {
            registrationEntity.ThrowIfNull();
            _dbContext.Set<RegistrationEntity>().Remove(registrationEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Код подтверждения регистрации удален из бд.");
        }
    }
}
