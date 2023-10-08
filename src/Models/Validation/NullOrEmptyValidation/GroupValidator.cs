using FluentValidation;
using Models.Entities.Timetables;

namespace Models.Validation.AllProperties;

public class GroupValidator : AbstractValidator<Group>
{
    public GroupValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.GroupId).NotEmpty();
    }
}
