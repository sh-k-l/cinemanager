using CMApi.Library.Models;

namespace CMApi.Library.DataAccess
{
    public interface IUserData
    {
        UserModel GetUserById(string id);
    }
}