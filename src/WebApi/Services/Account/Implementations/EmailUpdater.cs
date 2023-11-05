using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Validation;
using Repository;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations;

public class EmailUpdater
{
    private readonly SqlDbContext _dbContext;
    private readonly DbSet<User> _users;
    private readonly IApprovalService _approvalService;
    private readonly IApprovalSender _approvalSender;


    public EmailUpdater(SqlDbContext dbContext, IApprovalService approvalService, IApprovalSender approvalSender)
    {
        _dbContext = dbContext;
        _users = _dbContext.Set<User>();
        _approvalService = approvalService;
        _approvalSender = approvalSender;
    }

    public async Task<ServiceResult> UpdateEmailAsync(int userId, int approvalCode, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        if (approvalCode == default)
        {
            return new ServiceResult(false, "ApprovalCode не может быть равен нулю.");
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        var approvalServiceResult = await _approvalService.VerifyAndRevokeCodeAsync(userFromRepo.UserId, approvalCode, ApprovalCode.ApprovalCodeType.UpdateMail, deleteRequired: false, cancellationToken: cancellationToken);
        if ((approvalServiceResult.Success is false) || (approvalServiceResult.Value is null))
        {
            return new ServiceResult(false, "Код подтверждения для изменения почты не принят.", approvalServiceResult);
        }

        string? newEmail = await _dbContext.Set<EmailUpdateEntity>().Where(e => e.UserId == userId && e.ApprovalId == approvalServiceResult.Value.AprrovalCodeId)
            .Select(e => e.NewEmail).SingleOrDefaultAsync(cancellationToken);

        if (StaticValidator.ValidateEmail(newEmail) is false)
        {
            throw new InvalidOperationException("В таблицу EmailUpdateEntities попали невалидные значения. Параметр newEmail не прошел валидацию.");
        }

        userFromRepo.Email = newEmail;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok("Email адрес обновлен.");
    }

    public async Task<ServiceResult> SendUpdateMailAsync(int userId, string newEmail, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }

        if (StaticValidator.ValidateEmail(newEmail) is false)
        {
            return ServiceResult.Fail("Некорректный формат почты.");
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь не найден в бд.");
        }

        if (await _users.AnyAsync(e => e.Email == newEmail, cancellationToken: cancellationToken) is true)
        {
            return ServiceResult.Fail("Пользователь с таким email уже зарегистрирован.");
        }

        var approvalResult = await _approvalSender.SendEmailUpdateCodeAsync(userId, newEmail, cancellationToken);
        if ((approvalResult.Success is false) || (approvalResult.Value is null))
        {
            return ServiceResult.Fail("Письмо на почту для подтверждения нового Email не было отправлено.", approvalResult);
        }


        var emailUpdateInstance = new EmailUpdateEntity()
        {
            UserId = userFromRepo.UserId,
            OldEmail = userFromRepo.Email,
            NewEmail = newEmail,
            ApprovalId = approvalResult.Value.AprrovalCodeId
        };

        _dbContext.Set<EmailUpdateEntity>().Add(emailUpdateInstance);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok("Письмо для подтверждения нового Email было отправлено успешно.");
    }
}
