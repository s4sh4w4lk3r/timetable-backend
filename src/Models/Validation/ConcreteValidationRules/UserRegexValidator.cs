using FluentValidation;
using Models.Entities.Users;
using Models.Validation.BaseValidation;
using System.Text.RegularExpressions;

namespace Models.Validation.ConcreteValidationRules;

public class UserRegexValidator : AbstractValidator<User>
{
    public UserRegexValidator()
    {
        RuleFor(e => e).SetValidator(new UserValidator());
        RuleFor(e => e.Password).Matches(new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"));
        RuleFor(e => e.Email).Matches(new Regex("^\\S+@\\S+\\.\\S+$"));
    }
}
