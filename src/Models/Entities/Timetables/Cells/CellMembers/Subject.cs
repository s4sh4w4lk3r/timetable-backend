using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Subject
{
    public int SubjectId { get; init; }
    public required string Name { get; set; }
    public string? AscId { get; set; }

    [JsonIgnore]
    public ICollection<ActualTimetableCell>? ActualTimetableCells { get; set; }

    [JsonIgnore]
    public ICollection<StableTimetableCell>? StableTimetableCells { get; set; }

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