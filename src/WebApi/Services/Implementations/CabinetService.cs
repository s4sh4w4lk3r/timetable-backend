using FluentValidation;
using Models.Entities.Timetables.Cells;
using Repository.Interfaces;

namespace Services.Implementations;

public class CabinetService
{
    private readonly IRepository<Cabinet> _repository;
    private readonly IValidator<Cabinet> _validator;
    public IQueryable<Cabinet> Cabinets => _repository.Entites;

    public CabinetService(IRepository<Cabinet> repository, IValidator<Cabinet> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> CreateCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        if (_validator.Validate(cabinet).IsValid is false)
        {
            return false;
        }

        await _repository.InsertAsync(cabinet, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        if (_validator.Validate(cabinet).IsValid is false)
        {
            return false;
        }

        await _repository.DeleteAsync(cabinet, cancellationToken);
        return true;
    }

    public async Task<bool> UpdateCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        if (_validator.Validate(cabinet).IsValid is false)
        {
            return false;
}

        await _repository.UpdateAsync(cabinet, cancellationToken);
        return true;
    }
}