using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using System.Security.Claims;
using WebApi.Extensions;
using WebApi.Services;
using WebApi.Services.Account.Implementations;
using WebApi.Services.Account.Interfaces;
using WebApi.Types.Account;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class AuthController : ControllerBase
{
    private readonly IUserSessionService _userSessionService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserSessionService userSessionService, ITokenService tokenService)
    {
        _userSessionService = userSessionService;
        _tokenService = tokenService;
    }
#warning в работе с аккаунтами, принимать везде dto, выпилить Ivalidatorы
    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody, Bind("Email", "Password")] User user, [FromServices] IValidator<User> userValidator, [FromServices] PasswordService passwordService, CancellationToken cancellationToken = default)
    {
        var userValidation = userValidator.Validate(user, o => o.IncludeRuleSets("default", "password_regex").IncludeProperties(e => e.Email));
        if (userValidation.IsValid is false)
        {
            return BadRequest(userValidation);
        }

        var checkLoginDataResult = await passwordService.CheckLoginDataAsync(user, cancellationToken);
        if (checkLoginDataResult.Success is false || checkLoginDataResult.Value is null)
        {
            return BadRequest(new ServiceResult(false, "Неудачная попытка входа.", checkLoginDataResult));
        }
        user = checkLoginDataResult.Value;

        string refreshToken = _tokenService.GenerateRefreshToken();

        var userSession = new UserSession()
        {
            RefreshToken = refreshToken,
            DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString(),
            UserId = user.UserId,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty
        };

        var userSessionResult = await _userSessionService.AddAsync(userSession, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(new ServiceResult(false, "Сессия не добавлена в бд.", userSessionResult));
        }

        var claims = new List<Claim>
        {
            new Claim(TimetableClaimTypes.UserId, user.UserId.ToString()),
            new Claim(TimetableClaimTypes.UserSessionId, userSession.UserSessionId.ToString())
        };

        string accessToken = _tokenService.GenerateAccessToken(claims);


        return Ok(new TokenPair(accessToken, refreshToken));
    }

    [HttpGet, Authorize, Route("global-logout")]
    public async Task<IActionResult> GlobalLogout(CancellationToken cancellationToken = default)
    {
        if (User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось вытащить id из claimов.");
        }

        if (userId == default)
        {
            return BadRequest("Id пользователя не может быть равным нулю");
        }

        var revokeResult = await _userSessionService.DeleteAllAsync(userId, cancellationToken);
        if (revokeResult.Success is false)
        {
            return BadRequest(revokeResult);
        }

        return Ok(revokeResult);
    }

    [HttpGet, Route("token/refresh")]
    public async Task<IActionResult> Refresh([FromBody, Bind("AccessToken", "RefreshToken")] TokenPair tokenPair, CancellationToken cancellationToken)
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

        bool idOk = claimPrincipal.TryGetUserIdFromClaimPrincipal(out int userId);
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
        await _userSessionService.UpdateAsync(userSessionFromRepo, cancellationToken);

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

        bool idOk = User.TryGetUserIdFromClaimPrincipal(out int userId);
        if (idOk is false)
        {
            return BadRequest("Не получилось вытащить id из claimов.");
        }

        var userSessionResult = await _userSessionService.DeleteAsync(userId, refreshToken, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(userSessionResult);
        }

        return Ok(userSessionResult);
    }

    public record class TokenPair(string? AccessToken, string? RefreshToken);
}