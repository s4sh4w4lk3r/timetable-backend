using Microsoft.AspNetCore.Authentication;

namespace WebApi.Middlewares.Auth;

public class AccessTokenAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "AccessTokenAuthenticationScheme";
    public string TokenHeaderName { get; set; } = "Authorization";
}
