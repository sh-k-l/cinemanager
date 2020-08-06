using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMApi.Library.DataAccess;
using CMApi.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmData _filmData;

        public FilmController(IFilmData filmData)
        {
            _filmData = filmData;
        }
        
        [HttpGet]
        public List<FilmModel> GetAllFilms(string date = null)
        {
            List<FilmModel> result;

            if(date == null)
            {
                result = _filmData.GetFilms();
            }
            else
            {
                result = _filmData.GetFilms(date);
            }

            return result;
        }

        [HttpPost]
        public void AddFilm(FilmModel film)
        {
            _filmData.AddFilm(film);
        }

        [HttpPut("{id}")]
        public void EditFilm(string id, FilmModel film)
        {
            _filmData.EditFilm(film);
        }

        [HttpDelete("{id}")]
        public void DeleteFilm(string id)
        {
            _filmData.DeleteFilm(id);
        }
    }
}
