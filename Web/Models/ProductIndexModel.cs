using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class ProductIndexModel
    {
        public Pro_Orders OrderInfo { get; set; }
        public decimal Money { get; set; }
        public Pro_Case Product { get; set; }
    }
}