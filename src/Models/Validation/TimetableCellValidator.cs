using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation;

public class TimetableCellValidator : AbstractValidator<TimetableCell>
{
    public TimetableCellValidator()
    {
        RuleFor(e => e.Subject).NotEmpty();
        RuleFor(e=>e.Subject!.SubjectId).NotEmpty();

        RuleFor(e => e.Cabinet).NotNull().SetValidator(new CabinetValidator()!);
        RuleFor(e => e.Cabinet!.CabinetId).NotEmpty();

        RuleFor(e => e.Teacher).NotNull().SetValidator(new TeacherValidator()!);
        RuleFor(e=>e.Teacher!.TeacherId).NotEmpty();

        RuleFor(e => e.LessonTime).NotNull().SetValidator(new LessonTimeValidator()!);
        RuleFor(e => e.LessonTime!.LessonTimeId).NotEmpty();
    }
}
