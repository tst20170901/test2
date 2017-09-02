using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class VipTypeToBusi
    {
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public List<VipTypeSimple> TypeList { get; set; }
    }
    public class VipTypeSimple
    {
        public int TypeID { get; set; }
        public string Title { get; set; }
    }
}