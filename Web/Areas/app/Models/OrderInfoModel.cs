using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.app.Models
{
    public class OrderInfoModel
    {
        public int ID { get; set; }
        public string Osn { get; set; }
        public string PicBegin { get; set; }
        public string PicAfter { get; set; }
        public List<UserOrderActions> UserActionsList { get; set; }

    }
}