using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class CouponsSingleModel
    {
        public string Mobile { get; set; }
        public int BusinessID { get; set; }
        public int TypeID { get; set; }
        public int Count { get; set; }
        public string SMSContent { get; set; }
    }
}