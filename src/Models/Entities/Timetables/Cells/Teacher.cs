namespace Models.Entities.Timetables.Cells;

public class Teacher
{

    public int TeacherId { get; init; }
    public string? Surname { get; init; }
    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }
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
}
