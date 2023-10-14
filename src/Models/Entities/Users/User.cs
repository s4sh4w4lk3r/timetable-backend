using Models.Entities.Users.Auth;
using System.Text.RegularExpressions;

namespace Models.Entities.Users;

public class User
{
    public int UserId { get; init; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; } = UserRole.Common;

    public IList<ApprovalCode>? ApprovalCodes { get; init; } = new List<ApprovalCode>();

#warning может для этих полей валидатор надо сделать
    public List<ApprovalCode>? ApprovalCodes { get; init; }
    public User() { }

    public enum UserRole { Common,  Admininstrator}
}
