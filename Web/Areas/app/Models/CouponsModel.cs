using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.app.Models
{
    public class CouponsModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Business { get; set; }
        public decimal Price { get; set; }
        public DateTime EndDate { get; set; }
    }
}