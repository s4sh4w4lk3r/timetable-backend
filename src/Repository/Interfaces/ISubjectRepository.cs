using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ISubjectRepository
{
    IQueryable<Subject> Subjects { get; }
    Task<bool> InsertSubject(Subject subject);
    Task<bool> DeleteSubject(int id);
    Task<bool> UpdateSubject(Subject subject);
}
