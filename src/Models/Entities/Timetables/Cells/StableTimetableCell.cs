using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public class StableTimetableCell : TimetableCell
    {
        public required bool IsWeekEven { get; init; }

        private StableTimetableCell() : base() { }

        [SetsRequiredMembers]
        public StableTimetableCell(Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime, bool isWeekEven)
            : base(teacher, subject, cabinet, lessonTime)
        {
            IsWeekEven = isWeekEven;
        }
    }
}
