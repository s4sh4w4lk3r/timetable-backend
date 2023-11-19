using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Validation;
using WebApi.Extensions;
using WebApi.Services.Identity.Interfaces;

namespace WebApi.Controllers.Identity;

[ApiController, Route("identity")]
public class UpdateController : Controller
{
    private readonly IEmailUpdater _emailService;
    private readonly IPasswordService _passwordService;

    public UpdateController(IEmailUpdater emailService, IPasswordService passwordService)
    {
        _emailService = emailService;
        _passwordService = passwordService;
    }

    [HttpPost, Route("update/password"), Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] PasswordDto passwordDto, CancellationToken cancellationToken)
    {
        if (StaticValidator.ValidatePassword(passwordDto.Password) is false)
        {
            return BadRequest("Пароль не проходит проверку.");
        }

        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false || userId == default)
        {
            return BadRequest("Не получилось получить id из клеймов.");
        }

        var updatePasswordResult = await _passwordService.UpdatePassword(userId, passwordDto.Password, cancellationToken);
        if (updatePasswordResult.Success is false)
        {
            return BadRequest(updatePasswordResult);
        }

        return Ok(updatePasswordResult);
    }

    [HttpPost, Route("update/email"), Authorize]
    public async Task<IActionResult> UpdateEmail([FromQuery] string newEmail, CancellationToken cancellationToken = default)
    {
        if (StaticValidator.ValidateEmail(newEmail) is false)
        {
            return BadRequest("Неверный формат почты.");
        }

        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось получить id из клеймов.");
        }

        var serviceResult = await _emailService.SendUpdateMailAsync(userId, newEmail, cancellationToken);
        if (serviceResult.Success is false)
        {
            return BadRequest(serviceResult);
        }

        return Ok(serviceResult);
    }

    [HttpPost, Route("update/email/confirm"), Authorize]
    public async Task<IActionResult> ConfirmUpdateEmail([FromQuery] int approvalCode, CancellationToken cancellationToken = default)
    {
        if (HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось получить id из клеймов.");
        }

        if (approvalCode == default)
        {
            return BadRequest("Approval не может быть равен нулю.");
        }

        var serviceResult = await _emailService.UpdateEmailAsync(userId, approvalCode, cancellationToken);
        if (serviceResult.Success is false)
        {
            return BadRequest(serviceResult);
        }

        return Ok(serviceResult);
    }

    public record class PasswordDto(string Password);
}