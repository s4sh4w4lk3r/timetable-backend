using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITeacherRepository
{
    Task<Teacher?> GetTeacher(int id);
    Task<List<Teacher>?> GetTeacherList();
    Task<List<Teacher>?> GetTeacherList(Predicate<Teacher> predicate);
    Task<bool> InsertTeacher(Teacher teacher);
    Task<bool> DeleteTeacher(int id);
    Task<bool> UpdateTeacher(Teacher teacher);
}
