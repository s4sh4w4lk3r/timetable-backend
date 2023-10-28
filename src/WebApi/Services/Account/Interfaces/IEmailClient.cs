namespace WebApi.Services.Account.Interfaces;

public interface IEmailClient
{
    Task SendEmail(string subject, string message, string emailAddress);
}
