using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Repository;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("/api/timetable")]
    public class TimetableController : ControllerBase
    {
        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> GetCabinetList([FromQuery] int groupId, [FromServices] SqlDbContext dbContext)
        {
#warning сделать ендпоинт на получение списка групп с айдишниками
#warning сделать чтобы апи возвр расписание с заменами, замены пока в бд не сделал
            var tt = await dbContext.Set<Timetable>()
                .Include(e => e.TimetableCells)!.ThenInclude(e => e.LessonTime)
                .Include(e => e.TimetableCells)!.ThenInclude(e => e.Subject)
                .Include(e => e.TimetableCells)!.ThenInclude(e => e.Teacher)
                .Include(e => e.TimetableCells)!.ThenInclude(e => e.Cabinet)
                .Include(e=>e.Group)
                .FirstAsync();
            return Ok(tt);
        }
    }
}
