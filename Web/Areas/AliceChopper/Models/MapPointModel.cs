using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.AliceChopper.Models
{
    public class MapPointModel
    {
        public int ID { get; set; }
        public string Osn { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
        public string Icon { get; set; }
    }
}