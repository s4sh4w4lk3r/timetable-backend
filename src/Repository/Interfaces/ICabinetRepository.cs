using Models.Entities.Timetables.Cells;
using System.Text.RegularExpressions;

namespace Repository.Interfaces;

public interface ICabinetRepository
{
    Task<Cabinet?> GetCabinet(int id);
    Task<List<Cabinet>?> GetCabinetList();
    Task<List<Cabinet>?> GetCabinetList(Predicate<Group> predicate);
    Task<bool> InsertCabinet(Cabinet cabinet);
    Task<bool> DeleteCabinet(int id);
    Task<bool> UpdateCabinet(Cabinet cabinet);
}
