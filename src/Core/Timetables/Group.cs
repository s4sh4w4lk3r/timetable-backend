namespace Core.Timetables;

public class Group
{

    public int GroupPK { get; init; }
    public string? Name { get; init; }

    public Group(int groupPK, string name)
    {
        name.ThrowIfNull().IfWhiteSpace();
        GroupPK = groupPK;
        Name = name;
    }
}
