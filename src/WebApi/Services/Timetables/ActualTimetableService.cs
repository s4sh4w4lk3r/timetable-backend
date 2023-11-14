using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Repository;

namespace WebApi.Services.Timetables
{
    public class ActualTimetableService
    {
        private readonly TimetableContext _dbContext;

        public ActualTimetableService(TimetableContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateActualTimetimetable(int stableTimetableId, IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default)
        {
            var stableTimeTable = await _dbContext.Set<StableTimetable>().Include(e => e.StableTimetableCells)
                .SingleOrDefaultAsync(e => e.TimetableId == stableTimetableId, cancellationToken);

            //var actualTimetable = new ActualTimetableFactory(stableTimeTable).Create(default, datesOnly, idOnly: true);
        }
    }
}
