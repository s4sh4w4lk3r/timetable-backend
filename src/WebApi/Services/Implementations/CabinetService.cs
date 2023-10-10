using FluentValidation;
using Models.Entities.Timetables.Cells;
using Repository.Interfaces;

namespace Services.Implementations;

public class CabinetService
{
    private readonly IRepository<Cabinet> _repository;
    private readonly IValidator<Cabinet> _validator;
    public CabinetService(IRepository<Cabinet> repository, IValidator<Cabinet> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    /*public async Task<bool> CreateCabinet(Cabinet cabinet)
    {
        if (_validator.Validate(cabinet).IsValid is false)
        {
            return false;
        }
    }*/
}
#warning не забудь
