using CMDesktopApp.Library.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public class ShowingEndpoint : IShowingEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public ShowingEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<ShowingModel>> GetShowingsByIdAndDate(string id, DateTime date)
        {
            var query = new Dictionary<string, string>
            {
                ["id"] = id.ToString(),
                ["date"] = date.ToString()
            };

            string uri = QueryHelpers.AddQueryString("/api/showing", query);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ShowingModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
