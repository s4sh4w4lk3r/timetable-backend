using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using WebApi.Services;
using WebApi.Services.Account.Implementations;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class RegistrationController : ControllerBase
{
    private readonly IValidator<User> _userValidator;
    private readonly UserService _userService;
    private readonly ApprovalService _approvalService;

    public RegistrationController(IValidator<User> userValidator, UserService userService, ApprovalService approvalService)
    {
        _userValidator = userValidator;
        _userService = userService;
        _approvalService = approvalService;
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody, Bind("Email", "Password")] User user, CancellationToken cancellationToken = default)
    {
        var userValidation = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var regResult = await _userService.RegisterAsync(user, cancellationToken);
        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

        return Ok(regResult);
    }

    [HttpPost, Route("register/confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userEmail, [FromQuery] int approvalCode, CancellationToken cancellationToken = default)
    {
        var confirmResult = await _userService.ConfirmEmailAsync(userEmail, approvalCode, _approvalService, cancellationToken);
        if (confirmResult.Success is false)
        {
            return BadRequest(confirmResult);
        }
        return Ok(confirmResult);
    }

    [HttpPost, Route("register/send-email")]
    public async Task<IActionResult> SendRegisterEmail([FromQuery] string userEmail, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userEmail) is true)
        {
            return BadRequest(new ServiceResult(false, "Почта не введена."));
        }


        var sendApprovalResult = await _approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
        }

        return Ok("Письмо с кодом подтверждения отправлено на почту.");
    }
}