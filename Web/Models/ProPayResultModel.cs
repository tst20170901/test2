using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class ProPayResultModel
    {
        public Pro_Orders OrderInfo { get; set; }
        public List<Pro_Orders_Item> Orders_Items { get; set; }
    }
}