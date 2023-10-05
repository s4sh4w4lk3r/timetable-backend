using FluentValidation;
using Models.Entities.Timetables;
using Models.Validation.AllProperties;
using Repository.Implementations.MySql;
using Repository.Interfaces;
using Throw;

namespace Repository.Implementations.EFCore;

public class GroupRepository : IGroupRepository
{
    private readonly MySqlDbContext _context;
    private readonly CancellationToken _cancellationToken;

    public GroupRepository(MySqlDbContext context, CancellationToken cancellationToken = default)
    {
        _context = context;
        _cancellationToken = cancellationToken;
    }

    public IQueryable<Group> Groups => _context.Groups.AsQueryable();

    public async Task DeleteGroupAsync(int id)
    {
        id.Throw().IfDefault();
        var entityToDel = _context.Groups.FirstOrDefault(a => a.GroupId == id);
        entityToDel.ThrowIfNull();

        _context.Groups.Remove(entityToDel);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task InsertGroupAsync(Group group)
    {
        new GroupValidator().ValidateAndThrow(group);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync(_cancellationToken);
    }

    public async Task UpdateGroupAsync(Group group)
    {
        new GroupValidator().ValidateAndThrow(group);

        var entityEntry = _context.Groups.Entry(group);
        _context.Groups.Update(entityEntry.Entity);
        await _context.SaveChangesAsync(_cancellationToken);
    }
}
