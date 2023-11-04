using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells.CellMembers
{
    public class LessonTime
    {
        public int LessonTimeId { get; init; }
        public required int Number { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly EndsAt { get; init; }

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
    }
}
