using CMDesktopApp.Library.Models;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IAuthEndpoint
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}