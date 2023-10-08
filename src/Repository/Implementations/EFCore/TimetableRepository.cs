using FluentValidation;
using Models.Entities.Timetables;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class TimetableRepository : ITimetableRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public TimetableRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Timetable> Timetables => _context.Timetables;

    public async Task DeleteTimetableAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Timetables.FirstOrDefault(a => a.TimetableId == id);
        entityToDel.ThrowIfNull();

        _context.Timetables.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertTimetableAsync(Timetable timetable)
    {
        new TimetableValidator().ValidateAndThrow(timetable);

        _context.Timetables.Add(timetable);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateTimetableAsync(Timetable timetable)
    {
        new TimetableValidator().ValidateAndThrow(timetable);

        var entityEntry = _context.Timetables.Entry(timetable);
        _context.Timetables.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
