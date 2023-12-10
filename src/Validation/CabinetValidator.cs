using FluentValidation;
using Core.Entities.Timetables.Cells.CellMembers;

namespace Validation;

public class CabinetValidator : AbstractValidator<Cabinet>
{
    public CabinetValidator()
    {
        RuleFor(e => e.Address).NotEmpty();
        RuleFor(e => e.Number).NotEmpty();
    }
}
