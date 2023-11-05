using Models.Entities.Timetables.Cells;
using Models.Extenstions;
using System.Globalization;

namespace Models.Entities.Timetables
{
    public class ActualTimetableFactory
    {
        private readonly List<StableTimetableCell> _stableTimetableCells;
        private readonly List<ActualTimetableCell> _actualTimetableCells = new();
        private readonly Group _group;

        public ActualTimetableFactory(StableTimetable stableTimetable)
        {
            stableTimetable.Group.ThrowIfNull().IfDefault(e => e.GroupId);
            stableTimetable.StableTimetableCells.ThrowIfNull().IfHasNullElements().IfEmpty();

            _group = stableTimetable.Group;
            _stableTimetableCells = stableTimetable.StableTimetableCells.ToList();
        }

        /// <summary>
        /// Создает актульное расписание, принимая массив с датами, на которое надо наложить расписание из константного расписания.
        /// Вызывает исключение, если будет передан массив дат, которые указывают на разные недели.
        /// </summary>
        /// <param name="datesOnly">Массив с датами, дубликаты дат будут удалены из коллекции.</param>
        /// <returns>Возвращает актульное расписание.</returns>
        /// <exception cref="ArgumentException"></exception>
        public ActualTimetable Create(int newTimetableId, IEnumerable<DateOnly> datesOnly)
        {
            datesOnly.ThrowIfNull().IfEmpty();

            // Пропускаем одинаковые даты.
            datesOnly = datesOnly.Distinct();

            int weekNumber = ISOWeek.GetWeekOfYear(datesOnly.First().ToDateTime(TimeOnly.MinValue));
            foreach (var date in datesOnly)
            {
                {
                    int currentWeekNumber = ISOWeek.GetWeekOfYear(date.ToDateTime(TimeOnly.MinValue));
                    if (currentWeekNumber != weekNumber)
                    {
                        throw new ArgumentException("Передан массив дат, которые указывают на разные недели.");
                    }
                }
                AddCellsForDate(date);
            }

            return new ActualTimetable(newTimetableId, _group, _actualTimetableCells, weekNumber);
        }

        /// <summary>
        /// Добавляет ячейки из константного расписания в актульное.
        /// </summary>
        /// <param name="dateOnly"></param>
        private void AddCellsForDate(DateOnly dateOnly)
        {
            DayOfWeek dayOfWeek = dateOnly.DayOfWeek;
            bool isEven = dateOnly.IsWeekEven();

            var stableListForThisDay = _stableTimetableCells.Where(e => e.DayOfWeek == dayOfWeek && e.IsWeekEven == isEven).ToList();

            foreach (var item in stableListForThisDay)
            {
                var actualCell = item.CastToActualCell(dateOnly);
                _actualTimetableCells.Add(actualCell);
            }
        }

#warning надо как следует продебажить все по всем датам.
    }
}
