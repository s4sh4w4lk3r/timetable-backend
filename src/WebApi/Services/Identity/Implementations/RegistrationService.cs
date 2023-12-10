using Microsoft.EntityFrameworkCore;
using Core.Entities.Identity;
using Core.Entities.Identity.Users;
using Core.Entities.Timetables;
using Repository;
using Validation;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class RegistrationService : IRegistrationService
{
    private readonly TimetableContext _dbContext;
    private readonly DbSet<User> _users;
    private readonly IApprovalService _approvalService;
    private readonly IApprovalSender _approvalSender;
    private readonly ILogger _logger;

    public RegistrationService(TimetableContext dbContext, IApprovalService approvalService, IApprovalSender approvalSender, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _users = _dbContext.Set<User>();
        _approvalService = approvalService;
        _approvalSender = approvalSender;
        _logger = loggerFactory.CreateLogger<RegistrationService>();
    }
    public async Task<ServiceResult> AddUserToRepoAsync(User user, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(user.Email) is false)
        {
            return ServiceResult.Fail("Email имеет неверный формат.");
        }

        if (StaticValidator.ValidatePassword(user.Password) is false)
        {
            return ServiceResult.Fail("Пароль не соответствует минимальным требованиям безопасности.");
        }

        if (await _users.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == true, cancellationToken) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд.");
        }

        if (await _users.AnyAsync(x => x.Email == user.Email && x.IsEmailConfirmed == false, cancellationToken) is true)
        {
            return new ServiceResult(false, "Пользователь с таким Email уже есть в бд, но Email не подтвержден.");
        }

        if (user is Student student)
        {
            bool isGroupExist = await _dbContext.Set<Group>().AnyAsync(e => e.GroupId == student.GroupId, cancellationToken);
            if (isGroupExist is false)
            {
                return ServiceResult.Fail("Группы с таким id не существует");
            }

            student.Password = PasswordService.HashPassword(student.Password);
            await _dbContext.Set<Student>().AddAsync(student, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Студент добавлен в базу, но имеет не подтвержденный Email. Запросите отправку email.");
        }
        else if (user is Teacher teacher)
        {
            teacher.Password = PasswordService.HashPassword(teacher.Password);
            await _dbContext.Set<Teacher>().AddAsync(teacher, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Учитель добавлен в базу, но имеет не подтвержденный Email. Запросите отправку email.");
        }
        else if (user is Admin admin)
        {
            admin.Password = PasswordService.HashPassword(admin.Password);
            await _dbContext.Set<Admin>().AddAsync(admin, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Админ добавлен в базу, но имеет не подтвержденный Email. Запросите отправку email.");
        }

        string errorMsg = "Была произведена попытка зарегать юзера неизветсного типа.";
        _logger.LogCritical("Класс: {class}, Метод: {method}, {msg}", nameof(RegistrationService), nameof(AddUserToRepoAsync), errorMsg);
        return ServiceResult.Fail(errorMsg);
    }
    public async Task<ServiceResult> ConfirmAsync(string userEmail, int approvalCode, CancellationToken cancellationToken = default)
    {
        if (approvalCode == default)
        {
            return new ServiceResult(false, "Некорректный approvalCode пользователя.");
        }

        var emailOk = StaticValidator.ValidateEmail(userEmail);
        if (emailOk is false)
        {
            return new ServiceResult(false, "Email имеет неправильный формат.");
        }

        var validUser = await _users.Where(e => e.Email == userEmail).SingleOrDefaultAsync(cancellationToken);
        if (validUser is null)
        {
            return new ServiceResult(false, "Пользователь для валидации не был найден в бд.");
        }

        var approvalServiceResult = await _approvalService.VerifyAndRevokeCodeAsync(validUser.UserId, approvalCode, Approval.ApprovalCodeType.Registration, cancellationToken: cancellationToken);
        if (approvalServiceResult.Success is false)
        {
            return new ServiceResult(false, "Код подтверждения регистрации не принят.", approvalServiceResult);
        }

        validUser.IsEmailConfirmed = true;
        _dbContext.Set<User>().Update(validUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Email пользователя подтвержден.");
    }
    public async Task<ServiceResult> SendEmailAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var emailOk = StaticValidator.ValidateEmail(userEmail);
        if (emailOk is false)
        {
            return new ServiceResult(false, "Email имеет неправильный формат.");
        }

        var sendApprovalResult = await _approvalSender.SendRegistrationCodeAsync(userEmail, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return ServiceResult.Fail("Письмо подтверждения регистрации не было отправлено.");
        }

        return ServiceResult.Ok(sendApprovalResult.Description);
    }
}
