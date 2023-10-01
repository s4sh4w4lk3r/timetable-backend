using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ISubjectRepository
{
    Task<Subject?> GetSubject(int id);
    Task<List<Subject>?> GetSubjectList();
    Task<List<Subject>?> GetSubjectList(Predicate<Subject> predicate);
    Task<bool> InsertSubject(Subject subject);
    Task<bool> DeleteSubject(int id);
    Task<bool> UpdateSubject(Subject subject);
}
