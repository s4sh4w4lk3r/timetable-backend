using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ILessonTimeRepository
{
    IQueryable<LessonTime> LessonTimes { get; }
    Task InsertLessonTimeAsync(LessonTime lessonTime);
    Task DeleteLessonTimeAsync(int id);
    Task UpdateLessonTimeAsync(LessonTime lessonTime);
}
