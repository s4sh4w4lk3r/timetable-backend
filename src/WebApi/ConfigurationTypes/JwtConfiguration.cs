using Microsoft.IdentityModel.Tokens;
using System.Text;
using Throw;

namespace WebApi.ConfigurationTypes;

public class JwtConfiguration
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SecurityKey { get; set; }
    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        SecurityKey.ThrowIfNull().IfWhiteSpace();
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
    }
}
