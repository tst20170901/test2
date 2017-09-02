using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CarsBrand
    {
        public int id { get; set; }
        public string py { get; set; }
        public string name { get; set; }
        public List<Cars> models { get; set; }
    }
    public class Cars
    {
        public int id { get; set; }
        public string py { get; set; }
        public string name { get; set; }
    }
}