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
    }
}