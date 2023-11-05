using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public abstract class TimetableCell
    {
        public int TimetableCellId { get; init; }
        public required Teacher? Teacher { get; set; }
        public required Subject? Subject { get; init; }
        public required Cabinet? Cabinet { get; set; }
        public required LessonTime? LessonTime { get; init; }

        public required int TeacherId { get; set; }
        public required int SubjectId { get; init; }
        public required int CabinetId { get; set; }
        public required int LessonTimeId { get; init; }

        protected TimetableCell() { }

        [SetsRequiredMembers]
        protected TimetableCell(int timetableCellId, Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime)
        {
            TimetableCellId = timetableCellId;
            Teacher = teacher;
            Subject = subject;
            Cabinet = cabinet;
            LessonTime = lessonTime;

            TeacherId = teacher.TeacherId;
            SubjectId = subject.SubjectId;
            CabinetId = cabinet.CabinetId;
            LessonTimeId = lessonTime.LessonTimeId;

            Teacher.ThrowIfNull();
            Subject.ThrowIfNull();
            Cabinet.ThrowIfNull();
            LessonTime.ThrowIfNull();

            TeacherId.Throw().IfDefault();
            SubjectId.Throw().IfDefault();
            CabinetId.Throw().IfDefault();
            LessonTimeId.Throw().IfDefault();
        }
    }
}
