using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.UsersManger.Models
{
    public class BD_UserCarModel
    {
        public int ID { get; set; }
        public string Plate { get; set; }
        public int BrandID { get; set; }
        public int ModelID { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ModelStr { get; set; }
        public string UserName { get; set; }
        public string CarGroup { get; set; }
    }
}