using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;
using WebApi.Extensions;
using WebApi.Services.Account.Implementations;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class RegistrationController : ControllerBase
{
    private readonly IValidator<User> _userValidator;
    private readonly ApprovalService _approvalService;
    private readonly RegisterService _registerService;

    public RegistrationController(IValidator<User> userValidator, ApprovalService approvalService, RegisterService registerService)
    {
        _userValidator = userValidator;
        _approvalService = approvalService;
        _registerService = registerService;
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody, Bind("Email", "Password")] User user, CancellationToken cancellationToken = default)
    {
        var userValidation = _userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var regResult = await _registerService.RegisterAsync(user, cancellationToken);
        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

        return Ok(regResult);
    }

    
    [HttpPost, Route("register/confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userEmail, [FromQuery] int approvalCode, CancellationToken cancellationToken = default)
    {
        var confirmResult = await _registerService.ConfirmEmailAsync(userEmail, approvalCode, _approvalService, cancellationToken);
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

        var sendApprovalResult = await _approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
        }

        return Ok("Письмо с кодом подтверждения отправлено на почту.");
    }

    
    [HttpGet, Route("unregister/send-email"), Authorize]
    public async Task<IActionResult> UnregisterSendMail([FromServices] DbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (HttpContext.User.TryGetIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось считать id из клеймов.");
        }

        string? userEmail = await dbContext.Set<User>().Where(e => e.UserId == userId).Select(e => e.Email).FirstOrDefaultAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return BadRequest("Пользователь не найден в бд.");
        }

        var sendCodeResult = await _approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken);
        if (sendCodeResult.Success is false)
        {
            return BadRequest(sendCodeResult);
        }

        return Ok(sendCodeResult);
    }

    
    [HttpPost, Route("unregister/confirm"), Authorize]
    public async Task<IActionResult> ConfirmUnregistration(int approvalCode, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось считать id из клеймов.");
        }

        if (userId == default)
        {
            return BadRequest("Id пользователя не может быть равным нулю.");
        }

        var unregisterResult = await _registerService.UnregisterAsync(userId, approvalCode, _approvalService, cancellationToken);
        if (unregisterResult.Success is false)
        {
            return BadRequest(unregisterResult);
        }

        return Ok(unregisterResult);
    }
}