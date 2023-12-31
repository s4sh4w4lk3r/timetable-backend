﻿using Core.Entities.Identity;

namespace WebApi.Services.Identity.Interfaces;

public interface IUserSessionService
{
    IQueryable<UserSession> UserSessions { get; }
    Task<ServiceResult> AddAsync(UserSession userSession, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdateAsync(UserSession userSession, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int userSessionId, string refreshToken, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAllAsync(int userId, CancellationToken cancellationToken = default);
}
