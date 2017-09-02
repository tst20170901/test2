using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UserVipCardAndWashItemModel
    {
        public string ListVipCard { get; set; }
        public List<Wash_Item> WashItem { get; set; }
    }
}