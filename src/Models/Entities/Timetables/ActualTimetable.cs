using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public class ActualTimetable : Timetable
    {
        public required int WeekNumber { get; init; }
        public required IEnumerable<ActualTimetableCell>? ActualTimetableCells { get; init; }

        private ActualTimetable() : base() { }

        [SetsRequiredMembers]
        public ActualTimetable(int actualTimetableId, Group group, IEnumerable<ActualTimetableCell> actualTimetableCells, int weekNumber) : base(actualTimetableId, group)
        {
            actualTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            weekNumber.Throw().IfDefault();
            ActualTimetableCells = actualTimetableCells;
            WeekNumber = weekNumber;
        }
    }
}
