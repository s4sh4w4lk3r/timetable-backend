using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation;

namespace WebApi.Services.Account.Implementations;

public class EmailService
{
    private readonly DbContext _dbContext;
    private readonly IValidator<User> _userValidator;
    private readonly DbSet<User> _users;
    
    public EmailService(DbContext dbContext, IValidator<User> validator)
    {
        _dbContext = dbContext;
        _userValidator = validator;
        _users = _dbContext.Set<User>();
    }

    public async Task<ServiceResult> UpdateEmail(User user, int approvalCode, ApprovalService approvalService, CancellationToken cancellationToken = default)
    {
#warning не проверен
        if (user.UserId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        if (StaticValidator.ValidateEmail(user.Email) is false)
        {
            return ServiceResult.Fail("Некорректный формат почты.");
        }

        if (await _users.AnyAsync(e => e.Email == user.Email) is true)
        {
            return ServiceResult.Fail("Пользователь с таким email уже зарегистрирован.");
        }

        if (approvalCode == default)
        {
            return new ServiceResult(false, "ApprovalCode не может быть равен нулю.");
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == user.UserId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        var approvalServiceResult = await approvalService.VerifyCodeAsync(userFromRepo.UserId, approvalCode, ApprovalCode.ApprovalCodeType.UpdateMail, cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения для изменения почты не принят.", approvalServiceResult);
        }

        userFromRepo.Email = user.Email;
        _users.Update(userFromRepo);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Email пользователя обновлен.");
    }
}
