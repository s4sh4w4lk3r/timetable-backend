using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Teacher
{
    public int TeacherId { get; init; }
    public required string Surname { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }

    [JsonIgnore]
    public List<ActualTimetableCell>? ActualTimetableCells { get; set; }

    [JsonIgnore]
    public List<StableTimetableCell>? StableTimetableCells { get; set; }

    private Teacher() { }

    [SetsRequiredMembers]
    public Teacher(int teacherPK, string surname, string firstname, string middlename)
    {
        surname.ThrowIfNull().IfWhiteSpace();
        firstname.ThrowIfNull().IfWhiteSpace();
        middlename.ThrowIfNull().IfWhiteSpace();

        TeacherId = teacherPK;
        Surname = surname;
        FirstName = firstname;
        MiddleName = middlename;
    }

    public override string ToString()
    {
        return $"Id: {TeacherId}, {Surname} {FirstName} {MiddleName}";
    }
}
