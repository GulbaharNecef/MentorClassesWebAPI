using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication3.Auth;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ResponseModel<bool>> CreateRole(string name)
        {
            ResponseModel<bool> responseModel = new ResponseModel<bool>();

            try
            {
                AppRole appRole = new AppRole();
                appRole.Name = name;
                appRole.Id = Guid.NewGuid().ToString();
                var data = await _roleManager.CreateAsync(appRole);

                if (data.Succeeded)
                {
                    responseModel.Data = data.Succeeded;
                    responseModel.StatusCode = 201;
                    return responseModel;
                }
                else
                {
                    responseModel.Data = data.Succeeded;
                    responseModel.StatusCode = 400;
                    return responseModel;
                }
               
            }
            catch (Exception ex)
            {
                responseModel.Data = false;
                responseModel.StatusCode = 404;
                Log.Error(ex.Message);
                return responseModel;
            }
            
        }

        public async Task<ResponseModel<bool>> DeleteRole(string id)
        {
            ResponseModel<bool> responseModel = new ResponseModel<bool>();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                var data = await _roleManager.DeleteAsync(role);
                if (data.Succeeded)
                {
                    responseModel.Data = data.Succeeded;
                    responseModel.StatusCode = 201;
                    return responseModel;
                }
                else
                {
                    responseModel.Data = false;
                    responseModel.StatusCode = 404;
                    return responseModel;
                }
                
            }
            catch(Exception ex)
            {
                responseModel.Data = false;
                responseModel.StatusCode = 404;
                Log.Error(ex.Message);
                return responseModel;
            }
        }

        public async Task<ResponseModel<object>> GetAllRoles()
        {
            ResponseModel<object> responseModel = new ResponseModel<object>();
            try
            {
                var data = await _roleManager.Roles.ToListAsync();
                if(data is not null)
                {
                    responseModel.Data = data;
                    responseModel.StatusCode = 200;
                    return responseModel;
                }

                else
                {
                    responseModel.Data = null;
                    responseModel.StatusCode = 400;
                    return responseModel;
                }


            }
            catch(Exception ex)
            {
                responseModel.Data = null;
                responseModel.StatusCode = 400;
                Log.Error(ex.Message);
                return responseModel;
            }
        }

        public async Task<ResponseModel<object>> GetRoleById(string id)
        {
            ResponseModel<object> responseModel = new ResponseModel<object>();
            try
            {
                var data = await _roleManager.FindByIdAsync(id);

                if (data is not null)
                {
                    responseModel.Data = data;
                    responseModel.StatusCode = 200;
                    return responseModel;
                }

                else
                {
                    responseModel.Data = null;
                    responseModel.StatusCode = 400;
                    return responseModel;
                }


            }
            catch (Exception ex)
            {
                responseModel.Data = null;
                responseModel.StatusCode = 400;
                Log.Error(ex.Message);
                return responseModel;
            }
        }

        public async Task<ResponseModel<bool>> UpdateRole(string id, string name)
        {
            ResponseModel<bool> responseModel = new ResponseModel<bool>();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = name;
                var data = await _roleManager.UpdateAsync(role);
                if (data.Succeeded )
                {
                    responseModel.Data = data.Succeeded;
                    responseModel.StatusCode = 200;
                    return responseModel;
                }
                else
                {
                    responseModel.Data = false;
                    responseModel.StatusCode = 404;
                    return responseModel;
                }

            }
            catch (Exception ex)
            {
                responseModel.Data = false;
                responseModel.StatusCode = 404;
                Log.Error(ex.Message);
                return responseModel;
            }
        }
    }
}
