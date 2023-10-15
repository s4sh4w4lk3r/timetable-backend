using Services.Interfaces;

namespace WebApi.Services.Implementations;

public class EmailSimulator : IEmailClient
{
    public void SendEmail(string message, string emailAddress)
    {
        Console.WriteLine($"Кому: {emailAddress}, содержимое:\n{message}");
    }
}
