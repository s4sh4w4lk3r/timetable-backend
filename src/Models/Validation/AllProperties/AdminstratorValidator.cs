using FluentValidation;
using Models.Entities.Users;

namespace Models.Validation.AllProperties;

public class AdminstratorValidator : AbstractValidator<Administrator>
{
    public AdminstratorValidator()
    {
        RuleFor(e => e.Email).NotEmpty();
        RuleFor(e => e.Password).NotEmpty();
        RuleFor(e => e.UserId).NotEmpty();
    }
}
