﻿namespace Models.Entities.Users.Auth;

public class UserSession
{
    public int UserSessionId { get; init; } 
    public User? User { get; init; }
    public required int UserId { get; init; }
    public required string DeviceInfo { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpiryTime { get; set; }

    public UserSession()
    {
    }

    public bool RefreshIsNotExpired() => DateTime.UtcNow < RefreshTokenExpiryTime;
    public void SetNewRefreshToken(string token)
    {
        token.ThrowIfNull().IfWhiteSpace();
        RefreshToken = token;
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
    }
}
