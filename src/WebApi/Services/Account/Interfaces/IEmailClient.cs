namespace WebApi.Services.Account.Interfaces;

public interface IEmailClient
{
    Task SendEmailAsync(string subject, string message, string emailAddress, CancellationToken cancellationToken = default);
}
