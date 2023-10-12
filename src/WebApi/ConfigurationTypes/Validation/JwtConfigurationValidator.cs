using FluentValidation;

namespace WebApi.ConfigurationTypes.Validation;

public class JwtConfigurationValidator : AbstractValidator<JwtConfiguration>
{
    public JwtConfigurationValidator()
    {
        RuleFor(e => e.Audience).NotEmpty();
        RuleFor(e=>e.Issuer).NotEmpty();
        RuleFor(e=>e.SecurityKey).NotEmpty();
    }
}
