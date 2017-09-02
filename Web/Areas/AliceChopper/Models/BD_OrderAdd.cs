using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class BD_OrderAdd
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Plate { get; set; }
        public string Address { get; set; }
        public decimal Money { get; set; }
        public string Remark { get; set; }
    }
}