namespace Core.ChangeInfo;

public class TodayChanges
{
    public DateTime LastEdited { get; init; }
    public List<ChangeEntity>? ChangeEntities { get; init; }
}
