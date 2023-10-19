using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users.Auth;

namespace WebApi.Services.Implementations;

public class UserSessionService
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserSession> _userSessions;
    private readonly IValidator<UserSession> _userSessionValidator;

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
#warning не проверен
        var validationResult = _userSessionValidator.Validate(userSession);
        if (validationResult.IsValid is false)
        {
            return new ServiceResult(false, validationResult.ToString());
        }

        _userSessions.Update(userSession);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя обновлена в бд.");
    }

    public async Task<ServiceResult> DeleteSessionAsync(int userSessionId, CancellationToken cancellationToken = default)
    {
#warning не проверен
        if (userSessionId == default)
        {
            return new ServiceResult(false, "Сессия не удалена, id не может быть нулевым.");
        }

        var validUserSession = await _userSessions.FirstOrDefaultAsync(e => e.User!.UserId == userSessionId, cancellationToken);

        if (validUserSession is null)
        {
            return new ServiceResult(false, "Сессия не найдена в бд для удаления..");
        }    

        _userSessions.Remove(validUserSession);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Сессия пользователя удалена из бд.");
    }
}
