using FluentValidation;
using Models.Entities.Users.Auth;

namespace Models.Validation;

public class UserSessionValidator : AbstractValidator<UserSession>
{
    public UserSessionValidator()
    {
        RuleFor(e=>e.AccessToken).NotEmpty();
        RuleFor(e=>e.DeviceInfo).NotEmpty();
        RuleFor(e=>e.RefreshToken).NotEmpty();
        RuleFor(e => e.User).NotEmpty();
        RuleFor(e => e.User!.UserId).NotEmpty();
        RuleFor(e=>e.AccessToken).NotEmpty();
    }
}
