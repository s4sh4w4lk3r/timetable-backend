using Models.Entities.Timetables;

namespace Models.Users
{
    public class Student : TimetableUser
    {
        public Group? Group { get; set; }
        public int GroupId { get; set; }
    }
}
