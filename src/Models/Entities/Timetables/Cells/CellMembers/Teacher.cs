using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Teacher
{
    public int TeacherId { get; init; }
    public required string? Surname { get; set; }
    public required string? FirstName { get; set; }
    public required string? MiddleName { get; set; }
    public List<TimetableCell>? TimetableCells { get; set; }

    private Teacher() { }

    [SetsRequiredMembers]
    public Teacher(int teacherPK, string surname, string firstname, string middlename)
    {
        teacherPK.Throw().IfDefault();
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
