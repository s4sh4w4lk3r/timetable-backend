using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Entities.Identity;
using Repository;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WebApi.Extensions;
using WebApi.Services.Identity.Interfaces;
using WebApi.Types.Account;

namespace WebApi.Middlewares.Authentication;

public class AccessTokenAuthenticationHandler : AuthenticationHandler<AccessTokenAuthenticationOptions>
{
    private readonly TimetableContext _dbContext;
    private readonly ITokenService _tokenService;
    public AccessTokenAuthenticationHandler(IOptionsMonitor<AccessTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
        TimetableContext dbContext, ITokenService tokenService) : base(options, logger, encoder, clock)
    {
        logger.CreateLogger<AccessTokenAuthenticationHandler>();
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.ContainsKey(Options.TokenHeaderName) is false)
        {
            return AuthenticateResult.Fail($"Отсуствует заголовок: {Options.TokenHeaderName}.");
        }

        string? token = Request.Headers[Options.TokenHeaderName];
        if (string.IsNullOrWhiteSpace(token))
        {
            return AuthenticateResult.Fail($"Заголовок не содержит токена: {Options.TokenHeaderName}.");
        }
        token = token.Replace("Bearer ", string.Empty);

        var claimsPrinciapalResult = _tokenService.GetPrincipalFromAccessToken(token);
        if (claimsPrinciapalResult.Success is false || claimsPrinciapalResult.Value is null)
        {
            return AuthenticateResult.Fail($"Валидация AccessToken не прошла. {claimsPrinciapalResult}.");
        }
        var claimsPrinciapal = claimsPrinciapalResult.Value;

        string? userSessionIdStr = claimsPrinciapal.FindFirstValue(TimetableClaimTypes.UserSessionId);

        bool userIdOk = claimsPrinciapal.TryGetUserIdFromClaimPrincipal(out int userId);
        bool userSessionIdOk = int.TryParse(userSessionIdStr, out int userSessionId);
        bool userRoleOk = !string.IsNullOrWhiteSpace(claimsPrinciapal.FindFirstValue(TimetableClaimTypes.UserRole));

        if (userIdOk is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Id имеет неверный формат.");
        }

        if (userId == default)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Id равен нулю.");
        }

        if (userSessionIdOk is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. UserSessionId имеет неверный формат.");
        }

        if (userSessionId == default)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. UserSessionId равен нулю.");
        }

        if (userRoleOk is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Роль не указана.");
        }

        bool userAndSessionIsMatch = await _dbContext.Set<UserSession>().AnyAsync(e => e. UserId == userId && e.UserSessionId == userSessionId);
        if (userAndSessionIsMatch is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Не найдена пара UserId и UserSessionId в бд.");
        }


        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrinciapal, this.Scheme.Name));
    }
}