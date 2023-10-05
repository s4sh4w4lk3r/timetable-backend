using Models.Entities.Timetables.Cells;

namespace Repository.Interfaces;

public interface ITimetableCellRepository
{
    IQueryable<TimetableCell> TimetableCells { get; }
    Task InsertTimetableCellAsync(TimetableCell timetableCell);
    Task DeleteTimetableCellAsync(int id);
    Task UpdateTimetableCellAsync(TimetableCell timetableCell);
}
