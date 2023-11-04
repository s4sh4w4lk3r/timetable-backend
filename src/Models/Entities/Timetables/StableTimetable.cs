using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public class StableTimetable : Timetable
    {
        public required IEnumerable<StableTimetableCell>? StableTimetableCells { get; init; }

        private StableTimetable() : base() { } 

        [SetsRequiredMembers]
        public StableTimetable(int stableTimetableId, Group group, IEnumerable<StableTimetableCell> stableTimetableCells) : base(stableTimetableId, group)
        {
            stableTimetableCells.ThrowIfNull().IfEmpty().IfHasNullElements();
            StableTimetableCells = stableTimetableCells;
        }
    }
}
