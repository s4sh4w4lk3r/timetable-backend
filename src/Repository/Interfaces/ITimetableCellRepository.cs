using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITimetableCellRepository
{
    Task<TimetableCell?> GetTimetableCell(int id);
    Task<List<TimetableCell>?> GetTimetableCellList();
    Task<List<TimetableCell>?> GetTimetableCellList(Predicate<TimetableCell> predicate);
    Task<bool> InsertTimetableCell(TimetableCell timetableCell);
    Task<bool> DeleteTimetableCell(int id);
    Task<bool> UpdateTimetableCell(TimetableCell timetableCell);
}
