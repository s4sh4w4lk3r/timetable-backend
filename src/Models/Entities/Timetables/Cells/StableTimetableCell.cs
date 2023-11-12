using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Timetables.Cells
{
    public class StableTimetableCell : ITimetableCell
    {
        public int TimetableCellId { get; internal set; }
        public TeacherCM? Teacher { get; internal set; }
        public Subject? Subject { get; internal set; }
        public Cabinet? Cabinet { get; internal set; }
        public LessonTime? LessonTime { get; internal set; }
        public int TeacherId { get; internal set; }
        public int SubjectId { get; internal set; }
        public int CabinetId { get; internal set; }
        public int LessonTimeId { get; internal set; }
        public bool IsWeekEven { get; internal set; }
        public DayOfWeek DayOfWeek { get; internal set; }

        internal StableTimetableCell() { }
    }
}
