using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.app.Models
{
    public class UserModel
    {
        public int Uid { get; set; }
        public string LoginID { get; set; }
        public string EncryptPassword { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public decimal WalletMoney { get; set; }
        public int CouponCount { get; set; }
        public DateTime CDate { get; set; }
    }
}