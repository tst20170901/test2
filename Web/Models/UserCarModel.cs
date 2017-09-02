using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UserCarModel
    {
        public int ID { get; set; }
        public string Plate { get; set; }
        public string BrandShow { get; set; }
        public string Color { get; set; }
        public int UserID { get; set; }
    }
}