using FluentValidation;
using Models.Entities.Timetables;

namespace Models.Validation;

public class TimetableValidator : AbstractValidator<Timetable>
{
    public TimetableValidator()
    {
        RuleFor(e => e.Group).NotEmpty();
        RuleFor(e => e.Group!.GroupId).NotEmpty();
        RuleForEach(x => x.TimetableCells).NotEmpty().SetValidator(new TimetableCellValidator());
        RuleFor(e => e).Must(e => e.IsTimeLessonsOk()).WithMessage("В расписании есть ячейки на одно и то же время.");
    }
}
