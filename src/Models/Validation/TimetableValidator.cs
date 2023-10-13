using FluentValidation;
using Models.Entities.Timetables;

namespace Models.Validation;

public class TimetableValidator : AbstractValidator<Timetable>
{
    public TimetableValidator()
    {
        RuleFor(e => e.TimetableId).NotEmpty();
        RuleForEach(x => x.TimetableCells).NotEmpty().SetValidator(new TimetableCellValidator());
        RuleFor(e => e.Group).NotNull().SetValidator(new GroupValidator()!);
        RuleFor(e => e).Must(e => e.IsTimeLessonsOk()).WithMessage("В расписании есть ячейки на одно и то же время.");
    }
}
