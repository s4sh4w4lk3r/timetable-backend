using FluentValidation;
using Models.Entities.Users;

namespace Models.Validation.BaseValidation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(e => e.Email).NotEmpty();
        RuleFor(e => e.Password).NotEmpty();
        RuleFor(e => e.UserId).NotEmpty();
    }
}
