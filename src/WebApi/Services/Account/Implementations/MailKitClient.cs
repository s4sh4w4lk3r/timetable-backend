using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Throw;
using WebApi.Services.Account.Interfaces;
using WebApi.Types.Configuration;

namespace WebApi.Services.Account.Implementations
{
    public class MailKitClient : IEmailClient
    {
        private readonly EmailConfiguration _emailConfiguration;

        public MailKitClient(IOptions<EmailConfiguration> options)
        {
            _emailConfiguration = options.Value;
            _emailConfiguration.Login.ThrowIfNull().IfEmpty();
            _emailConfiguration.Host.ThrowIfNull().IfEmpty();
            _emailConfiguration.Password.ThrowIfNull().IfEmpty();
            _emailConfiguration.Port.ThrowIfNull().IfDefault();
            _emailConfiguration.Sender.ThrowIfNull().IfEmpty();
        }

        public async Task SendEmailAsync(string subject, string message, string emailAddress, CancellationToken cancellationToken = default)
        {
            using var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailConfiguration.Sender));
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = message };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, SecureSocketOptions.Auto, cancellationToken);
            await smtp.AuthenticateAsync(_emailConfiguration.Login, _emailConfiguration.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
