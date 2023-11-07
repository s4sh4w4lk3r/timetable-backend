using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Repository;

namespace WebApi.Services.Timetables
{
    public class ActualTimetableService
    {
        private readonly SqlDbContext _dbContext;

        public ActualTimetableService(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> CreateAndSaveActualTimetable(int stableTimetableId, IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (stableTimetableId == default)
            {
                return ServiceResult.Fail("stableTimetableId не может быть равным нулю.");
            }

#warning здесь у ячеек только айдишники, по ним и будет создаваться актульное расписание, надо бы будет проверить потом.
            var stableTimetable = _dbContext.Set<StableTimetable>()
                .Include(e=>e.Group)
                .Include(e=>e.StableTimetableCells)
                .SingleOrDefault(e=>e.TimetableId == stableTimetableId);
            if (stableTimetable is null)
            {
                return ServiceResult.Fail("Стабильное расписание с таким id не найдено в бд.");
            }

            if (stableTimetable.CheckNoDuplicates() is false)
            {
                return ServiceResult.Fail("В стабильном расписании присутствуют дубликаты.");
            }

            try
            {
                var actualTimetable = new ActualTimetableFactory(stableTimetable).Create(0, datesOnly, idOnly: true);
                await _dbContext.Set<ActualTimetable>().AddAsync(actualTimetable, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return ServiceResult.Ok("Актуальное расписание создано и сохранено в БД.");
            }
            catch (ArgumentException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
        }
#warning написать методы сервиса для внесения замен.

        public async Task<ServiceResult<ActualTimetable?>> GetActualTimetable(int groupId, int weekNumber, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (groupId == default)
            {
                return ServiceResult<ActualTimetable?>.Fail("Введен groupId равный нулю.", null);
            }

            var actualTimetable = await _dbContext.Set<ActualTimetable>().Where(e => e.Group!.GroupId == groupId && e.WeekNumber == weekNumber)
                .Include(e => e.Group)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Teacher)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Subject)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.LessonTime)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Cabinet).FirstOrDefaultAsync(cancellationToken);
            if (actualTimetable is null)
            {
                return ServiceResult<ActualTimetable?>.Fail("Атуальное расписание не найдено", null);
            }

            return ServiceResult<ActualTimetable?>.Ok("Атуальное расписание не найдено", actualTimetable);
        }


    }
}
