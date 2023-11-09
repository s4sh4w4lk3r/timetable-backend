namespace Models.Users
{
    public abstract class TimetableUser
    {
        public int UserId { get; set; }
        public string? Lastname { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
    }
}
