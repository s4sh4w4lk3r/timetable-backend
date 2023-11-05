using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables;

public class Group
{
    public int GroupId { get; init; }
    public required string Name { get; set; }

    private Group() { }

    [SetsRequiredMembers]
    public Group(int groupPk, string name)
    {
        name.ThrowIfNull().IfWhiteSpace();

        GroupId = groupPk;
        Name = name;
    }

    public override string ToString()
    {
        return $"Id: {GroupId}, Name: {Name}";
    }
}
