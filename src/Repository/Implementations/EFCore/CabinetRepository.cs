using FluentValidation;
using Models.Entities.Timetables.Cells;
using Models.Entities.Users;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

internal class CabinetRepository : ICabinetRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public CabinetRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Cabinet> Cabinets => _context.Cabinets.AsQueryable();

    public async Task DeleteCabinetAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Cabinets.FirstOrDefault(a => a.CabinetId == id);
        entityToDel.ThrowIfNull();

        _context.Cabinets.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertCabinetAsync(Cabinet cabinet)
    {
        new CabinetValidator().ValidateAndThrow(cabinet);

        _context.Cabinets.Add(cabinet);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateCabinetAsync(Cabinet cabinet)
    {
        new CabinetValidator().ValidateAndThrow(cabinet);

        var entityEntry = _context.Cabinets.Entry(cabinet);
        _context.Cabinets.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
