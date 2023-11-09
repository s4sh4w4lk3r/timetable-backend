using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Validation.IdValidators
{
    /// <summary>
    /// Проверяет все внешние ключи TimetableCell на неравенство нулю.
    /// </summary>
    public class TimetableCellIdValidator : AbstractValidator<TimetableCell>
    {
        public TimetableCellIdValidator()
        {
            RuleFor(e => e.CabinetId).NotEmpty();
            RuleFor(e => e.LessonTimeId).NotEmpty();
            RuleFor(e => e.TeacherId).NotEmpty();
            RuleFor(e => e.SubjectId).NotEmpty();
        }
    }
}
