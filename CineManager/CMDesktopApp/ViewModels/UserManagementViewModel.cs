using Caliburn.Micro;
using CMDesktopApp.Library.Api;
using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.ViewModels
{
    public class UserManagementViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;

        public UserManagementViewModel(IUserEndpoint userEndpoint)
        {
            _userEndpoint = userEndpoint;
            var x = FindByEmail("shakilchyy@gmail.com");
            var y = FindByEmail("shakilchyy@l.com");
        }

        private async Task GetAllUsers()
        {
            var x = await _userEndpoint.GetAllUsers();
        }

        private async Task<UserModel> FindByEmail(string email)
        {
            var user = await _userEndpoint.FindUserByEmail(email);
            return user;
        }
    }
}
