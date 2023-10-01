using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface IGroupRepository
{
    IQueryable<Group> Groups { get; }
    Task<bool> InsertGroup(Group group);
    Task<bool> DeleteGroup(int id);
    Task<bool> UpdateGroup(Group group);
}
