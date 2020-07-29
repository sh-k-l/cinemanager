using CMDesktopApp.Library.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public class FilmEndpoint : IFilmEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public FilmEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<FilmModel>> GetAllFilms()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/film"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<FilmModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<FilmModel>> GetFilmsByDate(DateTime date)
        {
            var query = new Dictionary<string, string>
            {
                ["date"] = date.ToString()
            };

            string uri = QueryHelpers.AddQueryString("/api/film/date", query);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<FilmModel>>();
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
