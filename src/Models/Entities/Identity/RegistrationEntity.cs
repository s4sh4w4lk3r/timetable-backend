using Models.Entities.Timetables;
using System.Security.Cryptography;
using System.Text;

namespace Models.Entities.Identity
{
    public class RegistrationEntity
    {
        public int RegistrationEntityId { get; set; }
        public string? SecretKey { get; set; }
        public DateTime CodeExpires { get; set; }
        public Role DesiredRole { get; set; }
        public int StudentGroupId { get; set; }
        public Group? Group { get; set; }

        public bool IsCodeNotExpired() => DateTime.UtcNow < CodeExpires;


        public static string GenerateSecretKey()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var sb = new StringBuilder(Convert.ToBase64String(randomNumber));
            sb = sb.Replace("=", "$").Replace("/", "!");
            return sb.ToString();
        }

        public enum Role
        {
            Student = 0, 
            Teacher = 1,
            Admin = 2
        }
    }
}
