using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using Models.Entities.Timetables.Cells.CellMembers;
using Validation;
using WebApi.Services;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Controllers.Identity;

[ApiController, Route("identity/registration")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registerService;
    private readonly IRegistrationEntityService _registrationEntityService;
    private readonly ILogger _logger;

    public RegistrationController(IRegistrationService registerService, IRegistrationEntityService registrationEntityService, ILoggerFactory loggerFactory)
    {
        _registerService = registerService;
        _registrationEntityService = registrationEntityService;
        _logger = loggerFactory.CreateLogger<RegistrationController>();
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto, CancellationToken cancellationToken = default)
    {
        var validateResult = new UserRegistrationDtoValidator().Validate(userRegistrationDto);
        if (validateResult.IsValid is false)
        {
            return BadRequest(validateResult);
        }

        var regEntity = await _registrationEntityService.CheckRegistrationEntityExistsAsync(userRegistrationDto.RegisterKey, userRegistrationDto.Role, cancellationToken);
        if (regEntity.Success is false || regEntity.Value is null)
        {
            return BadRequest(regEntity);
        }

        ServiceResult? regResult;

        switch (userRegistrationDto.Role)
        {
            case RegistrationEntity.Role.Student 
                when regEntity.Value.StudentGroupId is int studentGroupId 
                && regEntity.Value.SubGroup is SubGroup subGroup && Enum.IsDefined(subGroup):
                {
                    Student student = new()
                    {
                        Firstname = userRegistrationDto.Firstname, Lastname = userRegistrationDto.Lastname, Middlename = userRegistrationDto.Middlename,
                        Email = userRegistrationDto.Email, Password = userRegistrationDto.Password, GroupId = studentGroupId, SubGroup = subGroup
                    };
                    regResult = await _registerService.AddUserToRepoAsync(student, cancellationToken);
                    break;
                }

            case RegistrationEntity.Role.Teacher:
                {
                    Teacher teacher = new()
                    {
                        Firstname = userRegistrationDto.Firstname, Lastname = userRegistrationDto.Lastname, Middlename = userRegistrationDto.Middlename,
                        Email = userRegistrationDto.Email, Password = userRegistrationDto.Password
                    };
                    regResult = await _registerService.AddUserToRepoAsync(teacher, cancellationToken);
                    break;
                }

            case RegistrationEntity.Role.Admin:
                {
                    Admin admin = new()
                    {
                        Firstname = userRegistrationDto.Firstname, Lastname = userRegistrationDto.Lastname, Middlename = userRegistrationDto.Middlename,
                        Email = userRegistrationDto.Email, Password = userRegistrationDto.Password
                    };
                    regResult = await _registerService.AddUserToRepoAsync(admin, cancellationToken);
                    break;
                }

            default:
                _logger.LogCritical("В контроллере {controller} в методе {method} случился дефолт в enum ролей.", nameof(RegistrationController), nameof(Register));
                return StatusCode(500, "Что-то пошло не так, см. логи");
        }

        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

        await _registrationEntityService.RemoveRegistrationEntityAsync(regEntity.Value, cancellationToken);
        return Ok(regResult);
    }


    [HttpPost, Route("send-email")]
    public async Task<IActionResult> SendRegisterEmail([FromBody] EmailAddressDto emailAddressDto, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(emailAddressDto.Email) is false)
        {
            return BadRequest("Email адрес имеет неверный формат.");
        }

        var sendApprovalResult = await _registerService.SendEmailAsync(emailAddressDto.Email, cancellationToken);
        if (sendApprovalResult.Success is false)
        {
            return BadRequest(sendApprovalResult);
        }

        return Ok("Письмо с кодом подтверждения отправлено на почту.");
    }


    [HttpPost, Route("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(confirmEmailDto.Email) is true)
        {
            return BadRequest("UserEmail не введен.");
        }

        if (confirmEmailDto.ApprovalCode == default)
        {
            return BadRequest("ApprovalCode не введен.");
        }

        var confirmResult = await _registerService.ConfirmAsync(confirmEmailDto.Email, confirmEmailDto.ApprovalCode, cancellationToken);
        if (confirmResult.Success is false)
        {
            return BadRequest(confirmResult);
        }
        return Ok(confirmResult);
    }

    [HttpPost, Route("create-register-codes"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRegisterCodes([FromQuery] RegistrationEntity.Role role, [FromQuery] int numberOfCodes = 1, [FromQuery] int studentGroupId = 0, [FromQuery] SubGroup subGroup = SubGroup.All)
    {
        var serviceResult = await _registrationEntityService.CreateAndSaveRegistrationEntitesAsync(role: role, numberOfCodes: numberOfCodes, studentGroupId: studentGroupId, subGroup: subGroup);
        if (serviceResult.Success is false)
        {
            return BadRequest(serviceResult);
        }
        return Ok(serviceResult);
    }

    public record ConfirmEmailDto(string Email, int ApprovalCode);
    public record EmailAddressDto(string Email);
    public record UserRegistrationDto(string Email, string Password, string Lastname, string Firstname, string Middlename, RegistrationEntity.Role Role, string RegisterKey);

    public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
    {
        public UserRegistrationDtoValidator()
        {
            RuleFor(e => e.Email).Must(StaticValidator.ValidateEmail).WithMessage("Неверный формат почты.");
            RuleFor(e => e.Password).Must(StaticValidator.ValidatePassword).WithMessage("Пароль не соответствует минимальным требованиям безопасности.");
            RuleFor(e => e.Middlename).NotEmpty();
            RuleFor(e => e.Firstname).NotEmpty();
            RuleFor(e => e.Lastname).NotEmpty();
            RuleFor(e => e.RegisterKey).NotEmpty();
            RuleFor(e => e.Role).IsInEnum();
        }
    }
}
