﻿using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity.Users;
using Repository;
using Validation;

namespace WebApi.Services.Account.Implementations;

public class PasswordService
{
    private readonly TimetableContext _dbContext;
    private readonly DbSet<User> _users;

    public PasswordService(TimetableContext dbContext)
    {
        _dbContext = dbContext;
        _users = _dbContext.Set<User>();
    }
    public async Task<ServiceResult> UpdatePassword(int userId, string newPassword, CancellationToken cancellationToken = default)
    {
        if (userId == default)
        {
            return ServiceResult.Fail("Id пользователя не должен быть равен нулю.");
        }


        if (StaticValidator.ValidatePassword(newPassword) is false)
        {
            return new ServiceResult(false, "В пароле должно быть не менее 8 символов, большие и маленькие латинские буквы, цифры и спецсимволы #?!@$%^&*-");
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь с таким Id не найден.");
        }

        userFromRepo.Password = HashPassword(newPassword);
        _users.Update(userFromRepo);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Пароль пользователя обновлен.");
    }
    public async Task<ServiceResult<User>> CheckLoginDataAsync(User user, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(user.Email) is false)
        {
            return ServiceResult<User>.Fail("Email имеет неверный формат.", null);
        }

        if (StaticValidator.ValidatePassword(user.Password) is false)
        {
            return ServiceResult<User>.Fail("Пароль не соответствует минимальным требованиям безопасности.", null);
        }

        var userFromRepo = await _users.SingleOrDefaultAsync(e => e.Email == user.Email, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult<User>(false, "Пользователя нет в бд.", null);
        }

        if (userFromRepo.IsEmailConfirmed is false)
        {
            return new ServiceResult<User>(false, "Пользователь найден, но Email не подтвержден.", null);
        }

        string? hashFromRepo = userFromRepo.Password;
        if (string.IsNullOrWhiteSpace(hashFromRepo))
        {
            return new ServiceResult<User>(false, "Пользователь найден, но его пароль почему-то пуст.", null);
        }

        if (ValidatePassword(hashFromRepo, user.Password!) is true)
        {
            return new ServiceResult<User>(true, "Пароль подтвержден.", userFromRepo);
        }
        else
        {
            return new ServiceResult<User>(false, "Пароль неверный.", null);
        }
    }
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    public static bool ValidatePassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

}
