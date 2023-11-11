namespace Models.Entities.Identity.Users
{
    public abstract class User
    {
        public int UserId { get; set; }
        public required string Lastname { get; set; }
        public required string Firstname { get; set; }
        public required string Middlename { get; set; }

        public ICollection<UserSession>? UserSessions { get; set; }
    }
}
