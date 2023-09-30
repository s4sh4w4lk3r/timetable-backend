using Core.Timetables.Cells;

namespace Core.Timetables;

public class Timetable
{
    public Group Group { get; init; }

    /// <summary>
    /// Список занятий на четной недели
    /// </summary>
    public IList<TimetableCell> TimetableCellsOnEvenWeek { get; private set; }

    /// <summary>
    /// Список занятий на нечетной недели
    /// </summary>
    public IList<TimetableCell> TimetableCellsOnOddWeek { get; private set; }

    public Timetable(Group group, IEnumerable<TimetableCell> timetableCellsOnEvenWeek, IEnumerable<TimetableCell> timetableCellsOnOddWeek)
    {
        group.ThrowIfNull();
        timetableCellsOnEvenWeek.Throw().IfEmpty();
        timetableCellsOnOddWeek.Throw().IfEmpty();
        Group = group;
        TimetableCellsOnEvenWeek = timetableCellsOnEvenWeek.ToList();
        TimetableCellsOnOddWeek = timetableCellsOnOddWeek.ToList();
    }
}