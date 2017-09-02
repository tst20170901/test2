using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.app.Models
{
    public class CouponsListModel
    {
        public List<string> Business { get; set; }
        public List<CouponsModel> CouponsList { get; set; }
    }
}