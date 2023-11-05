using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Subject
{
    public int SubjectId { get; init; }
    public required string Name { get; set; }
    public List<ActualTimetableCell>? ActualTimetableCells { get; set; }
    public List<StableTimetableCell>? StableTimetableCells { get; set; }

    private Subject() { }

    [SetsRequiredMembers]
    public Subject(int subjectPK, string? name)
    {
        name.ThrowIfNull().IfWhiteSpace();

        SubjectId = subjectPK;
        Name = name;
    }

    public override string ToString()
    {
        return $"Id: {SubjectId}, Name: {Name}";
    }
}