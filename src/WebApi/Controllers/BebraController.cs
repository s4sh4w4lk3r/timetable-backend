using AscConverter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;

namespace WebApi.Controllers
{

    [ApiController, Route("/test")]
    public class BebraController : ControllerBase
    {
        [HttpGet, Route("")]
        public async Task<IActionResult> DoIt([FromServices] TimetableContext db)
        {
            //new AscConverter.Converter(@"C:\Users\sanchous\Desktop\projects\timetable-backend\данные\база.xml", db).Convert();

            var b = db.Set<StableTimetable>()
                .Include(e => e.StableTimetableCells).ThenInclude(e => e.Cabinet)
                .Include(e => e.StableTimetableCells).ThenInclude(e => e.Teacher)
                .Include(e => e.StableTimetableCells).ThenInclude(e => e.Subject)
                .Include(e => e.StableTimetableCells).ThenInclude(e => e.LessonTime)
                .Include(e => e.Group).Where(e => e.Group.Name == "4ИП-2-20").FirstOrDefault().StableTimetableCells.Where(e => e.DayOfWeek == DayOfWeek.Monday).ToList();
            return Ok();
        }
    }
}