namespace WebApi.Services.Account.Interfaces;

public interface IEmailClient
{
    void SendEmail(string message, string emailAddress);
}
