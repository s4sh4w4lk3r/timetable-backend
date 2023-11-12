using Models.Entities.Timetables;

namespace Models.Entities.Identity.Users
{
    public class Student : User
    {
        public Group? Group { get; set; }
        public int GroupId { get; set; }
    }
}
