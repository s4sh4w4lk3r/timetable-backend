using FluentValidation;
using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Validation;

public class CabinetValidator : AbstractValidator<Cabinet>
{
    public CabinetValidator()
    {
        RuleFor(e => e.Address).NotEmpty();
        RuleFor(e => e.Number).NotEmpty();
    }
}
