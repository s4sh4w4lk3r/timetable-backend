using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface ITimetableRepository
{
    IQueryable<Timetable> Timetables { get; }
    Task InsertTimetableAsync(Timetable timetable);
    Task DeleteTimetableAsync(int id);
    Task UpdateTimetableAsync(Timetable timetable);
}
