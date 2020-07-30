using CMApi.Library.Database;
using CMApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMApi.Library.DataAccess
{
    public class FilmData : IFilmData
    {
        private readonly ISqlDataAccess _sql;

        public FilmData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<FilmModel> GetAllFilms()
        {
            var results = _sql.LoadData<FilmModel, dynamic>("spFilm_GetAll", new { }, "CineManagerData");
            return results;
        }

        public List<FilmModel> GetFilmsByDate(string date)
        {
            var p = new { Date = date };

            var results = _sql.LoadData<FilmModel, dynamic>("spFilm_GetByDate", p, "CineManagerData");
            return results;
        }
    }
}
