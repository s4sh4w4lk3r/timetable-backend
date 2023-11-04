using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public abstract class Timetable
    {
        public required Group Group { get; init; }
        public required IEnumerable<TimetableCell> TimetableCells { get; init; }

        protected Timetable() { }

        [SetsRequiredMembers]
        protected Timetable(Group group, IEnumerable<TimetableCell> timetableCells)
        {
            timetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            group.ThrowIfNull();
            Group = group;
            TimetableCells = timetableCells;
        }
    }
}
