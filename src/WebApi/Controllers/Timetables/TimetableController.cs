using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("/api/timetable")]
    public class TimetableController : ControllerBase
    {
        [Route("get")]
        [HttpGet]
        public Task<IActionResult> GetCabinetList([FromQuery] int groupId)
        {
            throw new NotImplementedException();
#warning сделать тут
        }
    }
}
