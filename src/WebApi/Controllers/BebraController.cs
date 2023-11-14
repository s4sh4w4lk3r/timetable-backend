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
#pragma warning disable 1998
        public async Task<IActionResult> DoIt()
#pragma warning restore 1998
        {
            return Ok();
        }
    }
}