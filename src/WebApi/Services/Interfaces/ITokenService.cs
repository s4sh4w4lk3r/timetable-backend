using Models.Entities.Users.Auth;
using System.Security.Claims;

namespace WebApi.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ServiceResult<ClaimsPrincipal?> GetPrincipalFromAccessToken(string token);
}
