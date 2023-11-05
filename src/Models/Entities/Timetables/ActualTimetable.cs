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
            EnsureNoDuplicates();
        }

        public override bool CheckNoDuplicates()
        {
            ActualTimetableCells.ThrowIfNull().IfHasNullElements().IfEmpty();

            foreach (var item in ActualTimetableCells)
            {
                int count = ActualTimetableCells.Count(x => x.LessonTime == item.LessonTime && x.Date == item.Date);
                if (count > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
