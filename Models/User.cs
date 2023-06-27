using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car_Showroom_Web_Application.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public int Id { get; set; }
    }
}