using CMApi.Library.Models;
using System;
using System.Collections.Generic;

namespace CMApi.Library.DataAccess
{
    public interface IShowingData
    {
        List<ShowingModel> GetShowings(int id, DateTime date);
        List<ShowingModel> GetShowings(int id);
        List<ShowingModel> GetShowings(DateTime date);
        List<ShowingModel> GetShowings();
    }
}