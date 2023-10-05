using FluentValidation;
using Models.Entities.Timetables.Cells;
using Models.Entities.Users;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class LessonTimeRepository : ILessonTimeRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public LessonTimeRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<LessonTime> LessonTimes => _context.LessonTimes.AsQueryable();

    public async Task DeleteLessonTimeAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.LessonTimes.FirstOrDefault(a => a.LessonTimeId == id);
        entityToDel.ThrowIfNull();

        _context.LessonTimes.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertLessonTimeAsync(LessonTime lessonTime)
    {
        new LessonTimeValidator().ValidateAndThrow(lessonTime);

        _context.LessonTimes.Add(lessonTime);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateLessonTimeAsync(LessonTime lessonTime)
    {
        new LessonTimeValidator().ValidateAndThrow(lessonTime);

        var entityEntry = _context.LessonTimes.Entry(lessonTime);
        _context.LessonTimes.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
