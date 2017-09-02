using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ShopModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public int CoustTime { get; set; }
        public string Address { get; set; }
        public string HappyTime { get; set; }
    }
}