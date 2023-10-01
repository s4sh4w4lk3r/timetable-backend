using Models.Entities.Users;

namespace Repository.Interfaces;

internal interface IAdministratorRepository
{
    Task<Administrator?> GetAdministrator(int id);
    Task<List<Administrator>?> GetAdministratorList();
    Task<List<Administrator>?> GetAdministratorList(Predicate<Administrator> predicate);
    Task<bool> InsertAdministrator(Administrator administrator);
    Task<bool> DeleteAdministrator(int id);
    Task<bool> UpdateAdministrator(Administrator administrator);
}
