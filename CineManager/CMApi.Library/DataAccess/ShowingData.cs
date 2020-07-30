using CMApi.Library.Database;
using CMApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMApi.Library.DataAccess
{
    public class ShowingData : IShowingData
    {
        private readonly ISqlDataAccess _sql;

        public ShowingData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<ShowingModel> GetShowings(DateTime date, int id)
        {
            var p = new
            {
                Date = date.ToString("yyyy-dd-MM"),
                Id = id
            };

            var res = _sql.LoadData<ShowingModel, dynamic>("spShowing_GetByIdAndDate", p, "CineManagerData");
            return res;
        }
    }
}
