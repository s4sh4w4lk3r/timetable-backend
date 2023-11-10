using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells.CellMembers;

/// <summary>
/// Сущность учителя для расписания (не имя для входа).
/// </summary>
public class Teacher
{
    public int TeacherId { get; init; }
    public required string Surname { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }

    [JsonIgnore]
    public ICollection<ActualTimetableCell>? ActualTimetableCells { get; set; }

    [JsonIgnore]
    public ICollection<StableTimetableCell>? StableTimetableCells { get; set; }

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
}