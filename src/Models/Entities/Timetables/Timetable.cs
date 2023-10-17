using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables;

public class Timetable
{
    public int TimetableId { get; set; }
    public Group? Group { get; init; }

    /// <summary>
    /// Полный список всех занятий, в том числе с заменами. Замены идут отдельной ячейкой и имеют ссылку на занятие, которое под замену.
    /// На клиенте надо делать выборку для формирования общего расписания.
    /// </summary>
    public IList<TimetableCell>? TimetableCells { get; private set; }

#warning кажется бизнес-логику не продумал.
    private Timetable() { }
    public Timetable(int timeTablePk, Group group, IEnumerable<TimetableCell> timetableCells)
    {
        group.ThrowIfNull();
        timetableCells.Throw().IfEmpty();
        TimetableId = timeTablePk;
        Group = group;
        TimetableCells = timetableCells.ToList();
    }

    /// <summary>
    /// Проверяет, есть ли дубликаты занятий на одно и то же время.
    /// </summary>
    /// <returns> True если нет дубликатов, в противном случае false.</returns>
    public bool IsTimeLessonsOk()
    {
        if (TimetableCells is null) {  return false; }

        var lessonTimes = TimetableCells.DistinctBy(c => c.LessonTime).Select(c => c.LessonTime);

        foreach (var lessonTime in lessonTimes)
        {
            int matches = 0;

            foreach (var timeTableCell in TimetableCells)
            {

                if (timeTableCell.LessonTime == lessonTime)
                {
                    matches++;
                }

                if (matches > 1 && timeTableCell.IsReplaced is false)
                {
                    return false;
                }
            }
        }
        return true;
    }
}