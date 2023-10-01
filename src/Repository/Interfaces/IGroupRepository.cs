using Models.Entities.Timetables;

namespace Repository.Interfaces;

public interface IGroupRepository
{
    Task<Group?> GetGroup(int id);
    Task<List<Group>?> GetGroupList();
    Task<List<Group>?> GetGroupList(Predicate<Group> predicate);
    Task<bool> InsertGroup(Group group);
    Task<bool> DeleteGroup(int id);
    Task<bool> UpdateGroup(Group group);
}
