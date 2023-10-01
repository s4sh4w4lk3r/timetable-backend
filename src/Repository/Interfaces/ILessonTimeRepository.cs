using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ILessonTimeRepository
{
    IQueryable<LessonTime> LessonTimes { get; }
    Task<bool> InsertLessonTime(LessonTime lessonTime);
    Task<bool> DeleteLessonTime(int id);
    Task<bool> UpdateLessonTime(LessonTime lessonTime);
}
