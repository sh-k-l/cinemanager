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

        public async Task<List<FilmModel>> GetFilmsByDate(string date)
        {
            var query = new Dictionary<string, string>
            {
                ["date"] = date
            };

            string uri = QueryHelpers.AddQueryString("/api/film", query);

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

        public async Task<int> AddFilm(FilmModel film)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/film", film))
            {
                if (response.IsSuccessStatusCode)
                {
                    int id = await response.Content.ReadAsAsync<int>();
                    return id;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public async Task EditFilm(FilmModel film)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync($"/api/film/{film.Id}", film))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteFilm(string id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"/api/film/{id}"))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
