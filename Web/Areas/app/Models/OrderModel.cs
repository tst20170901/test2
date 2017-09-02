using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.app.Models
{
    public class OrderModel
    {
        public int ID { get; set; }
        public string Osn { get; set; }
        public int Uid { get; set; }
        public string Plate { get; set; }
        public int OrderState { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string ItemName { get; set; }
        public string CDate { get; set; }
        public string BookDate { get; set; }
    }
}