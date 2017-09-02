using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Web.Units
{
    public class AppContext
    {
        public bool IsHttpAjax;
        public int PageSize;
        public string IP;
        public int ID = -1;
        public string LoginID;
        public string UserPwd;
        public string Mobile;
        public decimal WalletMoney = 0;
        public BD_Wallet Wallet;
        public int CouponCount = 0;
        public string Data1;
        public string Data2;
        public string Data3;
        public string Data4;
        public string Data5;
        public string Data6;
        public string Data7;
        public string Data8;
        public string Data9;
        public string Data10;
        public DateTime CDate;
    }
}