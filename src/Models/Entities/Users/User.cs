using System.Text.RegularExpressions;

namespace Models.Entities.Users;

public abstract class User
{
    public int UserId { get; init; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    protected User() { }
    protected User(int userPK, string? email, string? password)
    {
        email.ThrowIfNull().IfWhiteSpace().IfNotMatches(new Regex("^\\S+@\\S+\\.\\S+$"));
        UserId = userPK;
        Email = email;
        Password = password;
    }
}
