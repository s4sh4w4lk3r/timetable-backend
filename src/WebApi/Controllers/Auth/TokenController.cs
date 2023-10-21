using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/token")]
public class TokenController : ControllerBase
{
    private readonly UserSessionService _userSessionService;
    private readonly ITokenService _tokenService;

    public TokenController(UserSessionService userSessionService, ITokenService tokenService)
    {
        _userSessionService = userSessionService;
        _tokenService = tokenService;
    }

    [HttpGet, Route("refresh")]
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


    [HttpPost, Authorize, Route("revoke")]
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