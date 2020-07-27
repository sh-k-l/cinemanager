using System.Collections.Generic;

namespace CMDesktopApp.Library.Models
{
    public interface IAuthenticatedUser
    {
        string Access_Token { get; set; }
        List<string> Roles { get; set; }
        string Username { get; set; }

        void Reset();
        void SetFields(IAuthenticatedUser user);
    }
}