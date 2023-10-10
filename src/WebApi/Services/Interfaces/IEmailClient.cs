namespace Services.Interfaces;

public interface IEmailClient
{
    void SendEmail(string message, string emailAddress);
}
