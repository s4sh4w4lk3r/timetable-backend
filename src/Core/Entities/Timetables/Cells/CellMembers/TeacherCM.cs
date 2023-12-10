using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Core.Entities.Timetables.Cells.CellMembers;

/// <summary>
/// Сущность учителя для расписания (не имя для входа).
/// </summary>
public class TeacherCM
{
    public int TeacherId { get; init; }
    public required string Lastname { get; set; }
    public required string Firstname { get; set; }
    public required string Middlename { get; set; }
    public string? AscId { get; set; }

    [JsonIgnore]
    public ICollection<ActualTimetableCell>? ActualTimetableCells { get; set; }

    [JsonIgnore]
    public ICollection<StableTimetableCell>? StableTimetableCells { get; set; }

    private TeacherCM() { }

    [SetsRequiredMembers]
    public TeacherCM(int teacherPK, string surname, string firstname, string middlename)
    {
        surname.ThrowIfNull(); ;
        firstname.ThrowIfNull();
        middlename.ThrowIfNull();

        TeacherId = teacherPK;
        Lastname = surname;
        Firstname = firstname;
        Middlename = middlename;
    }
}