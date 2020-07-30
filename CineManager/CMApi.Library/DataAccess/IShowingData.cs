using CMApi.Library.Models;
using System;
using System.Collections.Generic;

namespace CMApi.Library.DataAccess
{
    public interface IShowingData
    {
        List<ShowingModel> GetShowings(DateTime date, int id);
    }
}