using FluentValidation;
using Models.Entities.Timetables.Cells;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class TeacherRepository : ITeacherRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public TeacherRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Teacher> Teachers => _context.Teachers.AsQueryable();

    public async Task DeleteTeacherAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Teachers.FirstOrDefault(a => a.TeacherId == id);
        entityToDel.ThrowIfNull();

        _context.Teachers.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertTeacherAsync(Teacher teacher)
    {
        new TeacherValidator().ValidateAndThrow(teacher);

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateTeacherAsync(Teacher teacher)
    {
        new TeacherValidator().ValidateAndThrow(teacher);

        var entityEntry = _context.Teachers.Entry(teacher);
        _context.Teachers.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
