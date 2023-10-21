using System.Security.Claims;

namespace WebApi.Services.Account.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ServiceResult<ClaimsPrincipal?> GetPrincipalFromAccessToken(string token, bool isLifetimeValidationRequired = true);
}
