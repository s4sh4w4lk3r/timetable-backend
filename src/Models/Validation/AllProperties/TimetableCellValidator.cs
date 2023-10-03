using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation.AllProperties;

public class TimetableCellValidator : AbstractValidator<TimetableCell>
{
    public TimetableCellValidator()
    {
        RuleFor(e => e.Subject).NotEmpty();
        RuleFor(e => e.Subject).NotNull().SetValidator(new SubjectValidator()!);
        RuleFor(e => e.Cabinet).NotNull().SetValidator(new CabinetValidator()!);
        RuleFor(e => e.Teacher).NotNull().SetValidator(new TeacherValidator()!);
        RuleFor(e => e.LessonTime).NotNull().SetValidator(new LessonTimeValidator()!);
    }
}
