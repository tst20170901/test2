using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public class AdminWorkContext
    {
        public bool IsHttpAjax;
        public int PageSize;
        public string IP;
        public string Url;
        public string UrlReferrer;
        public int ID;
        public int RoleID;
        public BD_BearActionsAllot ActionsAllot;
        public BD_Branch Branch;
        public int BranchID;
        public string LoginID;
        public string Password;
        public string NickName;
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
        public string Controller;//控制器
        public string Action;//动作方法
        public string PageKey;//页面标示符
    }
}