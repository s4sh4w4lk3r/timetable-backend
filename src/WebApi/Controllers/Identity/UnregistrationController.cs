using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Controllers.Account
{
    [ApiController, Route("api/account/unregister")]
    public class UnregistrationController : ControllerBase
    {
        private readonly IUnregistrationService _unregistrationService;

        public UnregistrationController(IUnregistrationService unregistrationService)
        {
            _unregistrationService = unregistrationService;
        }


        [HttpGet, Route("send-email"), Authorize]
        public async Task<IActionResult> UnregisterSendMail(CancellationToken cancellationToken = default)
        {
            if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
            {
                return BadRequest("Не получилось считать id из клеймов.");
            }

            var requestUnreg = await _unregistrationService.SendEmailAsync(userId, cancellationToken);
            if (requestUnreg.Success is false)
            {
                return BadRequest(requestUnreg);
            }

            return Ok(requestUnreg);
        }


        [HttpPost, Route("confirm"), Authorize]
        public async Task<IActionResult> ConfirmUnregistration([FromBody] ApprovalDto approvalDto, CancellationToken cancellationToken)
        {
            if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
            {
                return BadRequest("Не получилось считать id из клеймов.");
            }

            if (userId == default)
            {
                return BadRequest("Id пользователя не может быть равным нулю.");
            }

            var unregisterResult = await _unregistrationService.ConfirmAsync(userId, approvalDto.ApprovalCode, cancellationToken);
            if (unregisterResult.Success is false)
            {
                return BadRequest(unregisterResult);
            }

            return Ok(unregisterResult);
        }
        public record ApprovalDto(int ApprovalCode);
    }
}
