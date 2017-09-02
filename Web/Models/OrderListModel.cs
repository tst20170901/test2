using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class OrderListModel
    {
        public List<Orders> OrderInfoList { get; set; }
    }
    public class GoodsOrderListModel
    {
        public List<Pro_Orders> OrderInfoList { get; set; }
    }
}