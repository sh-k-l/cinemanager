using CMDesktopApp.Library.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public UserEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetUsers(UserManagementSearchType type)
        {
            var query = new Dictionary<string, string>
            {
                ["type"] = type.ToString()
            };

            string uri = QueryHelpers.AddQueryString("/api/user/admin/getusers/", query);

            try
            {
                using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<List<UserModel>>();
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<UserModel> FindUserByEmail(string email)
        {
            var query = new Dictionary<string, string>
            {
                ["email"] = email
            };

            string uri = QueryHelpers.AddQueryString("/api/user/admin/findbyemail", query);

            try
            {
                using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<UserModel>();
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/user/admin/roles"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task AddUserToRole(string userId, string roleName)
        {
            var data = new { userId, roleName };

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/roles/add", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            var data = new { userId, roleName };

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/roles/remove", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
