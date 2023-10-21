using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WebApi.Services.Account.Implementations;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Middlewares.Auth;

public class AccessTokenAuthenticationHandler : AuthenticationHandler<AccessTokenAuthenticationOptions>
{
    private readonly UserService _userService;
    private readonly ITokenService _tokenService;
    public AccessTokenAuthenticationHandler(IOptionsMonitor<AccessTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
        UserService userService, ITokenService tokenService) : base(options, logger, encoder, clock)
    {
        _tokenService = tokenService;
        _userService = userService;
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

        string? idStr = claimsPrinciapal.FindFirstValue(ClaimTypes.NameIdentifier);
        bool idOk = int.TryParse(idStr, out int id);
        bool emailOk = MailAddress.TryCreate(claimsPrinciapal.FindFirstValue(ClaimTypes.Email), out _);

        if (idOk is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Id имеет неверный формат.");
        }

        if (id == default)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Id равен нулю.");
        }

        if (emailOk is false)
        {
            return AuthenticateResult.Fail("Валидация AccessToken не прошла. Email имеет неверный формат.");
        }

        var userServiceResult = await _userService.CheckUserExist(id);
        if (userServiceResult.Success is false)
        {
            return AuthenticateResult.Fail($"Валидация AccessToken не прошла. {userServiceResult.Description}");
        }

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrinciapal, this.Scheme.Name));
    }
}
