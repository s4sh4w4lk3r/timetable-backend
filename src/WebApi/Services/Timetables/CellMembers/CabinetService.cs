using FluentValidation;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;

namespace WebApi.Services.Timetables.CellMembers;

public class CabinetService
{
    private readonly TimetableContext _dbContext;
    private readonly IValidator<Cabinet> _validator;
    public IQueryable<Cabinet> Cabinets => _dbContext.Set<Cabinet>();

    public CabinetService(TimetableContext dbContext, IValidator<Cabinet> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<ServiceResult> CreateCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        var valResult = _validator.Validate(cabinet);
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        await _dbContext.Set<Cabinet>().AddAsync(cabinet, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Кабинет внесен в базу данных.");
    }

    public async Task<ServiceResult> DeleteCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        var valResult = _validator.Validate(cabinet);
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        _dbContext.Set<Cabinet>().Remove(cabinet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Кабинет удален из базы данных.");
    }

    public async Task<ServiceResult> UpdateCabinet(Cabinet cabinet, CancellationToken cancellationToken = default)
    {
        var valResult = _validator.Validate(cabinet);
        if (valResult.IsValid is false)
        {
            return new ServiceResult(false, valResult.ToString());
        }

        _dbContext.Set<Cabinet>().Update(cabinet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Кабинет обновлен в базе данных.");
    }
}