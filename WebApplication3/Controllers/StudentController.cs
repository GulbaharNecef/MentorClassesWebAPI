using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using WebApplication3.DTOs.StudentDTOs;
using WebApplication3.Entities;
using WebApplication3.Model;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetStudentById([FromQuery]int id)
        {
            var data = await _studentService.GetStudentById(id);
            //return StatusCode(data.StatusCode, data);
            throw new NotImplementedException();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var log = new LoggerConfiguration();
            Log.Information("Gulbaharrr");
            Log.Error("Necefzadeee");
            //LogContext
            var data = await _studentService.GetAllStudents();
            return StatusCode(data.StatusCode, data);
            
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStudent([FromBody]StudentCreateDTO studentCreateDTO)
        {
            var data = await _studentService.CreateStudent(studentCreateDTO);
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var data = await _studentService.DeleteStudent(id);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentUpdateDTO studentUpdateDTO, int Id)
        {
            var data = await _studentService.UpdateStudent(studentUpdateDTO, Id);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeSchool([FromQuery]int StudentId, int newSchoolId)
        {
            var data =await _studentService.ChangeSchool(StudentId, newSchoolId);
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetStudentsBySchoolId(int schoolId)
        {
            var data = await _studentService.GetStudentsBySchoolId(schoolId);
            return StatusCode(data.StatusCode, data);
        }



    }
}
