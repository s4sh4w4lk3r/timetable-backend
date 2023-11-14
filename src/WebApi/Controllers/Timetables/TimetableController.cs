using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("timetable")]
    public class TimetableController : ControllerBase
    {
        private readonly IStableTimetableService _stableTimetableService;

        public TimetableController(IStableTimetableService stableTimetableService)
        {
            _stableTimetableService = stableTimetableService;
        }

        [HttpPost, Route("import-xml"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportAscBase()
        {
            var result = await _stableTimetableService.ReadAndSaveAscXmlToRepoAsync(HttpContext.Request.Body);
            if (result.Success is false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
