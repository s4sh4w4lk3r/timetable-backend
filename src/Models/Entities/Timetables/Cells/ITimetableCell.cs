using Models.Entities.Timetables.Cells.CellMembers;
using Models.Users;

namespace Models.Entities.Timetables.Cells
{
    public interface ITimetableCell
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
    }
}
