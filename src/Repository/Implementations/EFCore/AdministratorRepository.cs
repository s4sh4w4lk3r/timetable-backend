using FluentValidation;
using Models.Entities.Users;
using Models.Validation.AllProperties;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.MySql;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;
    public AdministratorRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Administrator> Administrators => _context.Set<Administrator>().AsQueryable();

    public async Task DeleteAdministratorAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Administrators.FirstOrDefault(a => a.UserId == id);
        entityToDel.ThrowIfNull();

        _context.Administrators.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertAdministratorAsync(Administrator administrator)
    {
        new AdminstratorValidator().ValidateAndThrow(administrator);

        _context.Administrators.Add(administrator);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateAdministratorAsync(Administrator administrator)
    {
        new AdminstratorValidator().ValidateAndThrow(administrator);

        var entityEntry = _context.Administrators.Entry(administrator);
        _context.Administrators.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
