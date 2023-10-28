using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class EmailSimulator : IEmailClient
{
    public Task SendEmail(string subject, string message, string emailAddress)
    {
        Console.WriteLine($"Тема: {subject}\nКому: {emailAddress}\nСодержимое:{message}");
        return Task.CompletedTask;
    }
}
