using WebApplication3.Auth;
using WebApplication3.DTOs;

namespace WebApplication3.Services.Abstraction
{
    public interface ITokenHandler
    {
        Task<TokenDTO> CreateAccessTokenAsync(AppUser appUser);
        string CreateRefreshToken();
    }
}
