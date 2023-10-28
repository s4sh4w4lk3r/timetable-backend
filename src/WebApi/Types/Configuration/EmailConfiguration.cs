namespace WebApi.Types.Configuration
{
    public class EmailConfiguration
    {
        public required string Sender { get; init; }
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required string Login { get; init; }
        public required string Password { get; init; }
    }
}
