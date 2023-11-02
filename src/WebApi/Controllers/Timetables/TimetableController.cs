using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("/api/timetable")]
    public class TimetableController : ControllerBase
    {
        [Route("get")]
        [HttpGet]
        public IActionResult GetCabinetList(/*[FromQuery] int groupId*/)
        {
            return Ok(new TestDataSet().GetTimetable());
        }
    }
}
