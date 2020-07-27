using CMDesktopApp.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IUserEndpoint
    {
        Task AddUserToRole(string userId, string roleName);
        Task<UserModel> FindUserByEmail(string email);
        Task<Dictionary<string, string>> GetAllRoles();
        Task<List<UserModel>> GetUsers(UserManagementSearchType type);
        Task RemoveUserFromRole(string userId, string roleName);
    }
}