using Microsoft.AspNetCore.Mvc;
using WebApplication3.Entities;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

       [HttpGet]
       public async Task<IActionResult> GetStudentId(int id)
        {
            var data = await _studentService.GetStudentId(id);
            return  Ok(data);
        }

        
    }
}
