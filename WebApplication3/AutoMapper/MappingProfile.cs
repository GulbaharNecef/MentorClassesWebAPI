using AutoMapper;
using WebApplication3.DTOs.SchoolDTO;
using WebApplication3.DTOs.StudentDTOs;
using WebApplication3.Entities;

namespace WebApplication3.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<School, SchoolGetDTO>().ReverseMap();
            CreateMap<Student, StudentGetDTO>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.Name))
                .ReverseMap();
                
        }
    }
}
