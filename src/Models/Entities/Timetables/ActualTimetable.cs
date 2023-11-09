using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    /// <summary>
    /// Класс расписания, который должен заполняться из константного расписания фабрикой ActualTimetableFactory. 
    /// Этот тип используется для показа текущего расписания на неделю и для отправки на фронтенд.
    /// </summary>
    public class ActualTimetable : ITimetable
    {
        public required int WeekNumber { get; init; }
        public required IEnumerable<ActualTimetableCell>? ActualTimetableCells { get; init; }

        private ActualTimetable() : base() { }

        [SetsRequiredMembers]
        public ActualTimetable(int actualTimetableId, Group group, IEnumerable<ActualTimetableCell> actualTimetableCells, int weekNumber) : base(actualTimetableId, group)
        {
            actualTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            weekNumber.Throw().IfLessThan(1).IfGreaterThan(53);
            ActualTimetableCells = actualTimetableCells;
            WeekNumber = weekNumber;
#warning вернуть проверку если надо.
            //EnsureNoDuplicates();
        }

        public override bool CheckNoDuplicates()
        {
            ActualTimetableCells.ThrowIfNull().IfHasNullElements().IfEmpty();

            foreach (var item in ActualTimetableCells)
            {
                // Проверяем на дубликаты по времени занятия и по дате.
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
