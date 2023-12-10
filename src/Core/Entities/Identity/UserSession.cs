using Core.Entities.Identity.Users;

namespace Core.Entities.Identity;

public class UserSession
{
    public int UserSessionId { get; init; }
    public User? User { get; init; }
    public int UserId { get; init; }
    public required string DeviceInfo { get; set; }
    public required string IpAddress { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpiryTime { get; set; }

    public UserSession()
    {
    }

    public bool RefreshTokenIsNotExpired() => DateTime.UtcNow < RefreshTokenExpiryTime;
    public void SetNewRefreshToken(string token)
    {
        token.ThrowIfNull().IfWhiteSpace();
        RefreshToken = token;
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
    }
}
