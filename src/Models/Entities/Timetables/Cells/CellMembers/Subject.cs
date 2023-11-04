using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Subject
{
    public int SubjectId { get; init; }
    public required string? Name { get; set; }
    public List<TimetableCell>? TimetableCells { get; set; }

    private Subject() { }

    [SetsRequiredMembers]
    public Subject(int subjectPK, string? name)
    {
        name.ThrowIfNull().IfWhiteSpace();
        subjectPK.Throw().IfDefault();

        SubjectId = subjectPK;
        Name = name;
    }

    public override string ToString()
    {
        return $"Id: {SubjectId}, Name: {Name}";
    }
}