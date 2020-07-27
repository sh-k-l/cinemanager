using System;
using System.Collections.Generic;
using System.Text;

namespace CMApi.Library.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
