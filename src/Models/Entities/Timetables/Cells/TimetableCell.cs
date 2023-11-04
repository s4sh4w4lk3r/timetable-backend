using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public abstract class TimetableCell
    {
        public required Teacher Teacher { get; set; }
        public required Subject Subject { get; init; }
        public required Cabinet Cabinet { get; set; }
        public required LessonTime LessonTime { get; init; }

        protected TimetableCell() { }

        [SetsRequiredMembers]
        protected TimetableCell(Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime)
        {
            Teacher = teacher;
            Subject = subject;
            Cabinet = cabinet;
            LessonTime = lessonTime;
        }
    }
}
