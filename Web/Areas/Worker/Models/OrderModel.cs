using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Worker.Models
{
    public class OrderModel
    {
        public int ID { get; set; }
        public string Osn { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public int OrderState { get; set; }
        public string CDate { get; set; }
    }
}