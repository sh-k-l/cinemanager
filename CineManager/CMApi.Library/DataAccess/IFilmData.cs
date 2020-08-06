using CMApi.Library.Models;
using System;
using System.Collections.Generic;

namespace CMApi.Library.DataAccess
{
    public interface IFilmData
    {
        void AddFilm(FilmModel film);
        void EditFilm(FilmModel film);
        List<FilmModel> GetFilms();
        List<FilmModel> GetFilms(string date);
    }
}