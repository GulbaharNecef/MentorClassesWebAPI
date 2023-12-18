using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController:ControllerBase
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _roleService.GetAllRoles();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoles(string id)
        {
            var result = await _roleService.GetRoleById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateRole(string name)
        {
            var result = await _roleService.CreateRole(name);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, string name)
        {
            var result = await _roleService.UpdateRole(id, name);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRole(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
