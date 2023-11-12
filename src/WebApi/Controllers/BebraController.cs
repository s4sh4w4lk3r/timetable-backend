using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebApi.Controllers
{
    
    [ApiController, Route("/test")]
    public class BebraController : ControllerBase
    {
        [HttpGet, Route(""), Authorize()]
        public async Task<IActionResult> DoIt()
        {
            var sb = new StringBuilder();
            foreach (var item in User.Claims)
            {
                sb.Append(item.Type).Append(" - ").Append(item.Value).Append('\n');
            }
            return Ok(sb.ToString());
        }
        /*private static List<StableTimetableCell> GetListCells()
        {
            var mdk1101 = new Subject(0, "МДК 11.01 Технология разработки и защиты баз данных");
            var mdk0701 = new Subject(0, "МДК 07.01 Управление и автоматизация баз данных");
            var mdk1102 = new Subject(0, "МДК 11.02 Программные решения для бизнеса");
            var mdk0201 = new Subject(0, "МДК 02.01 Технология разработки программного обеспечения");
            var mdk0702 = new Subject(0, "МДК.07.02 Сертификация информационных систем");
            var angl = new Subject(0, "Иностранный язык в профессиональной деятельности");
            var cert = new Subject(0, "Стандартизация, сертификация и техническое докуметоведение");
            var fizra = new Subject(0, "Физическая культура");
            var pravo = new Subject(0, "Правовое обеспечение профессиональной деятельности");

            var cab213 = new Cabinet(0, "Миллионщикова", "213");
            var cab215 = new Cabinet(0, "Миллионщикова", "215");
            var cab210 = new Cabinet(0, "Миллионщикова", "210");
            var cab324 = new Cabinet(0, "Миллионщикова", "324");
            var cab308 = new Cabinet(0, "Миллионщикова", "308");
            var cab102 = new Cabinet(0, "Миллионщикова", "102");
            var cab301 = new Cabinet(0, "Миллионщикова", "301");
            var cabtren = new Cabinet(0, "Миллионщикова", "Трен");

            var artsybasheva = new Teacher(0, "Арцыбашева", "d", "d");
            var petrenko = new Teacher(0, "Петренко", "das", "da");
            var ahmerova = new Teacher(0, "Ахмерова", "d", "d");
            var prohor = new Teacher(0, "Прохоренкова", "d", "d");
            var barsuk = new Teacher(0, "Барускова", "d", "d");
            var chern = new Teacher(0, "Чернова", "d", "d");
            var ippo = new Teacher(0, "Ипполитова", "d", "d");
            var tanchenko = new Teacher(0, "Танченко", "d", "d");

            var para1 = new LessonTime(0, 1, TimeOnly.Parse("9:00"), TimeOnly.Parse("10:30"));
            var para2 = new LessonTime(0, 2, TimeOnly.Parse("10:50"), TimeOnly.Parse("12:20"));
            var para3 = new LessonTime(0, 3, TimeOnly.Parse("12:40"), TimeOnly.Parse("14:10"));
            var para4 = new LessonTime(0, 4, TimeOnly.Parse("14:30"), TimeOnly.Parse("16:00"));
            var para5 = new LessonTime(0, 5, TimeOnly.Parse("16:10"), TimeOnly.Parse("17:40"));

            var evenList = new List<StableTimetableCell>()
            {
                new StableTimetableCell(0, prohor, mdk1101, cab213, para2, true, DayOfWeek.Monday),
                new StableTimetableCell(0, prohor, mdk0701, cab213, para3, true, DayOfWeek.Monday),
                new StableTimetableCell(0, ahmerova, mdk1102, cab215, para4, true, DayOfWeek.Monday),
                new StableTimetableCell(0, artsybasheva, mdk0201, cab210, para5, true, DayOfWeek.Monday),

                new StableTimetableCell(0, artsybasheva, mdk0201, cab210, para3, true, DayOfWeek.Tuesday),
                new StableTimetableCell(0, petrenko, mdk0702, cab324, para4, true, DayOfWeek.Tuesday),
                new StableTimetableCell(0, tanchenko, angl, cab301, para5, true, DayOfWeek.Tuesday),

                new StableTimetableCell(0, petrenko, mdk0702, cab324, para2, true, DayOfWeek.Wednesday),
                new StableTimetableCell(0, chern, fizra, cabtren, para3, true, DayOfWeek.Wednesday),
                new StableTimetableCell(0, prohor, mdk1101, cab213, para4, true, DayOfWeek.Wednesday),
                new StableTimetableCell(0, barsuk, pravo, cab308, para5, true, DayOfWeek.Wednesday),

                new StableTimetableCell(0, ippo, mdk1101, cab213, para1, true, DayOfWeek.Thursday),
                new StableTimetableCell(0, prohor, mdk1101, cab213, para2, true, DayOfWeek.Thursday),
                new StableTimetableCell(0, prohor, mdk0701, cab213, para3, true, DayOfWeek.Thursday),
                new StableTimetableCell(0, ippo, cert, cab102, para4, true, DayOfWeek.Thursday),

                new StableTimetableCell(0, ippo, cert, cab102, para2, true, DayOfWeek.Friday),
                new StableTimetableCell(0, petrenko, mdk0702, cab324, para3, true, DayOfWeek.Friday),
                new StableTimetableCell(0, artsybasheva, mdk0201, cab210, para4, true, DayOfWeek.Friday),
                new StableTimetableCell(0, barsuk, pravo, cab213, para5, true, DayOfWeek.Friday)
            };

            var oddList = new List<StableTimetableCell>()
            {
                new StableTimetableCell(0, prohor, mdk1101, cab213, para2, false, DayOfWeek.Monday),
                new StableTimetableCell(0, prohor, mdk0701, cab213, para3, false, DayOfWeek.Monday),
                new StableTimetableCell(0, ahmerova, mdk0201, cab215, para4, false, DayOfWeek.Monday),

                new StableTimetableCell(0, artsybasheva, mdk1101, cab210, para3, false, DayOfWeek.Tuesday),
                new StableTimetableCell(0, petrenko, mdk0702, cab324, para4, false, DayOfWeek.Tuesday),
                new StableTimetableCell(0, tanchenko, angl, cab308, para5, false, DayOfWeek.Tuesday),

                new StableTimetableCell(0, petrenko, mdk0702, cab324, para2, false, DayOfWeek.Wednesday),
                new StableTimetableCell(0, chern, fizra, cabtren, para3, false, DayOfWeek.Wednesday),
                new StableTimetableCell(0, prohor, mdk1101, cab213, para4, false, DayOfWeek.Wednesday),
                new StableTimetableCell(0, barsuk, pravo, cab213, para5, false, DayOfWeek.Wednesday),

                new StableTimetableCell(0, ippo, mdk1101, cab102, para1, false, DayOfWeek.Thursday),
                new StableTimetableCell(0, prohor, mdk1101, cab213, para2, false, DayOfWeek.Thursday),
                new StableTimetableCell(0, prohor, mdk1101, cab213, para3, false, DayOfWeek.Thursday),
                new StableTimetableCell(0, ippo, mdk1101, cab102, para4, false, DayOfWeek.Thursday),

                new StableTimetableCell(0, petrenko, mdk0702, cab324, para3, false, DayOfWeek.Friday),
                new StableTimetableCell(0, artsybasheva, mdk1101, cab210, para4, false, DayOfWeek.Friday),
                new StableTimetableCell(0, barsuk, pravo, cab213, para5, false, DayOfWeek.Friday)
            };

            oddList.AddRange(evenList);
            return oddList;
        }*/
    }
}
