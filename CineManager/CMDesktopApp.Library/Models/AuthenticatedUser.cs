using System;
using System.Collections.Generic;
using System.Text;

namespace CMDesktopApp.Library.Models
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public string Access_Token { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public void Reset()
        {
            Access_Token = "";
            Username = "";
            Roles.Clear();
        }

        public void SetFields(IAuthenticatedUser user)
        {
            Access_Token = user.Access_Token;
            Username = user.Username;
            Roles = user.Roles;
        }
    }
}
