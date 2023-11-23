using Core.Entities.Timetables.Cells.CellMembers;

namespace Core.Entities.Timetables.Cells
{
    public class ActualTimetableCell : ITimetableCell
    {
        public int TimetableCellId { get; init; }
        public TeacherCM? Teacher { get; set; }
        public Subject? Subject { get; set; }
        public Cabinet? Cabinet { get; set; }
        public LessonTime? LessonTime { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int CabinetId { get; set; }
        public int LessonTimeId { get; set; }
        public DateOnly Date { get; init; }
        public bool IsModified { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public bool IsMoved { get; set; } = false;
        public SubGroup SubGroup { get; set; }

        private ActualTimetableCell() { }

        public ActualTimetableCell(int timetableCellId, TeacherCM? teacher, Subject? subject, Cabinet? cabinet, LessonTime? lessonTime, DateOnly dateOnly)
        {
            TimetableCellId = timetableCellId;
            Teacher = teacher;
            Subject = subject;
            Cabinet = cabinet;
            LessonTime = lessonTime;
            Date = dateOnly;
        }

        public ActualTimetableCell(int timetableCellId, int teacherId, int subjectId, int cabinetId, int lessonTimeId, SubGroup subGroup, DateOnly dateOnly)
        {
            TimetableCellId = timetableCellId;
            TeacherId = teacherId;
            SubjectId = subjectId;
            CabinetId = cabinetId;
            LessonTimeId = lessonTimeId;
            SubGroup = subGroup;
            Date = dateOnly;
        }
    }
}
