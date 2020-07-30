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
        [Route("/api/film")]
        public List<FilmModel> GetAllFilms()
        {
            var result = _filmData.GetAllFilms();
            return result;
        }

        [HttpGet]
        [Route("/api/film/date")]
        public List<FilmModel> GetFilmsByDate(string date)
        {
            var result = _filmData.GetFilmsByDate(date);
            return result;
        }

    }
}
