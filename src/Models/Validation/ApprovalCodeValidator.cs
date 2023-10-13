using FluentValidation;
using Models.Entities.Users.Auth;

namespace Models.Validation;

public class ApprovalCodeValidator : AbstractValidator<ApprovalCode>
{
    public ApprovalCodeValidator()
    {
        RuleFor(e => e.User).NotEmpty().SetValidator(new UserValidator()!);
        RuleFor(e => e).Must(e => e.IsValid());
    }
}
