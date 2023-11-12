using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using Validation;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Controllers.Identity;

[ApiController, Route("identity/register")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registerService;

    public RegistrationController(IRegistrationService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost, Route("")]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationDto userRegistrationDto, CancellationToken cancellationToken = default)
    {
#warning сделать ендпоинты для регистрации разных типов юезров.
        if (StaticValidator.ValidateEmail(userRegistrationDto.Email) is false)
        {
            return BadRequest("Неверный формат почты.");
        }

        if (StaticValidator.ValidatePassword(userRegistrationDto.Password) is false)
        {
            return BadRequest("Пароль не соответствует тербованиям.");
        }

        Admin user = new() { Email = userRegistrationDto.Email, Password = userRegistrationDto.Password, Firstname = "d", Lastname = "d", Middlename = "d" };
        var regResult = await _registerService.AddUserToRepoAsync(user, cancellationToken);
        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

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

    [HttpGet, Route("create-register-links"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRegisterCodes([FromQuery] RegistrationEntity.Role role, [FromQuery] int numberOfLinks = 1)
    {
        var serviceResult = await _registerService.CreateAndSaveRegisterCodes(role, numberOfLinks);
        if (serviceResult.Success is false)
        {
            return BadRequest(serviceResult);
        }
        return Ok(serviceResult);
    }

    public record ConfirmEmailDto(string Email, int ApprovalCode);
    public record EmailAddressDto(string Email);
    public record UserRegistrationDto(string Email, string Password);
}