using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMApi.Library.DataAccess;
using CMApi.Library.Database;
using CMApi.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowingController : ControllerBase
    {
        private readonly IShowingData _showingData;

        public ShowingController(IShowingData showingData)
        {
            _showingData = showingData;
        }
        
        [HttpGet]
        public List<ShowingModel> GetShowingsByDateAndId(DateTime date, int id)
        {
            var result = _showingData.GetShowings(date, id);
            return result;
        }
    }
}
