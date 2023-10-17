using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using Services.Implementations;
using System.Security.Claims;
using WebApi.Services.Implementations;

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
        [FromQuery] string? returnUrl, CancellationToken cancellationToken = default)
    {
        var userValidation = _userValidator.Validate(user, o => o.IncludeRuleSets("default","password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        if ((await _userService.CheckLoginDataAsync(user, cancellationToken)).Success is false)
        {
            return BadRequest("Неверная почта или пароль.");
        }

        var claims = new List<Claim> 
        { 
            new Claim(ClaimTypes.Name, user.Email!), 
            new Claim(user.UserId.ToString(), ClaimTypes.NameIdentifier) 
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return Redirect(returnUrl ?? "/");

#warning надо добавить работу с токенами.
    }


    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody, Bind("Email", "Password")] User user, 
        [FromServices] ApprovalService approvalService, CancellationToken cancellationToken = default)
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

        var sendApprovalResult = await approvalService.SendCodeAsync(user, Models.Entities.Users.Auth.ApprovalCode.ApprovalCodeType.Registration, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
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

}