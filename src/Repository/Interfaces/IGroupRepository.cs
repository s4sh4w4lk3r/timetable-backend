using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface IGroupRepository
{
    IQueryable<Group> Groups { get; }
    Task InsertGroupAsync(Group group);
    Task DeleteGroupAsync(int id);
    Task UpdateGroupAsync(Group group);
}
