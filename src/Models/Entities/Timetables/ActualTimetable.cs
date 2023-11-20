using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    /// <summary>
    /// Класс расписания, который должен заполняться из константного расписания фабрикой ActualTimetableFactory. 
    /// Этот тип используется для показа текущего расписания на неделю и для отправки на фронтенд.
    /// </summary>
    public class ActualTimetable : ITimetable
    {
        public int TimetableId { get; internal set; }
        public Group? Group { get; internal set; }
        public int GroupId { get; internal set; }
        public int WeekNumber { get; internal set; }
        public ICollection<ActualTimetableCell>? ActualTimetableCells { get; init; }

        private ActualTimetable() { }

        public ActualTimetable(int actualTimetableId, Group group, IEnumerable<ActualTimetableCell> actualTimetableCells, int weekNumber)
        {
            actualTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            weekNumber.Throw().IfLessThan(1).IfGreaterThan(53);
            group.ThrowIfNull();

            Group = group;
            TimetableId = actualTimetableId;
            ActualTimetableCells = actualTimetableCells.ToList();
            WeekNumber = weekNumber;
        }

        public bool CheckNoDuplicates()
        {
#warning не забыть эту проверку в сервисах поставить!
            ActualTimetableCells.ThrowIfNull().IfHasNullElements().IfEmpty();

            foreach (var item in ActualTimetableCells)
            {
                // Проверяем на дубликаты по времени занятия и по дате.
                int count = ActualTimetableCells.Count(x => x.LessonTime == item.LessonTime && x.Date == item.Date && x.SubGroup == item.SubGroup);
                if (count > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
