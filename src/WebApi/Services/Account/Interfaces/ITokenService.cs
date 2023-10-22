using System.Security.Claims;

namespace WebApi.Services.Account.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();

    /// <summary>
    /// Возвращает клеймы из токена. Параметр isLifetimeValidationRequired говорит о том, нужно ли проверять срок годности токена когда идет извлечение клеймов
    /// </summary>
    /// <param name="token"></param>
    /// <param name="isLifetimeValidationRequired"></param>
    /// <returns>Вернет ClaimsPrincipal если токен валидный, а если нет, то вернет null</returns>
    ServiceResult<ClaimsPrincipal?> GetPrincipalFromAccessToken(string token, bool isLifetimeValidationRequired = true);
}
