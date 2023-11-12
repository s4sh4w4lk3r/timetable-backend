using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class EmailSimulator : IEmailClient
{
    public Task SendEmailAsync(string subject, string message, string emailAddress, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Тема: {subject}\nКому: {emailAddress}\nСодержимое: {message}");
        return Task.CompletedTask;
    }
}
