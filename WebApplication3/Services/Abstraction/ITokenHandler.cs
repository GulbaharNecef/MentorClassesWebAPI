using WebApplication3.Auth;
using WebApplication3.DTOs;

namespace WebApplication3.Services.Abstraction
{
    public interface ITokenHandler
    {
        Task<TokenDTO> GetTokenAsync(AppUser appUser);
        string GetRefreshTokenAsync();
    }
}
