using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Entities.Identity;
using WebApi.Services.Identity.Interfaces;
using WebApi.Types.Configuration;

namespace WebApi.Controllers.Identity
{
    [ApiController, Route("bigdaddy")]
    public class BigDaddyController : ControllerBase
    {
        private readonly string _bigDaddyKey;
        private readonly IRegistrationEntityService _registrationEntityService;

        public BigDaddyController(IRegistrationEntityService registrationEntityService, IOptions<ApiSettings> options)
        {
            _bigDaddyKey = options.Value.BigDaddyKey;
            _registrationEntityService = registrationEntityService;
        }

        [HttpPost, Route("create-admin-register-code")]
        public async Task<IActionResult> CreateAdminRegisterCode([FromQuery] string bigDaddyKey, CancellationToken cancellationToken = default)
        {
            const int ONLY_ONE_CODE = 1;

            if (bigDaddyKey != _bigDaddyKey)
            {
                return BadRequest("Ключ папочки неверный.");
            }

            var serviceResult = await _registrationEntityService.CreateAndSaveRegistrationEntitesAsync(RegistrationEntity.Role.Admin, ONLY_ONE_CODE, default, cancellationToken);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }
            return Ok(serviceResult);
        }
    }
}
