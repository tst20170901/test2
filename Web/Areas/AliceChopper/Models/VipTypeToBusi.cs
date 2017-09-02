using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class CouponsTypeToBusi
    {
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public List<CouponsTypeSimple> TypeList { get; set; }
    }
    public class CouponsTypeSimple
    {
        public int TypeID { get; set; }
        public string Title { get; set; }
    }
}