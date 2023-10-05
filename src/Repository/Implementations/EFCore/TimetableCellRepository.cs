using FluentValidation;
using Models.Entities.Timetables.Cells;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class TimetableCellRepository : ITimetableCellRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public TimetableCellRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<TimetableCell> TimetableCells => _context.TimetableCells;

    public async Task DeleteTimetableCellAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.TimetableCells.FirstOrDefault(a => a.TimeTableCellId == id);
        entityToDel.ThrowIfNull();

        _context.TimetableCells.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertTimetableCellAsync(TimetableCell timetableCell)
    {
        new TimetableCellValidator().ValidateAndThrow(timetableCell);

        _context.TimetableCells.Add(timetableCell);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateTimetableCellAsync(TimetableCell timetableCell)
    {
        new TimetableCellValidator().ValidateAndThrow(timetableCell);

        var entityEntry = _context.TimetableCells.Entry(timetableCell);
        _context.TimetableCells.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
