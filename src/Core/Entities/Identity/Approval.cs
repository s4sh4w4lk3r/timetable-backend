using Core.Entities.Identity.Users;
using System.Text.Json.Serialization;

namespace Core.Entities.Identity
{
    public class Approval
    {
        public int AprrovalId { get; init; }
        public int Code { get; init; } = GenerateRandomCode();
        public User? User { get; init; }
        public required int UserId { get; init; }
        public DateTime ExpiryTime { get; init; } = DateTime.UtcNow.AddMinutes(120);
        public required ApprovalCodeType CodeType { get; init; }
        public bool IsRevoked { get; private set; } = false;

        [JsonIgnore]
        public EmailUpdateEntity? EmailUpdateEntity { get; init; }

        public bool IsNotExpired() => DateTime.UtcNow < ExpiryTime;
        public void SetRevoked() => IsRevoked = true;

        private static int GenerateRandomCode() => new Random().Next(111111, 999999);

        public enum ApprovalCodeType
        {
            Registration = 0,
            Unregistration = 1,
            UpdateMail = 2
        }
    }
}
