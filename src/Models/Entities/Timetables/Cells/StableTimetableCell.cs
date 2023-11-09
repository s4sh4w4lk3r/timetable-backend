using Models.Entities.Timetables.Cells.CellMembers;
using Models.Users;

namespace Models.Entities.Timetables.Cells
{
    public class StableTimetableCell : ITimetableCell
    {
        public bool IsWeekEven { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public int TimetableCellId { get; init; }
        public Teacher? Teacher { get; init; }
        public Subject? Subject { get; init; }
        public Cabinet? Cabinet { get; init; }
        public LessonTime? LessonTime { get; init; }
        public int TeacherId { get; init; }
        public int SubjectId { get; init; }
        public int CabinetId { get; init; }
        public int LessonTimeId { get; init; }

        public StableTimetableCell() { }



        /// <summary>
        /// Преобразовывает ячейку константного расписания в актульное, без замен и отмен.
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <returns></returns>
        public ActualTimetableCell CastToActualCell(DateOnly dateOnly)
        {
            this.Teacher.ThrowIfNull();
            this.Subject.ThrowIfNull();
            this.Cabinet.ThrowIfNull();
            this.LessonTime.ThrowIfNull();

            return new ActualTimetableCell(this.TimetableCellId, this.Teacher, this.Subject, this.Cabinet, this.LessonTime,
                dateOnly, isModified: false, isCanceled: false, isMoved: false);
        }
    }
}
