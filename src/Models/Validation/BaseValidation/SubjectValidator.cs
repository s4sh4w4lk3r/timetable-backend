using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation.BaseValidation;

public class SubjectValidator : AbstractValidator<Subject>
{
    public SubjectValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.SubjectId).NotEmpty();
    }
}
