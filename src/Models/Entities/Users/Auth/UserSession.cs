namespace Models.Entities.Users.Auth;

public class UserSession
{
    public int UserSessionId { get; init; } 
    public User? User { get; init; }
    public string? DeviceInfo { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; init; } = DateTime.Now.AddDays(2);
    public string? AccessToken { get; set; }

    public bool IsNotExpired() => DateTime.Now < RefreshTokenExpiryTime;
}
