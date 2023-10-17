using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Implementations.Timetables;

namespace WebApi.Controllers;

[ApiController, Route("/api/cabinet")]
public class CabinetController : ControllerBase
{
    private readonly CabinetService _service;
    public CabinetController([FromServices] CabinetService service)
    {
        _service = service;
    }

    [Route("getlist")]
    [HttpGet]
    [Authorize]
    public IActionResult GetCabinetList()
    {
        return Ok(_service.Cabinets.ToList());
    }
}
