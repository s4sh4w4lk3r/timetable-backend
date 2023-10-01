using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ICabinetRepository
{
    IQueryable<Cabinet> Cabinets { get; }
    Task<bool> InsertCabinet(Cabinet cabinet);
    Task<bool> DeleteCabinet(int id);
    Task<bool> UpdateCabinet(Cabinet cabinet);
}
