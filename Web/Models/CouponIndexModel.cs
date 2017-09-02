using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Models
{
    public class CouponIndexModel
    {
        public Orders OrderInfo { get; set; }
        public decimal Money { get; set; }
        public List<UserCarModel> ListCar { get; set; }
    }
}