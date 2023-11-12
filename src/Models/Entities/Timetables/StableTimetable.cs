using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public class StableTimetable : ITimetable
    {
        public int TimetableId { get; internal set; }
        public Group? Group { get; internal set; }
        public int GroupId { get; internal set; }
        public ICollection<StableTimetableCell>? StableTimetableCells { get; init; }

        private StableTimetable() { }

        public StableTimetable(int stableTimetableId, Group group, IEnumerable<StableTimetableCell> stableTimetableCells)
        {
            stableTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            group.ThrowIfNull();

            StableTimetableCells = stableTimetableCells.ToList();
            TimetableId = stableTimetableId;
            Group = group;
        }

        public bool CheckNoDuplicates()
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
