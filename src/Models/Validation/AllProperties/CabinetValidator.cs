using FluentValidation;
using Models.Entities.Timetables.Cells;

namespace Models.Validation.AllProperties;

public class CabinetValidator : AbstractValidator<Cabinet>
{
    public CabinetValidator()
    {
        RuleFor(e => e.Address).NotEmpty();
        RuleFor(e => e.CabinetId).NotEmpty();
        RuleFor(e => e.Number).NotEmpty();
    }
}
