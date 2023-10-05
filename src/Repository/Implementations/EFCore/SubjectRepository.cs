using FluentValidation;
using Models.Entities.Timetables.Cells;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class SubjectRepository : ISubjectRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public SubjectRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Subject> Subjects => _context.Subjects.AsQueryable();

    public async Task DeleteSubjectAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Subjects.FirstOrDefault(a => a.SubjectId == id);
        entityToDel.ThrowIfNull();

        _context.Subjects.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertSubjectAsync(Subject subject)
    {
        new SubjectValidator().ValidateAndThrow(subject);

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateSubjectAsync(Subject subject)
    {
        new SubjectValidator().ValidateAndThrow(subject);

        var entityEntry = _context.Subjects.Entry(subject);
        _context.Subjects.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
