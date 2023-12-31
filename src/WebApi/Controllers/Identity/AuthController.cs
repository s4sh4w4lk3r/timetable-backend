﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Identity;
using Core.Entities.Identity.Users;
using System.Security.Claims;
using Validation;
using WebApi.Extensions;
using WebApi.Services;
using WebApi.Services.Identity.Implementations;
using WebApi.Services.Identity.Interfaces;
using WebApi.Types.Account;

namespace WebApi.Controllers.Identity;

[ApiController, Route("identity")]
public class AuthController : ControllerBase
{
    private readonly IUserSessionService _userSessionService;
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;

    public AuthController(IUserSessionService userSessionService, ITokenService tokenService, ILoggerFactory loggerFactory)
    {
        _userSessionService = userSessionService;
        _tokenService = tokenService;
        _logger = loggerFactory.CreateLogger<AuthController>();
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody, Bind("Email", "Password")] EmailPasswordPairDto emailPasswordPair, [FromServices] IPasswordService passwordService, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(emailPasswordPair.Email) is false)
        {
            return BadRequest("Email имеет неверный формат.");
        }

        if (StaticValidator.ValidatePassword(emailPasswordPair.Password) is false)
        {
            return BadRequest("Пароль не соответствует минимальным требованиям безопасности.");
        }

        var checkLoginDataResult = await passwordService.CheckLoginDataAsync(emailPasswordPair.Email!, emailPasswordPair.Password!, cancellationToken);
        if (checkLoginDataResult.Success is false || checkLoginDataResult.Value is null)
        {
            return BadRequest(new ServiceResult(false, "Неудачная попытка входа.", checkLoginDataResult));
        }
        var userFromRepo = checkLoginDataResult.Value;

        string refreshToken = _tokenService.GenerateRefreshToken();

        var userSession = new UserSession()
        {
            RefreshToken = refreshToken,
            DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString(),
            UserId = userFromRepo.UserId,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0"
        };

        var userSessionResult = await _userSessionService.AddAsync(userSession, cancellationToken);
        if (userSessionResult.Success is false)
        {
            return BadRequest(new ServiceResult(false, "Сессия не добавлена в бд.", userSessionResult));
        }

        var claims = new List<Claim>
        {
            new Claim(TimetableClaimTypes.UserId, userFromRepo.UserId.ToString()),
            new Claim(TimetableClaimTypes.UserSessionId, userSession.UserSessionId.ToString()),
        };

        switch (userFromRepo)
        {
            case Student:
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, TimetableRoles.Student));
                break;

            case Teacher:
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, TimetableRoles.Teacher));
                break;

            case Admin:
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, TimetableRoles.Admin));
                break;

            default:
                string errorMessage = "[AuthController, Login] Не получилось задаункастить тип юзера, полученного из бд.";
                _logger.LogCritical(errorMessage);
                return StatusCode(500, errorMessage);
        }

        string accessToken = _tokenService.GenerateAccessToken(claims);

        return Ok(new TokenPairDto(accessToken, refreshToken));
    }

    [HttpPost, Authorize, Route("terminate-all-sessions")]
    public async Task<IActionResult> TermainateAllSessions(CancellationToken cancellationToken = default)
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

    [HttpPost, Route("token/refresh")]
    public async Task<IActionResult> RefreshToken([FromBody, Bind("AccessToken", "RefreshToken")] TokenPairDto tokenPair, CancellationToken cancellationToken)
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

        return Ok(new TokenPairDto(accessToken, refreshToken));
    }

    [HttpPost, Authorize, Route("token/revoke")]
    public async Task<IActionResult> TermainateSession([FromBody, Bind("AccessToken", "RefreshToken")] TokenPairDto tokenPair, CancellationToken cancellationToken)
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

    [HttpGet, Authorize, Route("whoami")]
    public IActionResult CheckAuthorization()
    {
        bool? isAuthenticated = HttpContext.User?.Identity?.IsAuthenticated;
        if (isAuthenticated is false || isAuthenticated is null)
        {
            return Unauthorized();
        }
        string? rolename = HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
        return Ok(rolename);
    }

    public record class TokenPairDto(string? AccessToken, string? RefreshToken);
    public record class EmailPasswordPairDto(string? Email, string? Password);
}