using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using Models.Entities.Users.Auth;
using Services.Implementations;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IValidator<User> _userValidator;
    private readonly UserService _userService;

    public AuthController(IValidator<User> userValidator, UserService userService)
    {
        _userValidator = userValidator;
        _userService = userService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody, Bind("Email", "Password")] User user, 
        [FromServices] ITokenService tokenService, [FromServices] UserSessionService userSessionService, 
        CancellationToken cancellationToken = default)
    {
        var userValidation = _userValidator.Validate(user, o => o.IncludeRuleSets("default","password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var checkLoginDataResult = await _userService.CheckLoginDataAsync(user, cancellationToken);
        if (checkLoginDataResult.Success is false || checkLoginDataResult.Value is null)
        {
            return BadRequest(new ServiceResult(false, "Неудачная попытка входа.", checkLoginDataResult));
        }
        user = checkLoginDataResult.Value;

        var claims = new List<Claim> 
        { 
            new Claim(user.UserId.ToString(), ClaimTypes.NameIdentifier),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        string accessToken = tokenService.GenerateAccessToken(claims);
        string refreshToken = tokenService.GenerateRefreshToken();

        var userSession = new UserSession()
        {
            RefreshToken = refreshToken,
            DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString(),
            UserId = user.UserId
        };

        var userSessionResult = await userSessionService.AddUserSessionAsync(userSession, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(new ServiceResult(false, "Сессия не добавлена в бд.", userSessionResult));
        }

        return Ok(new AuthenticatedResponse(accessToken, refreshToken));
#warning не проверен.
    }


    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody, Bind("Email", "Password")] User user, 
        CancellationToken cancellationToken = default)
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

    [HttpPost, Route("confirmemail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userEmail,
        [FromQuery] int approvalCode = default, CancellationToken cancellationToken = default)
    {
        var confirmResult = await _userService.ConfirmEmailAsync(userEmail, approvalCode, cancellationToken);
        if (confirmResult.Success is false)
        {
            return BadRequest(confirmResult);
        }
        return Ok(confirmResult);
    }


    [HttpPost, Route("sendemail")]
    public async Task<IActionResult> SendRegisterEmail([FromQuery] string userEmail, [FromServices] ApprovalService approvalService, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userEmail) is true)
        {
            return BadRequest(new ServiceResult(false, "Почта не введена."));
        }

        var sendApprovalResult = await approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
        }

        return Ok("Письмо с кодом подтверждения отправлено на почту.");
    }


    public record class AuthenticatedResponse(string? AccessToken, string? RefreshToken);
}