using Models.Entities.Timetables;
using Repository;

namespace WebApi.Services.Timetables;

public class GroupService
{
    private readonly SqlDbContext _dbContext;

    public GroupService(SqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ServiceResult<List<Group>?> GetGroupList()
    {
#warning проверитью
        var groups = _dbContext.Set<Group>().ToList();

        if (groups is null)
        {
            return ServiceResult<List<Group>?>.Fail("Список групп не получен.", null);
        }

        if (groups.Count == 0)
        {
            return ServiceResult<List<Group>?>.Fail("Список групп оказался пуст.", null);
        }

        return ServiceResult<List<Group>?>.Ok("Список групп получен из бд.", groups);
    }
}
