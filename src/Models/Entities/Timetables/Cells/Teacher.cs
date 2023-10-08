namespace Models.Entities.Timetables.Cells;

public class Teacher
{

    public int TeacherId { get; init; }
    public string? Surname { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public List<TimetableCell>? TimetableCells { get; set; }

    private Teacher() { }
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
