namespace Models.Entities.Timetables.Cells;

public class Teacher
{

    public int TeacherPK { get; init; }
    public string? Surname { get; init; }
    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }

    public Teacher(int teacherPK, string surname, string firstname, string middlename)
    {
        surname.ThrowIfNull().IfWhiteSpace();
        firstname.ThrowIfNull().IfWhiteSpace();
        middlename.ThrowIfNull().IfWhiteSpace();

        TeacherPK = teacherPK;
        Surname = surname;
        FirstName = firstname;
        MiddleName = middlename;
    }
}
