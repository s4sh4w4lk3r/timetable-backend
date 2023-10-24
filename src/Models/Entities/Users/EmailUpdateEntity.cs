namespace Models.Entities.Users;

public class EmailUpdateEntity
{
    public int EmailUpdateEntityId { get; init; }

    public required int UserId { get; init; }
    public User? User { get; init; }

    public required string? OldEmail { get; init; }
    public required string? NewEmail { get; init; }

    public required int ApprovalId { get; init; }
    public ApprovalCode? Approval { get; init; }
}
