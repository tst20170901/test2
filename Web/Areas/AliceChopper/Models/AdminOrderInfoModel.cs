using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.AliceChopper.Models
{
    public class AdminOrderInfoModel
    {
        public List<OrderActions> ActionsList { get; set; }
        public AdminOrders OrderInfo { get; set; }
        public BD_Users UserInfo { get; set; }
    }
}