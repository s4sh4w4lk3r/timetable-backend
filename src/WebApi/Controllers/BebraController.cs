#pragma warning disable 1998
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController, Route("/test")]
    public class BebraController : ControllerBase
    {
        [HttpGet, Route("")]
        public async Task<IActionResult> DoIt()
        {
            return Ok();
        }
    }
}
#pragma warning restore 1998