using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells.CellMembers
{
    public class LessonTime : IEquatable<LessonTime?>
    {
        public int LessonTimeId { get; init; }
        public required int Number { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly EndsAt { get; init; }


        [JsonIgnore]
        public ICollection<ActualTimetableCell>? ActualTimetableCells { get; set; }

        [JsonIgnore]
        public ICollection<StableTimetableCell>? StableTimetableCells { get; set; }

        private LessonTime() { }

        [SetsRequiredMembers]
        public LessonTime(int lessonTimePk, int number, TimeOnly startsAt, TimeOnly endsAt)
        {
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
