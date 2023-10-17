namespace Models.Entities.Users.Auth;
public class ApprovalCode
{
    public int AprrovalCodeId { get; init; }
    public int Code { get; init; }
    public User? User { get; init; }
    public DateTime ExpiryTime { get; init; } 
    public ApprovalCodeType CodeType { get; init; }
    public bool IsRevoked { get; private set; }

    private ApprovalCode() { }
    public ApprovalCode(User user, ApprovalCodeType approvalCodeType)
    {
        user.ThrowIfNull();
        User = user;
        CodeType = approvalCodeType;
        Code = GenerateRandomCode();
        ExpiryTime = DateTime.Now.AddMinutes(120);
    }

    public bool IsNotExpired() => DateTime.UtcNow < ExpiryTime;
    public void SetRevoked() => IsRevoked = true;

    private static int GenerateRandomCode() => new Random().Next(111111, 999999);

    public enum ApprovalCodeType { Registration, Unregistration, UpdateMail, UpdatePassword }
}
