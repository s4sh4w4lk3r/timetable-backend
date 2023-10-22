using System.Security.Claims;

namespace WebApi.Extensions;

public static class ClaimPrincipalExtension
{
    public static bool TryGetIdFromClaimPrincipal(this ClaimsPrincipal claimPrincipal, out int userId)
    {
        var idStr = claimPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out userId);
    }

}
