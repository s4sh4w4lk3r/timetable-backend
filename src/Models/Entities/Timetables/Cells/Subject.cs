namespace Models.Entities.Timetables.Cells;

public class Subject
{
    public int SubjectId { get; init; }
    public string? Name { get; init; }
    public List<TimetableCell>? TimetableCells { get; set; }

    private Subject() { }
    public Subject(int subjectPK, string? name)
    {
        name.ThrowIfNull().IfWhiteSpace();
        SubjectId = subjectPK;
        Name = name;
    }
}