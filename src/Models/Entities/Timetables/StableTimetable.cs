using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public class StableTimetable : Timetable
    {
        public required IEnumerable<StableTimetableCell>? StableTimetableCells { get; init; }

        private StableTimetable() : base() { } 

        [SetsRequiredMembers]
        public StableTimetable(int stableTimetableId, Group group, IEnumerable<StableTimetableCell> stableTimetableCells) : base(stableTimetableId, group)
        {
            stableTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            StableTimetableCells = stableTimetableCells;
        }

        public override bool CheckNoDuplicates()
        {
            StableTimetableCells.ThrowIfNull().IfHasNullElements().IfEmpty();

            foreach (var item in StableTimetableCells)
            {
                // Проверяем на дубликаты по времени занятия, четности и дню недели.
                int count = StableTimetableCells.Count(x => x.LessonTime == item.LessonTime && x.IsWeekEven == item.IsWeekEven && x.DayOfWeek == item.DayOfWeek);
                if (count > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
