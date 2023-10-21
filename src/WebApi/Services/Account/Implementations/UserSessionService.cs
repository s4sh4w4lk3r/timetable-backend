using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;

namespace WebApi.Services.Account.Implementations;

public class UserSessionService
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserSession> _userSessions;
    private readonly IValidator<UserSession> _userSessionValidator;

    public IQueryable<UserSession> UserSessions => _userSessions.AsQueryable();

    public UserSessionService(DbContext dbContext, IValidator<UserSession> validator)
    {
        _dbContext = dbContext;
        _userSessions = _dbContext.Set<UserSession>();
        _userSessionValidator = validator;
    }


    public async Task<ServiceResult> AddUserSessionAsync(UserSession userSession, CancellationToken cancellationToken = default)
    {
        var validationResult = _userSessionValidator.Validate(userSession);
        if (validationResult.IsValid is false)
        {
            return new ServiceResult(false, validationResult.ToString());
        }

        await _userSessions.AddAsync(userSession, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя добавлена в бд.");
    }

    public async Task<ServiceResult> UpdateUserSessionAsync(UserSession userSession, CancellationToken cancellationToken = default)
    {
        var validationResult = _userSessionValidator.Validate(userSession);
        if (validationResult.IsValid is false)
        {
            return new ServiceResult(false, validationResult.ToString());
        }

        _userSessions.Update(userSession);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя обновлена в бд.");
    }

    public async Task<ServiceResult> DeleteSessionAsync(int userSessionId, string refreshToken, CancellationToken cancellationToken = default)
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

    public async Task<ServiceResult> RevokeAllAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не может быть равным нулю");
        }

        await _userSessions.Where(e => e.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        return ServiceResult.Ok("Все сессии пользователя удалены.");
    }
}
