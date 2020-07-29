using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowingController : ControllerBase
    {
        [HttpGet]
        public List<ShowingModel> GetShowings(DateTime date, int id)
        {

        }
    }
}
