using CMDesktopApp.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IUserEndpoint
    {
        Task<UserModel> FindUserByEmail(string email);
        Task<List<UserModel>> GetUsers(UserManagementSearchType type);
    }
}