using WebApplication3.DTOs.StudentDTOs;
using WebApplication3.Entities;
using WebApplication3.Model;

namespace WebApplication3.Services.Abstraction
{
    public interface IStudentService
    {
        public Task<ResponseModel<List<StudentGetDTO>>> GetAllStudents();
        public Task<ResponseModel<StudentGetDTO>> GetStudentById(int id);
        public Task<ResponseModel<StudentCreateDTO>> CreateStudent(StudentCreateDTO studentCreateDTO);

        public Task<ResponseModel<StudentUpdateDTO>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int id);
        public Task<ResponseModel<bool>> DeleteStudent(int id);
        public Task<ResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId );
        public Task<ResponseModel<List<StudentGetDTO>>> GetStudentsBySchoolId(int schoolId);
    }
}
