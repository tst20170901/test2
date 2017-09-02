using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.BusinessManger.Models
{
    public class ProOrderInfoModel
    {
        public List<UserOrderActions> UserActionsList { get; set; }
        public Pro_Orders OrderInfo { get; set; }
        public List<Pro_Orders_Item> OrderItems { get; set; }
    }
}