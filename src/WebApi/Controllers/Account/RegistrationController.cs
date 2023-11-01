using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using Models.Validation;
using WebApi.Extensions;
using WebApi.Services.Account.Interfaces;

namespace WebApi.Controllers.Auth;

[ApiController, Route("api/account")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registerService;
    private readonly IUnregistrationService _unregistrationService;

    public RegistrationController(IRegistrationService registerService, IUnregistrationService unregistrationService)
    {
        _registerService = registerService;
        _unregistrationService = unregistrationService;
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto, CancellationToken cancellationToken = default)
    {
#warning проверить
        if (StaticValidator.ValidateEmail(userRegistrationDto.Email) is false)
        {
            return BadRequest(userRegistrationDto.Email);
        }

        if (StaticValidator.ValidateEmail(userRegistrationDto.Password) is false)
        {
            return BadRequest(userRegistrationDto.Email);
        }

        Models.Entities.Users.User user = new() { Email = userRegistrationDto.Email, Password = userRegistrationDto.Password };
        var regResult = await _registerService.AddUserToRepoAsync(user, cancellationToken);
        if (regResult.Success is false)
        {
            return BadRequest(regResult);
        }

        return Ok(regResult);
    }


    [HttpPost, Route("register/confirm")]
    public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDto confirmEmailDto, CancellationToken cancellationToken = default)
    {
#warning проверить
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


    [HttpPost, Route("register/send-email")]
    public async Task<IActionResult> SendRegisterEmail([FromBody] EmailAddressDto emailAddressDto, CancellationToken cancellationToken = default)
    {
#warning проверить
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


    [HttpGet, Route("unregister/send-email"), Authorize]
    public async Task<IActionResult> UnregisterSendMail(CancellationToken cancellationToken = default)
    {
        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось считать id из клеймов.");
        }

        var requestUnreg = await _unregistrationService.SendEmailAsync(userId, cancellationToken);
        if (requestUnreg.Success is false)
        {
            return BadRequest(requestUnreg);
        }

        return Ok(requestUnreg);
    }


    [HttpPost, Route("unregister/confirm"), Authorize]
    public async Task<IActionResult> ConfirmUnregistration(int approvalCode, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось считать id из клеймов.");
        }

        if (userId == default)
        {
            return BadRequest("Id пользователя не может быть равным нулю.");
        }

        var unregisterResult = await _unregistrationService.ConfirmAsync(userId, approvalCode, cancellationToken);
        if (unregisterResult.Success is false)
        {
            return BadRequest(unregisterResult);
        }

        return Ok(unregisterResult);
    }

    public record ConfirmEmailDto(string Email, int ApprovalCode);
    public record EmailAddressDto(string Email);
    public record UserRegistrationDto(string Email, string Password);
}