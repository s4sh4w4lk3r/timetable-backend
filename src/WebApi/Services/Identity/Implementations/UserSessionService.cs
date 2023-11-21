using Microsoft.EntityFrameworkCore;
using Core.Entities.Identity;
using Repository;
using Validation;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class UserSessionService : IUserSessionService
{
    private readonly TimetableContext _dbContext;
    private readonly DbSet<UserSession> _userSessions;
    public IQueryable<UserSession> UserSessions => _userSessions.AsQueryable();

    public UserSessionService(TimetableContext dbContext)
    {
        _dbContext = dbContext;
        _userSessions = _dbContext.Set<UserSession>();
    }


    public async Task<ServiceResult> AddAsync(UserSession userSession, CancellationToken cancellationToken = default)
    {
        var validationResult = new UserSessionValidator().Validate(userSession);
        if (validationResult.IsValid is false)
        {
            return new ServiceResult(false, validationResult.ToString());
        }

        await _userSessions.AddAsync(userSession, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя добавлена в бд.");
    }

    public async Task<ServiceResult> UpdateAsync(UserSession userSession, CancellationToken cancellationToken = default)
    {
        var validationResult = new UserSessionValidator().Validate(userSession);
        if (validationResult.IsValid is false)
        {
            return new ServiceResult(false, validationResult.ToString());
        }

        _userSessions.Update(userSession);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя обновлена в бд.");
    }

    public async Task<ServiceResult> DeleteAsync(int userSessionId, string refreshToken, CancellationToken cancellationToken = default)
    {
        if (userSessionId == default)
        {
            return new ServiceResult(false, "Сессия не удалена, id не может быть равен нулю.");
        }

        var validUserSession = await _userSessions.Where(e => e.User!.UserId == userSessionId && e.RefreshToken == refreshToken).ExecuteDeleteAsync(cancellationToken);

        if (validUserSession == 0)
        {
            return new ServiceResult(false, "Сессия не найдена в бд для удаления.");
        }

        return new ServiceResult(true, "Сессия пользователя удалена из бд.");
    }

    public async Task<ServiceResult> DeleteAllAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не может быть равным нулю");
        }

        await _userSessions.Where(e => e.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        return ServiceResult.Ok("Все сессии пользователя удалены.");
    }
}