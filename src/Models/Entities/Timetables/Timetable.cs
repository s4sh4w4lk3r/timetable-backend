using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables
{
    public abstract class Timetable
    {
        public required int TimetableId { get; init; }
        public required Group? Group { get; init; }

        protected Timetable() { }

        [SetsRequiredMembers]
        protected Timetable(int timetableId, Group group)
        {
            timetableId.Throw().IfDefault();
            group.ThrowIfNull();

            TimetableId = timetableId;
            Group = group;
        }
    }
}
