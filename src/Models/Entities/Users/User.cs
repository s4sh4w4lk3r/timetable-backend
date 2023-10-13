using Models.Entities.Users.Auth;
using System.Text.RegularExpressions;

namespace Models.Entities.Users;

public abstract class User
{
    public int UserId { get; init; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; init; }

#warning может для этих полей валидатор надо сделать
    public List<ApprovalCode>? ApprovalCodes { get; init; }

    protected User() { }
}
