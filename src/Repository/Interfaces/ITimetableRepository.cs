using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface ITimetableRepository
{
    Task<Timetable> GetTimetable(int id);
    Task<List<Timetable>> GeTimetabletList();
    Task<List<Timetable>> GetTimetableList(Predicate<Timetable> predicate);
    Task<bool> InsertTimetable(Timetable timetable);
    Task<bool> DeleteTimetable(int id);
    Task<bool> UpdateTimetable(Timetable timetable);
}
