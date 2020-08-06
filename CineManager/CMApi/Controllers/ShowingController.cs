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
        public List<ShowingModel> GetShowingsByDateAndId(DateTime? date = null, int id = 0)
        {
            List<ShowingModel> result;

            if(date == null && id == 0)
            {
                result = _showingData.GetShowings();
            }
            else if(date == null)
            {
                result = _showingData.GetShowings(id);
            }
            else if (id == 0)
            {
                result = _showingData.GetShowings((DateTime)date);
            }
            else
            {
                result = _showingData.GetShowings(id, (DateTime)date);
            }

            return result;
        }
    }
}
