using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells;

public class LessonTime
{
    public int LessonTimeId { get; init; }
    public int LessonNumber { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    /// True если неделя четная, в противном случае false
    /// </summary>
    public bool IsWeekEven { get; init; }
    public TimeOnly From { get; init; }
    public TimeOnly To { get; init; }

    [JsonIgnore]
    public List<TimetableCell>? TimetableCells { get; set; }

    private LessonTime() { }
    public LessonTime(int lessonTimePk, int lessonNumber, bool isWeekEven, DayOfWeek dayOfWeek, TimeOnly from, TimeOnly to)
    {
        LessonTimeId = lessonTimePk;
        LessonNumber = lessonNumber;
        IsWeekEven = isWeekEven;
        From = from;
        To = to;
        DayOfWeek = dayOfWeek;
    }

    public override bool Equals(object? obj)
    {
        return obj is LessonTime time &&
               LessonNumber == time.LessonNumber &&
               DayOfWeek == time.DayOfWeek &&
               IsWeekEven == time.IsWeekEven &&
               From.Equals(time.From) &&
               To.Equals(time.To) &&
               LessonTimeId.Equals(time.LessonTimeId);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(LessonNumber, DayOfWeek, IsWeekEven, From, To);
    }
    public static bool operator ==(LessonTime? left, LessonTime? right)
    {
        return EqualityComparer<LessonTime>.Default.Equals(left, right);
    }
    public static bool operator !=(LessonTime? left, LessonTime? right)
    {
        return !(left == right);
    }
    public override string ToString()
    {
        return $"LessonTimePK: {LessonTimeId}, LessonNumber: {LessonNumber}, DayOfWeek: {DayOfWeek}, IsWeekEven: {IsWeekEven}, From: {From}, To: {To}.";
    }
}