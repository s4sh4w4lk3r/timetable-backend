using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Users;
using Models.Validation.ConcreteValidationRules;
using Repository.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class AdministratorService
{
    private readonly IRepository<Administrator> _repository;

    public AdministratorService(IRepository<Administrator> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Register(Administrator administrator, CancellationToken cancellationToken = default)
    {
        if ((await new UserRegexValidator().ValidateAsync(administrator, cancellationToken)).IsValid is false)
        {
            return false;
        }

        if ((await _repository.Entites.AnyAsync(x => x.Email == administrator.Email, cancellationToken)) is true)
        {
            return false;
        }

        administrator.Password = HashPassword(administrator.Password!);
        await _repository.InsertAsync(administrator, cancellationToken);
        return true;
#warning сделай регистрацию с подтверждением
    }

    public async Task<bool> Unregister(Administrator administrator, string approval, CancellationToken cancellationToken = default)
    {
        /*await new AdministratorBusinessValidator().ValidateAndThrowAsync(administrator, cancellationToken);

        if (string.IsNullOrWhiteSpace(approval) || VerifyApprovalCode(administrator) is false)
        {
            
        }*/
#warning сделай тут
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Administrator administrator, string approval, CancellationToken cancellationToken = default)
    {
#warning сделай тут
        throw new NotImplementedException();
    }

    public async Task<bool> LoginAsync(Administrator administrator, CancellationToken cancellationToken = default)
    {
        await new UserRegexValidator().ValidateAndThrowAsync(administrator, cancellationToken);
        var hashFromRepo = await _repository.Entites.Where(e => e.Email == administrator.Email).Select(e => e.Password).FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(hashFromRepo))
        {
            return false;
        }

        return ValidatePassword(hashFromRepo, administrator.Password!);
    }

    public static void SendApprovalCode(Administrator administrator, IEmailClient emailClient, ApprovalCodeType approvalCodeType)
    {
#warning сделай тут
        throw new NotImplementedException();
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    private static bool ValidatePassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

    private Task<bool> VerifyApprovalCode(Administrator administrator)
    {
#warning сделай тут
        throw new NotImplementedException();
    }
}

public enum ApprovalCodeType { Registration, Unregistration, UpdatePassword}
