using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables
{
    public class StableTimetable : Timetable
    {
        public required IEnumerable<StableTimetableCell> StableTimetableCells { get; init; }

        private StableTimetable() : base() { } 

        [SetsRequiredMembers]
        public StableTimetable(Group group, IEnumerable<StableTimetableCell> stableTimetableCells) : base(group, stableTimetableCells)
        {
            StableTimetableCells = stableTimetableCells;
        }
    }
}
