using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ILessonTimeRepository
{
    Task<LessonTime?> GetLessonTime(int id);
    Task<List<LessonTime>?> GetLessonTimeList();
    Task<List<LessonTime>?> GetLessonTimeList(Predicate<LessonTime> predicate);
    Task<bool> InsertLessonTime(LessonTime lessonTime);
    Task<bool> DeleteLessonTime(int id);
    Task<bool> UpdateLessonTime(LessonTime lessonTime);
}
