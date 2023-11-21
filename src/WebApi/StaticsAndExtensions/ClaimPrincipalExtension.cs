using System.Security.Claims;
using WebApi.Types.Account;

namespace WebApi.Extensions;

public static class ClaimPrincipalExtension
{
    /// <summary>
    /// Пытается получить Id из Claimов.
    /// </summary>
    /// <param name="claimPrincipal"></param>
    /// <param name="userId"></param>
    /// <returns>True, если парсинг прошел успешно, в противном случае False.</returns>
    public static bool TryGetUserIdFromClaimPrincipal(this ClaimsPrincipal claimPrincipal, out int userId)
    {
        var idStr = claimPrincipal.FindFirstValue(TimetableClaimTypes.UserId);
        return int.TryParse(idStr, out userId);
    }

}
