using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Identity.Users
{
    public class Student : User
    {
        public Group? Group { get; set; }
        public int GroupId { get; set; }
        public SubGroup SubGroup { get; set; } = SubGroup.All;

#warning проверить добавляется ли подругрппа студента при регистрации
    }
}
