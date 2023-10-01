using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface ITimetableRepository
{
    IQueryable<Timetable> Timetables { get; }
    Task<bool> InsertTimetable(Timetable timetable);
    Task<bool> DeleteTimetable(int id);
    Task<bool> UpdateTimetable(Timetable timetable);
}
