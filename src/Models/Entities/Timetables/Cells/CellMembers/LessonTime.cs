namespace Models.Entities.Timetables.Cells.CellMembers
{
    public class LessonTime
    {
        public required int Number { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly EndsAt { get; init; }

        private LessonTime() { }
        public LessonTime(int number, TimeOnly startsAt, TimeOnly endsAt)
        {
            Number = number;
            StartsAt = startsAt;
            EndsAt = endsAt;
        }
    }
}
