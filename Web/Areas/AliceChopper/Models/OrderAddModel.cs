using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Areas.AliceChopper.Models
{
    public class OrderAddModel
    {
        public List<Wash_Item> Items { get; set; }
        public List<UserCar> Cars { get; set; }
        public decimal Money { get; set; }
        public System.Data.DataTable group { get; set; }
    }
}