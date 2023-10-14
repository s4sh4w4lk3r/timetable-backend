namespace Models.Entities.Users.Auth;
public class ApprovalCode
{
    public int AprrovalCodeId { get; init; }
    public int Code { get; init; }
    public User? User { get; init; }
    public DateTime CreateTime { get; init; }
    public ApprovalCodeType CodeType { get; init; }
    public bool IsRevoked { get; private set; }

    private readonly TimeSpan ApprovalInterval = TimeSpan.FromMinutes(120);

    private ApprovalCode() { }
    public ApprovalCode(User user, ApprovalCodeType approvalCodeType)
    {
        user.ThrowIfNull();
        User = user;
        CodeType = approvalCodeType;
        Code = GenerateRandomCode();
        CreateTime = DateTime.UtcNow;
    }

    public bool IsNotExpired() => DateTime.UtcNow - CreateTime < ApprovalInterval;
    public void SetRevoked() => IsRevoked = true;

    private static int GenerateRandomCode() => new Random().Next(111111, 999999);

    public enum ApprovalCodeType { Registration, Unregistration, UpdateMail, UpdatePassword }
}
