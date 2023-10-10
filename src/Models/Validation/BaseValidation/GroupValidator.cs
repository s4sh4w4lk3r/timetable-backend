using FluentValidation;
using Models.Entities.Timetables;

namespace Models.Validation.BaseValidation;

public class GroupValidator : AbstractValidator<Group>
{
    public GroupValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.GroupId).NotEmpty();
    }
}
