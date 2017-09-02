using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.WebWorker.Models
{
    public class OrderInfoModel
    {
        public Orders OrderInfo { get; set; }
        public List<Orders_Item> OrderItems { get; set; }
        public List<Wash_Item> WashItems { get; set; }
    }
}