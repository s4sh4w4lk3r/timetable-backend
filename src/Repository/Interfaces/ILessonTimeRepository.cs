using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ILessonTimeRepository
{
    IQueryable<LessonTime> LessonTimes { get; }
    Task<bool> InsertLessonTimeAsync(LessonTime lessonTime);
    Task<bool> DeleteLessonTimeAsync(int id);
    Task<bool> UpdateLessonTimeAsync(LessonTime lessonTime);
}
