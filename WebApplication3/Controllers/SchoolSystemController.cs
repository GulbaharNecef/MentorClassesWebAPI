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
        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<SchoolGetDTO>>>> GetAllSchools()
        {
            try
            {
                var response = await _schoolService.GetAllSchool();
                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseModel<List<SchoolGetDTO>>
                {
                    Data = null,
                    StatusCode = 500,
                });
            }
           

        }
    }
}
