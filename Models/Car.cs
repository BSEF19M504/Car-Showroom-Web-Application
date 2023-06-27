using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car_Showroom_Web_Application.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }

    }
}