using FluentValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using WebApi.Services.Account.Interfaces;
using WebApi.Types.Configuration;

namespace WebApi.Services.Account.Implementations
{
    public class MailKitClient : IEmailClient
    {
        private readonly EmailConfiguration _emailConfiguration;

        public MailKitClient(IOptions<EmailConfiguration> options, IValidator<EmailConfiguration> validator)
        {
            _emailConfiguration = options.Value;
            validator.ValidateAndThrow(_emailConfiguration);
        }


        public async Task SendEmail(string subject, string message, string emailAddress)
        {
            using var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailConfiguration.Sender));
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = message };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, SecureSocketOptions.Auto);
            await smtp.AuthenticateAsync(_emailConfiguration.Login, _emailConfiguration.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
