using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
using System.Text.Unicode;
using WebApplication3.Auth;
using WebApplication3.DTOs;
using WebApplication3.Model;
using WebApplication3.Services.Abstraction;

namespace WebApplication3.Services.Implementation
{
    public class AuthoService : IAuthoService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;//bunu bilmedim
        public AuthoService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;

        }
        public async Task<ResponseModel<TokenDTO>> LoginAsync(string userNameOrEmail, string password)
        {
            ResponseModel<TokenDTO> response = new ResponseModel<TokenDTO>()
            {
                Data = null,
                StatusCode = 400
            };
            try
            {
                if (userNameOrEmail != null && password != null)
                {
                    AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
                    if (user == null)
                    {
                        user = await _userManager.FindByEmailAsync(userNameOrEmail);
                    }
                    if (user == null)
                    {
                        return response;
                    }
                    SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false); //lockOutOnFailure false qoydum, 5 defe sehv data girende necese deqiqe girmek olmur sisteme
                    if (result.Succeeded)//Authentication Ugurlu
                    {
                        //Authorizasiya edilmelidi
                        TokenDTO tokenDTO = await _tokenHandler.CreateAccessTokenAsync(user);
                        await _userService.UpdateRefreshToken(tokenDTO.RefershToken, user, tokenDTO.ExpirationTime); //bunu da anlamadim
                        return new ResponseModel<TokenDTO>()
                        {
                            Data = tokenDTO,
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new()
                        {
                            Data = null,
                            StatusCode = 401
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken)
        {
            ResponseModel<TokenDTO> response = new()
            {
                Data = null,
                StatusCode = 400
            };
            try
            {
                AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

                //rft vaxti bitmeyibse login ola biler
                if (user != null && user.RefreshTokenEndTime > DateTime.UtcNow)
                {
                    //token yaradiriq hem de rft update edirik
                    TokenDTO tokenDTO = await _tokenHandler.CreateAccessTokenAsync(user);
                    await _userService.UpdateRefreshToken(tokenDTO.RefershToken, user, tokenDTO.ExpirationTime);

                    response.Data = tokenDTO;
                    response.StatusCode = 200;
                    return response;
                    //var result =  await _signInManager.RefreshSignInAsync(user);
                }
                else
                {
                    return new()
                    {
                        Data = null,
                        StatusCode = 401
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<ResponseModel<bool>> LogOut(string userNameOrEmail)
        {
            AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            }
            if(user is null)
            {
                return new()
                {
                    Data = false,
                    StatusCode = 400
                };
            }

            user.RefreshTokenEndTime = null;
            user.RefreshToken = null;

            var result = await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();

            if(result.Succeeded)
            {
                return new()
                {
                    Data = true,
                    StatusCode = 200
                };
            }
            else
            {
                return new()
                {
                    Data = false,
                    StatusCode = 400
                };
            }
        }

        public async Task<ResponseModel<bool>> PasswordResetAsync(string email, string currentPass, string newPass)
        {
            ResponseModel<bool> response = new()
            {
                Data = false,
                StatusCode = 404
            };
            AppUser user = await _userManager.FindByEmailAsync(email);
            if(user is not null)
            {
                var data  = await _userManager.ChangePasswordAsync(user, currentPass, newPass);

                if (data.Succeeded)
                {
                    response.Data = true;
                    response.StatusCode = 200;
                    return response;
                }
            }
            return response;
        }
    }
}
