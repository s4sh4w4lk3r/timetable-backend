using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Users;
using Models.Validation;
using WebApi.Extensions;
using WebApi.Services.Account.Implementations;

namespace WebApi.Controllers.Account;

[ApiController, Route("api/account")]
public class UpdateController : Controller
{
    private readonly UserService _userService;

    public UpdateController(UserService userService)
    {
        _userService = userService;
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

        var updatePasswordResult = await _userService.UpdatePassword(userId, user.Password!, cancellationToken);
        if (updatePasswordResult.Success is false)
        {
            return BadRequest(updatePasswordResult);
        }

        return Ok(updatePasswordResult);
    }

    [HttpPost, Route("update/email"), Authorize]
    public async Task<IActionResult> UpdateEmail([FromQuery] string newEmail)
    {
        /*if (StaticValidator.ValidateEmail(newEmail) is false)
        {
            return BadRequest("Неверный формат почты.");
        }

        if (HttpContext.User.TryGetIdFromClaimPrincipal(out int userId) is false)
        {
            return BadRequest("Не получилось получить id из клеймов.");
        }

        _userService.UpdateEmail*/
        throw new NotImplementedException();
#warning надо делать еще и подтверждение новой почты
#warning добавил сущность в контекст, осталось реализовать.
#warning возможно надо сделать чтобы при разворачивании приложения была 1 учетка админа, а он уже вручную регистрирует других админов. Добавить enum с большим админом, который может удалять маленьких админов.
    }
}
