using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Text;
using WebApplication3.Auth;
using WebApplication3.DTOs;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Services.Implementation
{

    public class UserService : IUserService
    {
        private readonly  UserManager<AppUser>  _user_manager;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _user_manager = userManager;
            _mapper = mapper;
        }
        public async Task<ResponseModel<bool>> AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            ResponseModel<bool> response = new()
            {
                Data = false,
                StatusCode = 400
            };

            var user = await _user_manager.FindByIdAsync(userId);
            try
            {
                if (user != null)
                {
                    var userRoles = await _user_manager.GetRolesAsync(user);
                    await _user_manager.RemoveFromRolesAsync(user, userRoles);
                    await _user_manager.AddToRolesAsync(user, roles);
                    response.Data = true;
                    response.StatusCode = 200;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDTO model)
        {
            AppUser appUser = new AppUser();
            ResponseModel<CreateUserResponseDTO> responseModel = new();
            appUser.Id = Guid.NewGuid().ToString();
            appUser.UserName = model.UserName;
            appUser.Email = model.Email;
            appUser.FirstName = model.FirstName;
            appUser.LastName = model.LastName;
            var result = await _user_manager.CreateAsync(appUser,model.Password);
            
            responseModel.StatusCode = result.Succeeded ? 200 : 400;
            responseModel.Data = new CreateUserResponseDTO
            {
                Succeeded = result.Succeeded,
                
            };
            if (!result.Succeeded)
            {
                responseModel.Data.Message = string.Join("\n", result.Errors.Select(x => $"{x.Code}, {x.Description}" ));
            }

            var user = await _user_manager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user =  await _user_manager.FindByEmailAsync(model.Email);
            }
            if(user != null)
            {
                await _user_manager.AddToRoleAsync(user, "User");
            }
            return responseModel;
        }

        public async Task<ResponseModel<bool>> DeleteUserAsync(string userIdOrName)
        {
            ResponseModel<bool> response = new()
            {
                Data = false,
                StatusCode = 400
            };
            var user = await _user_manager.FindByNameAsync(userIdOrName);
            if(user == null)
            {
                user = await _user_manager.FindByIdAsync(userIdOrName);
            }
            try
            {
                if (user != null)
                {
                    await _user_manager.DeleteAsync(user);
                    response.Data = true;
                    response.StatusCode = 200;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<List<UserGetDTO>>> GetAllUsersAsync()
        {
            ResponseModel<List<UserGetDTO>> responseModel = new ResponseModel<List<UserGetDTO>>()
            {
                Data = null,
                StatusCode = 400
            };
           var users = await _user_manager.Users.ToListAsync();
            try
            {
                if(users != null && users.Count > 0)
                {
                    var data = _mapper.Map<List<UserGetDTO>>(users);
                    responseModel.Data = data;
                    responseModel.StatusCode = 200;
                    return responseModel;
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);                
            }
            return responseModel;
            
        }

        public async  Task<ResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName)//userin rollarin tapir
        {
            ResponseModel<string[]> responseModel = new ResponseModel<string[]>()
            {
                Data = null,
                StatusCode = 400
            };

            var user = await _user_manager.FindByNameAsync(userIdOrName);
            if(user == null)
            {
                user = await _user_manager.FindByIdAsync(userIdOrName);
            }
            try
            {
                if(user != null)
                {
                    var userRole = await _user_manager.GetRolesAsync(user);
                    responseModel.Data = userRole.ToArray();
                    responseModel.StatusCode = 200;
                    return responseModel;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            return responseModel;
        }

        //bunu basa dusmedim
        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _user_manager.FindByIdAsync(userId);

            if(user is not null)
            {
                byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                resetToken = Encoding.UTF8.GetString(tokenBytes);

                IdentityResult result = await _user_manager.ResetPasswordAsync(user, resetToken, newPassword);
                if(result.Succeeded)
                {
                    await _user_manager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        //bunu da tam basa dusmedim
        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate)
        {//bu authoService de olmalidirmi?
            if(user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndTime = accessTokenDate.AddMinutes(10);
                await _user_manager.UpdateAsync(user);
            }
        }

        public async Task<ResponseModel<bool>> UpdateUserAsync(UserUpdateDTO model)
        {
            ResponseModel<bool> response = new()
            {
                Data = false,
                StatusCode = 400
            };
            var user = await _user_manager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                user = await _user_manager.FindByIdAsync(model.UserId);              
            }
            try
            {
                if(user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    await _user_manager.UpdateAsync(user);
                    response.Data = true;
                    response.StatusCode = 200;
                    return response;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message); 
            }
            return response;
        }
    }
}
