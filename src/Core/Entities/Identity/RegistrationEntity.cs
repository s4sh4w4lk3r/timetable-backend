using Core.Entities.Timetables;
using Core.Entities.Timetables.Cells.CellMembers;

namespace Core.Entities.Identity
{
    public class RegistrationEntity
    {
        public int RegistrationEntityId { get; set; }
        public string? SecretKey { get; set; }
        public DateTime CodeExpires { get; set; }
        public Role DesiredRole { get; set; }
        public int? StudentGroupId { get; set; }
        public SubGroup? SubGroup { get; set; }
        public Group? Group { get; set; }

        public bool IsCodeNotExpired() => DateTime.UtcNow < CodeExpires;

        public enum Role
        {
            Student = 0,
            Teacher = 1,
            Admin = 2
        }
    }
}
