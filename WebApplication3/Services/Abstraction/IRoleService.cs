using WebApplication3.Model;

namespace WebApplication3.Services.Abstraction
{
    public interface IRoleService
    {
        Task<ResponseModel<object>> GetAllRoles();
        Task<ResponseModel<object>> GetRoleById(string id);
        Task<ResponseModel<bool>> CreateRole(string name);
        Task<ResponseModel<bool>> DeleteRole(string id);
        Task<ResponseModel<bool>> UpdateRole(string id, string name);
    }
}
