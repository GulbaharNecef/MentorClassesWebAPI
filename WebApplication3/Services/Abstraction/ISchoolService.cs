using WebApplication3.DTOs.SchoolDTO;
using WebApplication3.Entities;
using WebApplication3.Model;

namespace WebApplication3.Services.Abstraction
{
    public interface ISchoolService
    {
        public Task<ResponseModel<List<SchoolGetDTO>>> GetAllSchool();
        public Task<ResponseModel<SchoolGetDTO>> GetSchoolById(int id);
        public Task<ResponseModel<SchoolUpdateDTO>> UpdateSchool(SchoolUpdateDTO schoolUpdateDTO);
        public Task<ResponseModel<SchoolCreateDTO>> CreateSchool(SchoolCreateDTO schoolCreateDTO);
        public Task<ResponseModel<bool>> DeleteSchool(int id);
        //Change School-refactoring

     
    }
}
