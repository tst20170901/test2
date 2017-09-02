using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class PayResultModel
    {
        public Orders OrderInfo { get; set; }
        public List<Orders_Item> Orders_Items { get; set; }
    }
}