using Core.Entities.Timetables;
using Core.Entities.Timetables.Cells.CellMembers;

namespace Core.Entities.Identity.Users
{
    public class Student : User
    {
        public Group? Group { get; set; }
        public int GroupId { get; set; }
        public SubGroup SubGroup { get; set; } = SubGroup.All;
    }
}
