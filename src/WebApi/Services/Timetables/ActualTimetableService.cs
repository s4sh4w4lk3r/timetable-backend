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

        public ActualTimetable InsertActualTimetable()
        {
            /*var datesOnly = new List<DateOnly>()
            {
                DateOnly.Parse("06.11.2023"),
                DateOnly.Parse("07.11.2023"),
                DateOnly.Parse("08.11.2023"),
                DateOnly.Parse("09.11.2023"),
                DateOnly.Parse("10.11.2023"),
            };

            var a = _dbContext.Set<StableTimetable>()
                .Include(e => e.Group)
                .Include(e => e.StableTimetableCells)!.ThenInclude(e => e.Teacher)
                .Include(e => e.StableTimetableCells)!.ThenInclude(e => e.Subject)
                .Include(e => e.StableTimetableCells)!.ThenInclude(e => e.LessonTime)
                .Include(e => e.StableTimetableCells)!.ThenInclude(e => e.Cabinet).Where(e => e.Group!.Name == "4ИП-2-20").First();


            var b = new ActualTimetableFactory(a).Create(0, datesOnly);

            _dbContext.Set<ActualTimetable>().Add(b);
            _dbContext.SaveChanges();*/

            var a = _dbContext.Set<ActualTimetable>().Where(e => e.Group.Name == "4ИП-2-20")
                .Include(e => e.Group)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Teacher)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Subject)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.LessonTime)
                .Include(e => e.ActualTimetableCells)!.ThenInclude(e => e.Cabinet).Where(e => e.Group!.Name == "4ИП-2-20").First();
            return a;
#warning написать методы сервиса для создания актульного расписания, получения групп, внесения замен.
        }
    }
}
