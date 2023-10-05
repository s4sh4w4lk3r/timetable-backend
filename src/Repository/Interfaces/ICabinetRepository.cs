using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ICabinetRepository
{
    IQueryable<Cabinet> Cabinets { get; }
    Task InsertCabinetAsync(Cabinet cabinet);
    Task DeleteCabinetAsync(int id);
    Task UpdateCabinetAsync(Cabinet cabinet);
}
