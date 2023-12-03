using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.AccessControl;
using WebApplication3.DTOs.SchoolDTO;
using WebApplication3.Entities;
using WebApplication3.IUnitOfWorks;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Services.Implementation
{
    public class SchoolService : ISchoolService
    {
        private readonly NewDbContext _dbContext;
        private readonly IMapper _mapper;
        public SchoolService(NewDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<SchoolCreateDTO>> CreateSchool(SchoolCreateDTO schoolCreateDTO)
        {
            try
            {
                if (schoolCreateDTO is not null)
                {
                    await _dbContext.Schools.AddAsync(new School()
                    {
                        Number = schoolCreateDTO.Number,
                        Name = schoolCreateDTO.Name
                    });
                    var affectedRows = await _dbContext.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        return new ResponseModel<SchoolCreateDTO>
                        {
                            Data = schoolCreateDTO,
                            StatusCode = 201
                        };

                    }
                    else
                    {
                        return new ResponseModel<SchoolCreateDTO>
                        {
                            Data = null,
                            StatusCode = 500
                        };
                    }
                }
                else
                {
                    return new ResponseModel<SchoolCreateDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<SchoolCreateDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }

        }

        public async Task<ResponseModel<bool>> DeleteSchool(int id)
        {
            try
            {
                var deletedData = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == id);
                if (deletedData != null)
                {
                    _dbContext.Schools.Remove(deletedData);
                    var affectedRows = await _dbContext.SaveChangesAsync();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<List<SchoolGetDTO>>> GetAllSchool()
        {
            try
            {
                List<School> schoolList = await _dbContext.Schools.ToListAsync();
                if (schoolList.Count > 0)
                {
                    List<SchoolGetDTO> schools = _mapper.Map<List<SchoolGetDTO>>(schoolList);
                    return new ResponseModel<List<SchoolGetDTO>>
                    {
                        Data = schools,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<List<SchoolGetDTO>>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<List<SchoolGetDTO>>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<SchoolGetDTO>> GetSchoolById(int id)
        {
            try
            {
                School schoolList = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == id);
                if (schoolList != null)
                {
                    var schools = _mapper.Map<SchoolGetDTO>(schoolList);
                    return new ResponseModel<SchoolGetDTO>
                    {
                        Data = schools,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<SchoolGetDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<SchoolGetDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<SchoolUpdateDTO>> UpdateSchool(SchoolUpdateDTO schoolUpdateDTO)
        {
            try
            {
                var updatedData = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == schoolUpdateDTO.Id);
                if (updatedData != null)
                {
                    updatedData.Name = schoolUpdateDTO.Name;
                    updatedData.Number = schoolUpdateDTO.Number;
                    _dbContext.Schools.Update(updatedData);

                    var affectedRows = await _dbContext.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        return new ResponseModel<SchoolUpdateDTO>
                        {
                            Data = schoolUpdateDTO,
                            StatusCode = 200
                        };

                    }
                    else
                    {
                        return new ResponseModel<SchoolUpdateDTO>
                        {
                            Data = null,
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new ResponseModel<SchoolUpdateDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<SchoolUpdateDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }

        }
    }
}
