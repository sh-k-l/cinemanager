using CMApi.Library.Models;
using System;
using System.Collections.Generic;

namespace CMApi.Library.DataAccess
{
    public interface IFilmData
    {
        List<FilmModel> GetAllFilms();
        List<FilmModel> GetFilmsByDate(DateTime date);
    }
}