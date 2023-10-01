using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITeacherRepository
{
    IQueryable<Teacher> Teachers { get; }
    Task<bool> InsertTeacher(Teacher teacher);
    Task<bool> DeleteTeacher(int id);
    Task<bool> UpdateTeacher(Teacher teacher);
}
