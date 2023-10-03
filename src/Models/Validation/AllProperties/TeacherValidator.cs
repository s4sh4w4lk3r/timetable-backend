using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation.AllProperties;

public class TeacherValidator : AbstractValidator<Teacher>
{
    public TeacherValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.Surname).NotEmpty();
        RuleFor(e => e.MiddleName).NotEmpty();
        RuleFor(e => e.TeacherId).NotEmpty();
    }
}
