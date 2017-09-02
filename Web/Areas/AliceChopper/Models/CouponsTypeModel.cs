using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class CouponsTypeModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int BusinessID { get; set; }
        public string SMSContent { get; set; }
        public int Count { get; set; }
        public int Period { get; set; }
    }
}