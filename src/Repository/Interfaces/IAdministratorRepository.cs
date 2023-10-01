using Models.Entities.Users;

namespace Repository.Interfaces;

public interface IAdministratorRepository
{
    IQueryable<Administrator> Administrators { get; }
    Task<bool> InsertAdministrator(Administrator administrator);
    Task<bool> DeleteAdministrator(int id);
    Task<bool> UpdateAdministrator(Administrator administrator);
}
