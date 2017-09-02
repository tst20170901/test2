using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class OrderInfoModel
    {
        public List<UserOrderActions> UserActionsList { get; set; }
        public Orders OrderInfo { get; set; }
        public List<Wash_Item> WashItems { get; set; }
        public List<Orders_Item> OrdersItem { get; set; }
        public WorkShop WorkShop { get; set; }
    }
    public class ProOrderInfoModel
    {
        public List<UserOrderActions> UserActionsList { get; set; }
        public Pro_Orders OrderInfo { get; set; }
        public List<Pro_Orders_Item> OrdersItem { get; set; }
    }
}