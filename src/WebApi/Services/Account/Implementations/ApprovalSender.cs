using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using Repository;
using Validation;
using WebApi.Services.Account.Interfaces;
using static Models.Entities.Identity.Approval;

namespace WebApi.Services.Account.Implementations
{
    public class ApprovalSender : IApprovalSender
    {
        private readonly TimetableContext _dbContext;
        private readonly IEmailClient _emailClient;

        public ApprovalSender(TimetableContext dbContext, IEmailClient emailClient)
        {
            _dbContext = dbContext;
            _emailClient = emailClient;
        }

        public async Task<ServiceResult> SendUnregistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            if (StaticValidator.ValidateEmail(userEmail) is false)
            {
                return ServiceResult<Approval?>.Fail("Email имеет неверный формат или не указан вовсе.", null);
            }

            var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.Email == userEmail, cancellationToken);
            if (userFromRepo is null)
            {
                return ServiceResult<Approval?>.Fail("Пользователь не был найден в бд.", null);
            }

            var approval = new Approval()
            {
                CodeType = ApprovalCodeType.Unregistration,
                UserId = userFromRepo.UserId
            };
            await _dbContext.Set<Approval>().AddAsync(approval, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            string message = $"Код подтверждения для удаления аккаунта: {approval.Code}.";

            await _emailClient.SendEmailAsync("Подтверждение удаления аккаунта", message, userFromRepo.Email!, cancellationToken);
            return ServiceResult<Approval?>.Ok("Код подтверждения должен будет отправится.", approval);
        }
        public async Task<ServiceResult> SendRegistrationCodeAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            if (StaticValidator.ValidateEmail(userEmail) is false)
            {
                return ServiceResult<Approval?>.Fail("Email имеет неверный формат или не указан вовсе.", null);
            }

            var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.Email == userEmail, cancellationToken);
            if (userFromRepo is null)
            {
                return ServiceResult<Approval?>.Fail("Пользователь не был найден в бд.", null);
            }

            //Нужно только для тех слуаче, если зареганный пользователь пытается подтвердить свою почту.
            if (userFromRepo.IsEmailConfirmed is true)
            {
                return ServiceResult<Approval?>.Fail("У пользователя уже подтверждена почта.", null);
            }

            var approval = new Approval()
            {
                CodeType = ApprovalCodeType.Registration,
                UserId = userFromRepo.UserId
            };
            await _dbContext.Set<Approval>().AddAsync(approval, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            string message = $"Код подтверждения для регистрации: {approval.Code}";

            await _emailClient.SendEmailAsync("Подтверждение адреса электронной почты", message, userFromRepo.Email!, cancellationToken);
            return ServiceResult<Approval?>.Ok("Код подтверждения должен будет отправится.", approval);
        }
        public async Task<ServiceResult<Approval?>> SendEmailUpdateCodeAsync(int userId, string userEmail, CancellationToken cancellationToken = default)
        {

            if (StaticValidator.ValidateEmail(userEmail) is false)
            {
                return ServiceResult<Approval?>.Fail("Email имеет неверный формат или не указан вовсе.", null);
            }

            if (userId == 0)
            {
                return ServiceResult<Approval?>.Fail("Было получен UserId равный нулю", null);
            }

            var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken: cancellationToken);
            if (userFromRepo is null)
            {
                return ServiceResult<Approval?>.Fail("Пользователь не был найден в бд.", null);
            }

            var approval = new Approval()
            {
                CodeType = ApprovalCodeType.UpdateMail,
                UserId = userFromRepo.UserId
            };
            await _dbContext.Set<Approval>().AddAsync(approval, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            string message = $"Код подтверждения для смены адреса почты: {approval.Code}.";

            await _emailClient.SendEmailAsync("Подтверждение адреса электронной почты", message, userEmail, cancellationToken);
            return ServiceResult<Approval?>.Ok("Код подтверждения должен будет отправится.", approval);
        }
    }
}
