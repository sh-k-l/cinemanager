using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IFilmEndpoint
    {
        Task<List<FilmModel>> GetAllFilms();
        Task<List<FilmModel>> GetFilmsByDate(string date);
        Task<int> AddFilm(FilmModel film);
        Task EditFilm(FilmModel film);
        Task DeleteFilm(string id);
    }
}