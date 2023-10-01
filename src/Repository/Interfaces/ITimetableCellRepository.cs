using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITimetableCellRepository
{
    IQueryable<TimetableCell> TimetableCells { get; }
    Task<bool> InsertTimetableCell(TimetableCell timetableCell);
    Task<bool> DeleteTimetableCell(int id);
    Task<bool> UpdateTimetableCell(TimetableCell timetableCell);
}
