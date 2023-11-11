using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Identity.Users;
using Validation;
using WebApi.Extensions;
using WebApi.Services.Identity.Implementations;

namespace WebApi.Controllers.Account;

[ApiController, Route("api/account")]
public class UpdateController : Controller
{
    private readonly EmailUpdater _emailService;
    private readonly PasswordService _passwordService;

    public UpdateController(EmailUpdater emailService, PasswordService passwordService)
    {
        _emailService = emailService;
        _passwordService = passwordService;
    }

    [HttpPost, Route("update/password"), Authorize]
    public async Task<IActionResult> UpdatePassword([Bind("Password")] User user, CancellationToken cancellationToken)
    {
        if (StaticValidator.ValidatePassword(user.Password) is false)
        {
            return BadRequest("Пароль не проходит проверку.");
        }

        if ((HttpContext.User.TryGetUserIdFromClaimPrincipal(out int userId) is false) || userId == default)
        {
            return BadRequest("Не получилось получить id из клеймов.");
        }

        var updatePasswordResult = await _passwordService.UpdatePassword(userId, user.Password!, cancellationToken);
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

}
#warning возможно надо сделать чтобы при разворачивании приложения была 1 учетка админа, а он уже вручную регистрирует других админов. Добавить enum с большим админом, который может удалять маленьких админов. Или может сделать приватный ендпоинт для регистрации админов.