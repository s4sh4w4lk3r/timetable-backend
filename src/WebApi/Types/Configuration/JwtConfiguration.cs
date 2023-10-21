using Microsoft.IdentityModel.Tokens;
using System.Text;
using Throw;

namespace WebApi.Types.Configuration;

public class JwtConfiguration
{
    public required string Issuer { get; set; }
    public required string SecurityKey { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey() => GetSymmetricSecurityKey(SecurityKey);
    public static SymmetricSecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        securityKey.ThrowIfNull().IfWhiteSpace();
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}
