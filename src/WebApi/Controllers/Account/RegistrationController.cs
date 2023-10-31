using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using Models.Validation;
using WebApi.Extensions;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class RegistrationController : ControllerBase
{
    private readonly IValidator<User> _userValidator;
    private readonly IRegistrationService _registerService;
    private readonly IUnregistrationService _unregistrationService;

    public RegistrationController(IValidator<User> userValidator, IRegistrationService registerService, IUnregistrationService unregistrationService)
    {
        _userValidator = userValidator;
        _registerService = registerService;
        _unregistrationService = unregistrationService;
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody, Bind("Email", "Password")] User user, CancellationToken cancellationToken = default)
    {
        var userValidation = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var regResult = await _registerService.AddUserToRepoAsync(user, cancellationToken);
        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

        return Ok(regResult);
    }


    [HttpPost, Route("register/confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userEmail, [FromQuery] int approvalCode, CancellationToken cancellationToken = default)
    {
        var confirmResult = await _registerService.ConfirmAsync(userEmail, approvalCode, cancellationToken);
        if (confirmResult.Success is false)
        {
            return BadRequest(confirmResult);
        }
        return Ok(confirmResult);
    }


    [HttpPost, Route("register/send-email")]
    public async Task<IActionResult> SendRegisterEmail([FromQuery] string userEmail, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(userEmail) is false)
        {
            return BadRequest("Email адрес имеет неверный формат.");
        }

        var sendApprovalResult = await _registerService.SendEmailAsync(userEmail, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
        }

        return Ok("Письмо с кодом подтверждения отправлено на почту.");
    }


    [HttpGet, Route("unregister/send-email"), Authorize]
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


    [HttpPost, Route("unregister/confirm"), Authorize]
    public async Task<IActionResult> ConfirmUnregistration(int approvalCode, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось считать id из клеймов.");
        }

        if (userId == default)
        {
            return BadRequest("Id пользователя не может быть равным нулю.");
        }

        var unregisterResult = await _unregistrationService.ConfirmAsync(userId, approvalCode, cancellationToken);
        if (unregisterResult.Success is false)
        {
            return BadRequest(unregisterResult);
        }

        return Ok(unregisterResult);
    }
}