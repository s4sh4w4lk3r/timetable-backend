using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Services.Account.Implementations;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class AuthController : ControllerBase
{
    private readonly UserSessionService _userSessionService;
    private readonly ITokenService _tokenService;

    public AuthController(UserSessionService userSessionService, ITokenService tokenService)
    {
        _userSessionService = userSessionService;
        _tokenService = tokenService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody, Bind("Email", "Password")] User user, [FromServices] IValidator<User> userValidator, [FromServices] UserService userService, CancellationToken cancellationToken = default)
    {
        var userValidation = userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var checkLoginDataResult = await userService.CheckLoginDataAsync(user, cancellationToken);
        if (checkLoginDataResult.Success is false || checkLoginDataResult.Value is null)
        {
            return BadRequest(new ServiceResult(false, "Неудачная попытка входа.", checkLoginDataResult));
        }
        user = checkLoginDataResult.Value;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        string accessToken = _tokenService.GenerateAccessToken(claims);
        string refreshToken = _tokenService.GenerateRefreshToken();

        var userSession = new UserSession()
        {
            RefreshToken = refreshToken,
            DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString(),
            UserId = user.UserId,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty
        };

        var userSessionResult = await _userSessionService.AddUserSessionAsync(userSession, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(new ServiceResult(false, "Сессия не добавлена в бд.", userSessionResult));
        }

        return Ok(new TokenPair(accessToken, refreshToken));
    }

    [HttpGet, Authorize, Route("global-logout")]
    public async Task<IActionResult> GlobalLogout([FromQuery] int userId, [FromServices] UserSessionService userSessionService, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return BadRequest("Id пользователя не может быть равным нулю");
        }

        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool idOk = int.TryParse(userIdStr, out var idFromToken);
        if (idOk is false)
        {
            return BadRequest("Не получилось вытащить id из claimов.");
        }

        if (idFromToken != userId)
        {
            return BadRequest("Вы пытаетесь удалить сессии другого пользователя.");
        }

        var revokeResult = await userSessionService.RevokeAllAsync(userId, cancellationToken);
        if (revokeResult.Success is false)
        {
            return BadRequest(revokeResult);
        }

        return Ok(revokeResult);
    }

    [HttpGet, Route("token/refresh")]
    public async Task<IActionResult> Refresh([Bind("AccessToken", "RefreshToken")] TokenPair tokenPair, CancellationToken cancellationToken)
    {
        string? accessToken = tokenPair.AccessToken;
        string? refreshToken = tokenPair.RefreshToken;

        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Отсутствует RefreshToken в теле запроса.");
        }
        
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return BadRequest("Отсутствует AccessToken в заголовке Authorization.");
        }

        accessToken = accessToken.Replace("Bearer ", string.Empty);

        var tokenSerivceResult = _tokenService.GetPrincipalFromAccessToken(accessToken, isLifetimeValidationRequired: false);
        if (tokenSerivceResult.Success is false)
        {
            return BadRequest(tokenSerivceResult);
        }

        var claimPrincipal = tokenSerivceResult.Value;
        if (claimPrincipal is null)
        {
            return BadRequest("ClaimPrincipal is null.");
        }

        string? userIdStr = claimPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        bool idOk = int.TryParse(userIdStr, out var userId);
        if (idOk is false)
        {
            return BadRequest("Не получилось вытащить id из claimов.");
        }

        var userSessionFromRepo = await _userSessionService.UserSessions.SingleOrDefaultAsync(e => e.UserId == userId && e.RefreshToken == refreshToken, cancellationToken);
        if (userSessionFromRepo is null)
        {
            return BadRequest("Связка AccessToken и RefreshToken невалидная.");
        }

        if (userSessionFromRepo.RefreshTokenIsNotExpired() is false)
        {
            return BadRequest("RefreshToken просрочен.");
        }

        refreshToken = _tokenService.GenerateRefreshToken();
        accessToken = _tokenService.GenerateAccessToken(claimPrincipal.Claims);

        userSessionFromRepo.SetNewRefreshToken(refreshToken);
        userSessionFromRepo.DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString();
        userSessionFromRepo.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        await _userSessionService.UpdateUserSessionAsync(userSessionFromRepo, cancellationToken);

        return Ok(new TokenPair(accessToken, refreshToken));
    }

    [HttpPost, Authorize, Route("token/revoke")]
    public async Task<IActionResult> Revoke([FromBody] TokenPair tokenPair, CancellationToken cancellationToken)
    {
        string? refreshToken = tokenPair.RefreshToken;
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest("RefreshToken не получен.");
        }

        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool idOk = int.TryParse(userIdStr, out var id);
        if (idOk is false)
        {
            return BadRequest("Не получилось вытащить id из claimов.");
        }

        var userSessionResult = await _userSessionService.DeleteSessionAsync(id, refreshToken, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(userSessionResult);
        }

        return Ok(userSessionResult);
    }

    public record class TokenPair(string? AccessToken, string? RefreshToken);
}