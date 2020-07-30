using CMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDesktopApp.Library.Api
{
    public interface IShowingEndpoint
    {
        Task<List<ShowingModel>> GetShowingsByIdAndDate(int id, DateTime date);
    }
}