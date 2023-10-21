using FluentValidation;
using WebApi.Types.Configuration;

namespace WebApi.Types.Validation;

public class JwtConfigurationValidator : AbstractValidator<JwtConfiguration>
{
    public JwtConfigurationValidator()
    {
        RuleFor(e => e.Issuer).NotEmpty();
        RuleFor(e => e.SecurityKey).NotEmpty();
    }
}
