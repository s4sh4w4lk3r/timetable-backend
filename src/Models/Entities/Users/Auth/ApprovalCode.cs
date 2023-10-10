namespace Models.Entities.Users.Auth;
public class ApprovalCode
{
    public int AprrovalCodeId { get; init; }
    public int Code { get; init; }
    public User? User { get; init; }
    public DateTime CreateTime { get; init; }

    private readonly TimeSpan ApprovalInterval = TimeSpan.FromMinutes(120);
    public bool IsValid() => DateTime.UtcNow - CreateTime < ApprovalInterval;
#warning не забудь добавить в контекст

}
