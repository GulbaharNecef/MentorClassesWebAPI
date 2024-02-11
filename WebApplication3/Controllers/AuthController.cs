using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("controller")]
    public class AuthController : ControllerBase
    {
        private IAuthoService _authoService;
        public AuthController(IAuthoService authoService)
        {
            _authoService = authoService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync(string usernameOrEmail, string password)
        {
            var data = await _authoService.LoginAsync(usernameOrEmail, password);
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("GetRefreshToken")]
        public async Task<IActionResult> GetRefreshTokenAsync(string refreshToken)
        {
            var data = await _authoService.LoginWithRefreshTokenAsync(refreshToken);
            return StatusCode(data.StatusCode, data);
        }

    }
}
