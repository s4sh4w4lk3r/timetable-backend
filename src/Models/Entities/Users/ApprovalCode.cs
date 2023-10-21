namespace Models.Entities.Users;
public class ApprovalCode
{
    public int AprrovalCodeId { get; init; }
    public int Code { get; init; } = GenerateRandomCode();
    public User? User { get; init; }
    public required int UserId { get; init; }
    public DateTime ExpiryTime { get; init; } = DateTime.UtcNow.AddMinutes(120);
    public required ApprovalCodeType CodeType { get; init; }
    public bool IsRevoked { get; private set; }

    public ApprovalCode()
    {

    }

    public bool IsNotExpired() => DateTime.UtcNow < ExpiryTime;
    public void SetRevoked() => IsRevoked = true;

    private static int GenerateRandomCode() => new Random().Next(111111, 999999);

    public enum ApprovalCodeType { Registration, Unregistration, UpdateMail, UpdatePassword }
}
