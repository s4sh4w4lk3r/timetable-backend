namespace Models.Entities.Users
{
    internal class Administrator : User
    {
        public Administrator(int userPK, string? email, string? password) : base(userPK, email, password)
        {

        }
    }
}
