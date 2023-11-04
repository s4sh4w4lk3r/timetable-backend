using FluentValidation;
using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Validation;

public class TeacherValidator : AbstractValidator<Teacher>
{
    public TeacherValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.Surname).NotEmpty();
        RuleFor(e => e.MiddleName).NotEmpty();
    }
}
