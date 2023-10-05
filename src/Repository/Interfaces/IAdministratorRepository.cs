using Models.Entities.Users;

namespace Repository.Interfaces;

public interface IAdministratorRepository
{
    IQueryable<Administrator> Administrators { get; }
    Task InsertAdministratorAsync(Administrator administrator);
    Task DeleteAdministratorAsync(int id);
    Task UpdateAdministratorAsync(Administrator administrator);
}
