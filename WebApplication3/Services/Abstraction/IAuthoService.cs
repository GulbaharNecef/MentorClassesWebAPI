using WebApplication3.DTOs;
using WebApplication3.Model;

namespace WebApplication3.Services.Abstraction
{
    public interface IAuthoService
    {
        Task<ResponseModel<TokenDTO>> LoginAsync(string userNameOrEmail, string password);
        Task<ResponseModel<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken);
        Task<ResponseModel<bool>> LogOut(string userNameOrEmail);
        
        //public Task<string> PasswordResetAsnyc(string email);
        //public Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
