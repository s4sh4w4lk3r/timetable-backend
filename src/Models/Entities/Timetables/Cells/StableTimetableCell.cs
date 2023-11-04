using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public class StableTimetableCell : TimetableCell
    {
        public required bool IsWeekEven { get; init; }
        public required DayOfWeek DayOfWeek { get; init; }

        private StableTimetableCell() : base() { }

        [SetsRequiredMembers]
        public StableTimetableCell(int stableTimetableCellId, Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime, bool isWeekEven, DayOfWeek dayOfWeek)
            : base(stableTimetableCellId, teacher, subject, cabinet, lessonTime)
        {
            dayOfWeek.Throw().IfLessThan(e => (int)e, 0);
            dayOfWeek.Throw().IfGreaterThan(e => (int)e, 6);
            IsWeekEven = isWeekEven;
            DayOfWeek = dayOfWeek;
        }
    }
}
