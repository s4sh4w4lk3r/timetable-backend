﻿using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Repository;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Services.Account.Implementations
{
    public class UnregistrationService : IUnregistrationService
    {
        private readonly SqlDbContext _dbContext;
        private readonly DbSet<User> _users;
        private readonly ApprovalService _approvalService;

        public UnregistrationService(SqlDbContext dbContext, ApprovalService approvalService)
        {
            _dbContext = dbContext;
            _users = _dbContext.Set<User>();
            _approvalService = approvalService;
        }

        public async Task<ServiceResult> ConfirmAsync(int userId, int approvalCode, CancellationToken cancellationToken = default)
        {
            if (userId == default)
            {
                return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
            }

            var validUser = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
            if (validUser is null)
            {
                return new ServiceResult(false, "Пользователь не найден в бд.");
            }

            var approvalServiceResult = await _approvalService.VerifyCodeAsync(validUser.UserId, approvalCode, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken: cancellationToken);
            if (approvalServiceResult.Success is false)
            {
                return new ServiceResult(false, "Код подтверждения для удаления аккаунта не принят.", approvalServiceResult);
            }

            _users.Remove(validUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResult(true, "Аккаунт пользователя удален.");
        }

        public async Task<ServiceResult> SendEmailAsync(int userId, CancellationToken cancellationToken = default)
        {
            string? userEmail = await _users.Where(e => e.UserId == userId).Select(e => e.Email).FirstOrDefaultAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return ServiceResult.Fail("Пользователь не найден в бд.");
            }

            var sendCodeResult = await _approvalService.SendCodeAsync(userEmail, ApprovalCode.ApprovalCodeType.Unregistration, cancellationToken);
            if (sendCodeResult.Success is false)
            {
                return ServiceResult.Fail("Код подтверждения удаления аккаунта не был отправлен.", sendCodeResult);
            }

            return ServiceResult.Ok("Код подтверждения удаления аккаунта был успешно отправлен.");
        }
    }
}