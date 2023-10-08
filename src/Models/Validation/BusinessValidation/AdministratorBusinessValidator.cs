using FluentValidation;
using Models.Entities.Users;
using Models.Validation.AllProperties;
using System.Text.RegularExpressions;

namespace Models.Validation.BusinessValidation;

public class AdministratorBusinessValidator : AbstractValidator<Administrator>
{
    public AdministratorBusinessValidator()
    {
        RuleFor(e => e).SetValidator(new AdminstratorValidator());
        RuleFor(e => e.Password).Matches(new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"));
        RuleFor(e => e.Email).Matches(new Regex("^\\S+@\\S+\\.\\S+$"));
    }
}
