namespace Core.Timetables.Cells;

public class Subject
{
    public int SubjectPK { get; init; }
    public string? Name { get; init; }

    public Subject(int subjectPK, string? name)
    {
        name.ThrowIfNull().IfWhiteSpace();
        SubjectPK = subjectPK;
        Name = name;
    }
}