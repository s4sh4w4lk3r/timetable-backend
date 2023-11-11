namespace Models.Entities.Identity.Users
{
    /// <summary>
    /// Класс учителя для хранения данных для входа в бд.
    /// </summary>
    public class Teacher : User
    {
        public Timetables.Cells.CellMembers.TeacherCM? TeacherCellMember { get; init; }
        public int? TeacherCellMemberId { get; init; }
    }
}
