using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class EmailSimulator : IEmailClient
{
    public void SendEmail(string message, string emailAddress)
    {
        Console.WriteLine($"Кому: {emailAddress}, содержимое:\n{message}");
    }
}
