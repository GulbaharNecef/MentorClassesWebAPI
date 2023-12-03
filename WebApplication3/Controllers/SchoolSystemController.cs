using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.SchoolDTO;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchoolSystemController : Controller
    {
        public readonly ISchoolService _schoolService;
        public SchoolSystemController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllSchools()
        {
            var data = await _schoolService.GetAllSchool();
            return StatusCode(data.StatusCode, data);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateSchool(SchoolUpdateDTO schoolUpdateDTO)
        {
            var data = await _schoolService.UpdateSchool(schoolUpdateDTO);
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetSchoolById([FromQuery]int id)
        {
            var data = await _schoolService.GetSchoolById(id);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateSchool([FromBody]SchoolCreateDTO schoolCreateDTO)
        {
            var data = await _schoolService.CreateSchool(schoolCreateDTO);
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteSchool(int id)
        {
            var data = await _schoolService.DeleteSchool(id);
            return StatusCode(data.StatusCode, data);
        }
    }
}
