using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
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

        private readonly NewDbContext _newDbContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<School> _schoolRepository;


        //public Student student { get; private set; }

        public StudentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _schoolRepository = _unitOfWork.GetRepository<School>();
            _mapper = mapper;
        }

        public async Task<ResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId)
        {
            var studentData = await _studentRepository.GetById(studentId);
            if (studentData == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
            var schoolData = await _schoolRepository.GetById(newSchoolId);
            if (schoolData == null)
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
            if (rowAffected > 0)
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

        public async Task<ResponseModel<StudentCreateDTO>> CreateStudent(StudentCreateDTO studentCreateDTO)
        {

            if (studentCreateDTO is not null)
            {
                await _studentRepository.AddAsync(new Student()
                {
                    LastName = studentCreateDTO.LastName,
                    FirstName = studentCreateDTO.FirstName,
                    SchoolId = studentCreateDTO.SchoolId
                });
                var affectedRows = await _unitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return new ResponseModel<StudentCreateDTO>
                    {
                        Data = studentCreateDTO,
                        StatusCode = 201
                    };

                }
                else
                {
                    return new ResponseModel<StudentCreateDTO>
                    {
                        Data = null,
                        StatusCode = 500
                    };
                }
            }
            else
            {
                return new ResponseModel<StudentCreateDTO>
                {
                    Data = null,
                    StatusCode = 400
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteStudent(int id)
        {
            var deletedData = await _studentRepository.GetById(id);
            if (deletedData != null)
            {
                _studentRepository.Remove(deletedData);
                var affectedRows = await _unitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
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
            else
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
        }


        public async Task<ResponseModel<List<StudentGetDTO>>> GetAllStudents()
        {
            var studentList = await _studentRepository.GetAll().Include(x=> x.School).ToListAsync();//eager loading
            if (studentList.Count > 0)
            {
                List<StudentGetDTO> students = _mapper.Map<List<StudentGetDTO>>(studentList);
                return new ResponseModel<List<StudentGetDTO>>
                {
                    Data = students,
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseModel<List<StudentGetDTO>>
                {
                    Data = null,
                    StatusCode = 400
                };
            }
        }

        public async Task<ResponseModel<StudentGetDTO>> GetStudentById(int id)
        {
            //Student student = await _studentRepository.GetById(id).Include(s => s.School); Include() IQeryable return edende isletmek olur,
            //single entity return edende yox
            //IQueryable<Student> student = await _studentRepository.GetById(id).AsQueryable();

            Student student = await _studentRepository.GetById(id);

            if (student != null)
            {
                await _schoolRepository.GetById(student.SchoolId);//bu explicit loading dir
                
                var students = _mapper.Map<StudentGetDTO>(student);
                return new ResponseModel<StudentGetDTO>
                {
                    Data = students,
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseModel<StudentGetDTO>
                {
                    Data = null,
                    StatusCode = 400
                };
            }
        }

      

        public async Task<ResponseModel<StudentUpdateDTO>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int id)
        {
            var updatedData = await _studentRepository.GetById(id);
            if (updatedData != null)
            {
                updatedData.LastName = studentUpdateDTO.LastName;
                updatedData.FirstName = studentUpdateDTO.FirstName;
                _studentRepository.Update(updatedData);

                var affectedRows = await _unitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return new ResponseModel<StudentUpdateDTO>
                    {
                        Data = studentUpdateDTO,
                        StatusCode = 200
                    };

                }
                else
                {
                    return new ResponseModel<StudentUpdateDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            else
            {
                return new ResponseModel<StudentUpdateDTO>
                {
                    Data = null,
                    StatusCode = 400
                };
            }
        }

        public async Task<ResponseModel<List<StudentGetDTO>>> GetStudentsBySchoolId(int schoolId)
        {
            var students =  await _studentRepository.GetAll().Where(s => s.SchoolId == schoolId).Include(s=> s.School).ToListAsync();
            if(students.Count > 0)
            {
                var studentDto = _mapper.Map<List<StudentGetDTO>>(students);
                return new ResponseModel<List<StudentGetDTO>>
                {
                    Data = studentDto,
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseModel<List<StudentGetDTO>>
                {
                    Data = null,
                    StatusCode = 400
                };
            }

        }
    }
}



