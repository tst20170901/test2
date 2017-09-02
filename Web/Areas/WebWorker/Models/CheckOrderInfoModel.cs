using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.WebWorker.Models
{
    public class CheckOrderInfoModel
    {
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Plate { get; set; }
        public string BrandShow { get; set; }
        public string Color { get; set; }
        public string WashItem { get; set; }
        public string WorkName { get; set; }
        public string WorkMobile { get; set; }
        public string Address { get; set; }
        public string OrderState { get; set; }
        public string CDate { get; set; }
        public string WorkState { get; set; }
    }
}