using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Timetables.Cells
{
    public class ActualTimetableCell : ITimetableCell
    {
        public int TimetableCellId { get; init; }
        public Teacher? Teacher { get; init; }
        public Subject? Subject { get; init; }
        public Cabinet? Cabinet { get; init; }
        public LessonTime? LessonTime { get; init; }
        public int TeacherId { get; init; }
        public int SubjectId { get; init; }
        public int CabinetId { get; init; }
        public int LessonTimeId { get; init; }
        public DateOnly Date { get; init; }
        public bool IsModified { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public bool IsMoved { get; set; } = false;

        private ActualTimetableCell() { }

        public ActualTimetableCell(int timetableCellId, Teacher? teacher, Subject? subject, Cabinet? cabinet, LessonTime? lessonTime, DateOnly dateOnly)
        {
            TimetableCellId = timetableCellId;
            Teacher = teacher;
            Subject = subject;
            Cabinet = cabinet;
            LessonTime = lessonTime;
            Date = dateOnly;
        }

        public ActualTimetableCell(int timetableCellId, int teacherId, int subjectId, int cabinetId, int lessonTimeId, DateOnly dateOnly)
        {
            TimetableCellId = timetableCellId;
            TeacherId = teacherId;
            SubjectId = subjectId;
            CabinetId = cabinetId;
            LessonTimeId = lessonTimeId;
            Date = dateOnly;
        }
    }
}
