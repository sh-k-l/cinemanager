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

    }
}
