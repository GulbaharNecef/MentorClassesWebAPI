using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            var data = await _userService.CreateAsync(model);
            return StatusCode(data.StatusCode, data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _userService.GetAllUsersAsync();
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("get-roles-to-user/{UserIdOrName}")]
        public async Task<IActionResult> GetUsersByRole(string UserIdOrName)
        {
            var roles = await _userService.GetRolesToUserAsync(UserIdOrName);
            return StatusCode(roles.StatusCode, roles);
        }

        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoles(string userId, string[] roles)
        {
            var data = await _userService.AssignRoleToUserAsnyc(userId, roles);
            return StatusCode(data.StatusCode, data);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userIdOrName)
        {
            var data = await _userService.DeleteUserAsync(userIdOrName);
            return StatusCode(data.StatusCode, data);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO model)
        {
            var data = await _userService.UpdateUserAsync(model);
            return StatusCode(data.StatusCode, data);
        }
    }
}
