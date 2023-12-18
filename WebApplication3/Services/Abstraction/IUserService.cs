using WebApplication3.Auth;
using WebApplication3.DTOs;
using WebApplication3.Model;

namespace WebApplication3.Services.Abstraction
{
    public interface IUserService
    {
        Task<ResponseModel<CreateUserDTO>> CreateAsync(CreateUserDTO model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate);
        public Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);

        public Task<ResponseModel<List<UserGetDTO>>> GetAllUsersAsync();
        public Task<ResponseModel<bool>> AssignRoleToUserAsnyc(string userId, string[] roles);
        public Task<ResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName);
        public Task<ResponseModel<bool>> DeleteUserAsync(string userIdOrName);
        public Task<ResponseModel<bool>> UpdateUserAsync(UserUpdateDTO model);
    }
}
