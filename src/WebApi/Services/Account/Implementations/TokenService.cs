using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Throw;
using WebApi.Services.Account.Interfaces;
using WebApi.Types.Configuration;

namespace WebApi.Services.Account.Implementations;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration;
    public TokenService(IOptions<JwtConfiguration> options)
    {
        _jwtConfiguration = options.Value;

        _jwtConfiguration.ThrowIfNull();
        _jwtConfiguration.Issuer.ThrowIfNull().IfWhiteSpace();
        _jwtConfiguration.SecurityKey.ThrowIfNull().IfWhiteSpace();
    }
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecurityKey!));

        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ServiceResult<ClaimsPrincipal?> GetPrincipalFromAccessToken(string token, bool isLifetimeValidationRequired = true)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,

            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecurityKey!)),

            ValidateLifetime = isLifetimeValidationRequired,

            //Параметр задает мнимальное допустимое время отставание часов клиента от часов сервера,
            //использующееся при валидации времени токена. По дефолту 5 мин.
            //ClockSkew = TimeSpan.FromSeconds(10)
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return ServiceResult<ClaimsPrincipal?>.Fail("Валидация сигнатуры токена не прошла.", null);
            }

            return ServiceResult<ClaimsPrincipal?>.Ok("Валидация токена прошла успешно", principal);
        }
        catch (Exception ex)
        {
            return ServiceResult<ClaimsPrincipal?>.Fail(ex.Message, null);
        }
    }
}
