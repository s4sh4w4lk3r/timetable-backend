using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ISubjectRepository
{
    IQueryable<Subject> Subjects { get; }
    Task InsertSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(int id);
    Task UpdateSubjectAsync(Subject subject);
}
