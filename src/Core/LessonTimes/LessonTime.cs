namespace Core.LessonTimes;

public class LessonTime
{
    public int LessonNumber { get; init; }
    public DayOfWeek DayOfWeek { get; init; }

    /// <summary>
    /// True если неделя четная, в противном случае false
    /// </summary>
    public bool IsWeekEven { get; init; }
    public TimeOnly From { get; init; }
    public TimeOnly To { get; init; }
    public LessonTime(int lessonNumber, TimeOnly from, TimeOnly to)
    {
        LessonNumber = lessonNumber;
        From = from;
        To = to;
    }
}