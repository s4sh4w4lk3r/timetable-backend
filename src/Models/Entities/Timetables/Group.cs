namespace Models.Entities.Timetables;

public class Group
{

    public int GroupId { get; init; }
    public string? Name { get; set; }

    public IList<Timetable>? Timetables { get; init; }
    private Group() { }
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
