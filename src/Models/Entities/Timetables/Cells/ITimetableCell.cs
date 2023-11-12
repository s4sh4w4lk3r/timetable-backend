using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Timetables.Cells
{
    public interface ITimetableCell
    {
        public int TimetableCellId { get; }
        public TeacherCM? Teacher { get; }
        public Subject? Subject { get; }
        public Cabinet? Cabinet { get; }
        public LessonTime? LessonTime { get; }

        public int TeacherId { get; }
        public int SubjectId { get; }
        public int CabinetId { get; }
        public int LessonTimeId { get; }
    }
}
