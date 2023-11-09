using FluentValidation;
using Models.Entities.Users;
using System.Text.RegularExpressions;

namespace Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(e => e.Email).NotEmpty().Matches(new Regex("^\\S+@\\S+\\.\\S+$")).WithMessage("Email имеет неверный формат");
        RuleFor(e => e.Password).NotEmpty();

        RuleSet("password_regex", () =>
        {
            RuleFor(e => e.Password).Matches(new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
            .WithMessage("В пароле должно быть не менее 8 символов, большие и маленькие латинские буквы, цифры и спецсимволы #?!@$%^&*-");
        });
    }
}
