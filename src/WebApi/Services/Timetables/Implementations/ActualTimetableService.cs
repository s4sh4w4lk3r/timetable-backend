using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Repository;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Services.Timetables.Implementations
{
    public class ActualTimetableService : IActualTimetableService
    {
        private readonly TimetableContext _dbContext;

        public ActualTimetableService(TimetableContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> CreateActualTimetableForAll(IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default)
        {
            var stableTimeTables = await _dbContext.Set<StableTimetable>()
                .Include(e => e.StableTimetableCells)
                .Include(e => e.Group).ToListAsync(cancellationToken);

            foreach (var item in stableTimeTables)
            {
                var actualTimetable = new ActualTimetableFactory(item).Create(default, datesOnly);
                _dbContext.Set<ActualTimetable>().Add(actualTimetable);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Ok("Расписание для всех существующих групп с константным расписанием будет создано на указанные дни.");
        }

        public async Task<ServiceResult> CreateOnlyOneActualTimetable(int stableTimetableId, IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default)
        {

            var stableTimeTable = await _dbContext.Set<StableTimetable>()
                .Include(e => e.StableTimetableCells)
                .Include(e => e.Group)
                .SingleOrDefaultAsync(e => e.TimetableId == stableTimetableId, cancellationToken);

            if (stableTimeTable is null)
            {
                return ServiceResult.Fail("Расписание с таким stableTimetableId не найдено.");
            }

            var actualTimetable = new ActualTimetableFactory(stableTimeTable).Create(default, datesOnly);
            _dbContext.Set<ActualTimetable>().Add(actualTimetable);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Ok("Расписание будет добавлено на указанные дни.");
            
        }
    }
}
