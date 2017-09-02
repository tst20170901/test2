using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.WebWorker.Models
{
    public class CarInfo
    {
        public int OrderID { get; set; }
        public string Plate { get; set; }
        public string BrandShow { get; set; }
        public string Color { get; set; }
        public string SafeCompany { get; set; }
        public string DueTime { get; set; }
        public string VinNo { get; set; }
    }
}