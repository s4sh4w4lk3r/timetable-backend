using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells.CellMembers
{
    public class LessonTime : IEquatable<LessonTime?>
    {
        public int LessonTimeId { get; init; }
        public required int Number { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly EndsAt { get; init; }

        public List<ActualTimetableCell>? ActualTimetableCells { get; set; }
        public List<StableTimetableCell>? StableTimetableCells { get; set; }

        private LessonTime() { }

        [SetsRequiredMembers]
        public LessonTime(int lessonTimePk, int number, TimeOnly startsAt, TimeOnly endsAt)
        {
#warning добавить проверку
            LessonTimeId = lessonTimePk;
            Number = number;
            StartsAt = startsAt;
            EndsAt = endsAt;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LessonTime);
        }

        public bool Equals(LessonTime? other)
        {
            return other is not null &&
                   LessonTimeId == other.LessonTimeId &&
                   Number == other.Number &&
                   StartsAt.Equals(other.StartsAt) &&
                   EndsAt.Equals(other.EndsAt);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LessonTimeId, Number, StartsAt, EndsAt);
        }

        public static bool operator ==(LessonTime? left, LessonTime? right)
        {
            return EqualityComparer<LessonTime>.Default.Equals(left, right);
        }

        public static bool operator !=(LessonTime? left, LessonTime? right)
        {
            return !(left == right);
        }
    }
}
