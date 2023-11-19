using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DTOs.SchoolDTO;
using WebApplication3.DTOs.StudentDTOs;
using WebApplication3.Entities;
using WebApplication3.IRepositories;
using WebApplication3.IRepositories.IStudentRepos;
using WebApplication3.IUnitOfWorks;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Services.Implementation
{
    public class StudentService : IStudentService
    {

        public readonly NewDbContext _newDbContext;
        public readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<School> _schoolRepository;


        //public Student student { get; private set; }

        public StudentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this._studentRepository = _unitOfWork.GetRepository<Student>();
            //this._schoolRepository = _unitOfWork.GetRepository<School>();
            _mapper = mapper;
        }

        public Task<ResponseModel<StudentCreateDTO>> CreateStudent(StudentCreateDTO studentCreateDTO, int schoolId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<bool>> DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<List<StudentGetDTO>>> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<StudentGetDTO>> GetStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<StudentUpdateDTO>> UpdateStudent(StudentUpdateDTO studentUpdateDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> GetStudentId(int id)
        {
            var data = await _studentRepository.GetById(id);
            return data;
        }

        public async Task<ResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId)
        {
            var studentData = await _studentRepository.GetById(studentId);
            if(studentData  == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
            var schoolData = await _schoolRepository.GetById(newSchoolId);
            if(schoolData == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
            studentData.SchoolId = newSchoolId;
            _studentRepository.Update(studentData);
            int rowAffected = await _unitOfWork.SaveChangesAsync();
            if(rowAffected > 0)
            {
                return new ResponseModel<bool>
                {
                    Data = true,
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500
                };
            }
        }
    }
}
