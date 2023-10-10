using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation.BaseValidation;

public class LessonTimeValidator : AbstractValidator<LessonTime>
{
    public LessonTimeValidator()
    {
        RuleFor(e => e.From).NotEmpty();
        RuleFor(e => e.To).NotEmpty();
        RuleFor(e => e.LessonTimeId).NotEmpty();
        RuleFor(e => e.DayOfWeek).IsInEnum();
    }
}
