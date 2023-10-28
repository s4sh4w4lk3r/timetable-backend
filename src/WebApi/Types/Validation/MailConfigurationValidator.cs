using FluentValidation;
using WebApi.Types.Configuration;

namespace WebApi.Types.Validation;

public class MailConfigurationValidator : AbstractValidator<EmailConfiguration>
{
    public MailConfigurationValidator()
    {
        RuleFor(e => e.Sender).NotEmpty();
        RuleFor(e => e.Host).NotEmpty();
        RuleFor(e => e.Login).NotEmpty();
        RuleFor(e => e.Password).NotEmpty();
        RuleFor(e => e.Port).NotEmpty();
    }
}
