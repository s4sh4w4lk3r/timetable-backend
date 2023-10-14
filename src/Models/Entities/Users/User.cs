using Models.Entities.Users.Auth;

namespace Models.Entities.Users;

public class User
{
    public int UserId { get; init; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; } = UserRole.Common;

    public IList<ApprovalCode>? ApprovalCodes { get; init; } = new List<ApprovalCode>();
    public IList<UserSession>? UserSessions { get; init; } = new List<UserSession>();

    public User() { }

    public enum UserRole { Common,  Admininstrator}
}
