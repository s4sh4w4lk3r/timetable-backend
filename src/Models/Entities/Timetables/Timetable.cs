using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables;

public class Timetable
{
    public Group Group { get; init; }

    /// <summary>
    /// Полный список всех занятий, в том числе с заменами. Замены идут отдельной ячейкой и имеют ссылку на занятие, которое под замену.
    /// На клиенте надо делать выборку для формирования общего расписания.
    /// </summary>
    public IList<TimetableCell> TimetableCells { get; private set; }

    public Timetable(Group group, IEnumerable<TimetableCell> timetableCells)
    {
        group.ThrowIfNull();
        timetableCells.Throw().IfEmpty();
        Group = group;
        TimetableCells = timetableCells.ToList();
        EnsureTimeLessonsOk();
    }

    /// <summary>
    /// Проверяет, есть ли дубликаты занятий на одно и то же время.
    /// </summary>
    /// <returns> True если нет дубликатов, в противном случае false.</returns>
    private bool EnsureTimeLessonsOk()
    {
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
                    throw new ArgumentException($"В коллекции несколько занятий на одно и то же время. {timeTableCell.LessonTime}");
                }

            }
        }

        return true;
    }
}