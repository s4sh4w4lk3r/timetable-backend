using FluentValidation;
using Core.Entities.Identity;

namespace Validation;

public class UserSessionValidator : AbstractValidator<UserSession>
{
    public UserSessionValidator()
    {
        RuleFor(e => e.DeviceInfo).NotEmpty();
        RuleFor(e => e.RefreshToken).NotEmpty();
        RuleFor(e => e.UserId).NotEmpty();
    }
}
