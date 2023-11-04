using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class EmailSimulator : IEmailClient
{
    public Task SendEmailAsync(string subject, string message, string emailAddress, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Тема: {subject}\nКому: {emailAddress}\nСодержимое: {message}");
        return Task.CompletedTask;
    }
}
