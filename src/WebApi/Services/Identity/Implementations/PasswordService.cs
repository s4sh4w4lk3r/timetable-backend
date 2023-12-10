using Microsoft.EntityFrameworkCore;
using Core.Entities.Identity.Users;
using Repository;
using Validation;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Services.Identity.Implementations;

public class PasswordService : IPasswordService
{
    private readonly TimetableContext _dbContext;

    public PasswordService(TimetableContext dbContext)
    {
        _dbContext = dbContext;
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

        var userFromRepo = await _dbContext.Set<User>().SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (userFromRepo is null)
        {
            return new ServiceResult(false, "Пользователь с таким Id не найден.");
        }

        userFromRepo.Password = HashPassword(newPassword);
        _dbContext.Set<User>().Update(userFromRepo);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResult(true, "Пароль пользователя обновлен.");
    }
    public async Task<ServiceResult<User>> CheckLoginDataAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(email) is false)
        {
            return ServiceResult<User>.Fail("Email имеет неверный формат.", null);
        }

        if (StaticValidator.ValidatePassword(password) is false)
        {
            return ServiceResult<User>.Fail("Пароль не соответствует минимальным требованиям безопасности.", null);
        }


        // Ищем какому типу User принадлжеит имейл.
        User? userFromRepo = await _dbContext.Set<Student>().SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
        if (userFromRepo is null)
        {
            userFromRepo = await _dbContext.Set<Teacher>().SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
            if (userFromRepo is null)
            {
                userFromRepo = await _dbContext.Set<Admin>().SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
            }
        }


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

        if (ValidatePassword(hashFromRepo, password!) is true)
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
