using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IFilmEndpoint
    {
        Task<List<FilmModel>> GetAllFilms();
        Task<List<FilmModel>> GetFilmsByDate(DateTime date);
    }
}