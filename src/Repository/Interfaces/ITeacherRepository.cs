using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITeacherRepository
{
    IQueryable<Teacher> Teachers { get; }
    Task InsertTeacherAsync(Teacher teacher);
    Task DeleteTeacherAsync(int id);
    Task UpdateTeacherAsync(Teacher teacher);
}
