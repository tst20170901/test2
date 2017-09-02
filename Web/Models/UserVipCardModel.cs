using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UserVipCardModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string VipDes { get; set; }
        public int CardCount { get; set; }
        public int UserCount { get; set; }
        public int FreeID { get; set; }
        public string Plate { get; set; }
        public string CardState { get; set; }
        public string CDate { get; set; }
        public string TDate { get; set; }
        public string BusinessName { get; set; }
    }
    public class UserCouponsModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int BusinessID { get; set; }
        public int TypeID { get; set; }
        public string BusinessName { get; set; }
        public string CDate { get; set; }
        public string TDate { get; set; }
    }

    public class VipModel
    {
        public string ListCoupons { get; set; }
    }
}