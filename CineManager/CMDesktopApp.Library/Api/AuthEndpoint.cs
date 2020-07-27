using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public class AuthEndpoint : IAuthEndpoint
    {
        private readonly IApiHelper _apiHelper;
        public AuthEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });


            try
            {
                using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync("/Authenticate", data))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                        return result;
                    }
                    else
                    {
                        throw new Exception($"{response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
