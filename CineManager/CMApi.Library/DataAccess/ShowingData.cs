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

        public List<ShowingModel> GetShowings(int id, DateTime date)
        {
            var p = new
            {
                Date = date.ToString("yyyy-dd-MM"),
                Id = id
            };

            var res = _sql.LoadData<ShowingModel, dynamic>("spShowing_GetByIdAndDate", p, "CineManagerData");
            return res;
        }

        public List<ShowingModel> GetShowings(int id)
        {
            var p = new
            {
                Id = id
            };

            var res = _sql.LoadData<ShowingModel, dynamic>("spShowing_GetById", p, "CineManagerData");
            return res;
        }

        public List<ShowingModel> GetShowings(DateTime date)
        {
            var p = new
            {
                Date = date.ToString("yyyy-dd-MM"),
            };

            var res = _sql.LoadData<ShowingModel, dynamic>("spShowing_GetByDate", p, "CineManagerData");
            return res;
        }

        public List<ShowingModel> GetShowings()
        {
            var res = _sql.LoadData<ShowingModel, dynamic>("spShowing_GetAll", new { }, "CineManagerData");
            return res;
        }
    }
}
