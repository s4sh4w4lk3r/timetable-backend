using Core.LessonTimes;

namespace Core.Timetables.Cells
{
    /// <summary>
    /// Ячейка расписания
    /// </summary>
    public class TimetableCell
    {
        public LessonTime? LessonTime { get; init; }
        public Cabinet? Cabinet { get; init; }
        public Teacher? Teacher { get; init; }
        public Subject? Subject { get; init; }

        public TimetableCell(LessonTime lessonTime, Cabinet cabinet, Teacher teacher, Subject subject)
        {
            subject.ThrowIfNull();
            cabinet.ThrowIfNull();
            teacher.ThrowIfNull();
            lessonTime.ThrowIfNull();

            Subject = subject;
            Cabinet = cabinet;
            Teacher = teacher;
            LessonTime = lessonTime;
        }
    }
}
