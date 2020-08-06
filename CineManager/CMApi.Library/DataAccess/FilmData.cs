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

        public List<FilmModel> GetFilms()
        {
            var results = _sql.LoadData<FilmModel, dynamic>("spFilm_GetAll", new { }, "CineManagerData");
            return results;
        }

        public List<FilmModel> GetFilms(string date)
        {
            var p = new { Date = date };

            var results = _sql.LoadData<FilmModel, dynamic>("spFilm_GetByDate", p, "CineManagerData");
            return results;
        }

        public void AddFilm(FilmModel film)
        {
             _sql.SaveData("spFilm_Insert", film, "CineManagerData");
        }

        public void EditFilm(FilmModel film)
        {
            _sql.SaveData("spFilm_Update", film, "CineManagerData");
        }
    }
}
