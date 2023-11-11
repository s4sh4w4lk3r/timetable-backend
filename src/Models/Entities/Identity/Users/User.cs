using System.Text.Json.Serialization;

namespace Models.Entities.Identity.Users
{
    public abstract class User
    {
        public int UserId { get; set; }
        public required string Lastname { get; set; }
        public required string Firstname { get; set; }
        public required string Middlename { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsEmailConfirmed { get; set; }

        [JsonIgnore] 
        public ICollection<UserSession>? UserSessions { get; set; }

        [JsonIgnore] 
        public ICollection<Approval>? Approvals { get; set; }

        [JsonIgnore] 
        public ICollection<EmailUpdateEntity>? EmailUpdateEntities { get; set; }
    }
}
