using CMApi.Library.Database;
using CMApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMApi.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public UserModel GetUserById(string id)
        {
            var user = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookupById", new { Id = id }, "CineManagerData").First();
            return user;
        }

    }
}
