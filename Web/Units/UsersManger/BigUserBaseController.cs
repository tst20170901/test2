using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class BigUserBaseController : UsersAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            BD_Users admin = GetUser();
            if (admin != null)
            {
                WorkContext.Data1 = admin.Data1;
                WorkContext.Data2 = admin.Data2;
                WorkContext.Data3 = admin.Data3;
                WorkContext.Data4 = admin.Data4;
                WorkContext.Data5 = admin.Data5;
                WorkContext.Data6 = admin.Data6;
                WorkContext.Data7 = admin.Data7;
                WorkContext.Data8 = admin.Data8;
                WorkContext.Data9 = admin.Data9;
                WorkContext.Data10 = admin.Data10;
                WorkContext.LoginID = admin.LoginID;
                WorkContext.CDate = admin.CDate;
                WorkContext.Mobile = admin.Mobile;
                WorkContext.UserPwd = admin.UserPwd;
                WorkContext.ID = admin.ID;
                WorkContext.PageSize = 10;
                WorkContext.wallet = DAL.BD_Users.GetUserWalletByUserID(WorkContext.ID);
                WorkContext.BranchID = admin.BranchID;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            if (WorkContext.ID <= 0 && String.IsNullOrEmpty(WorkContext.LoginID))
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/UsersManger/FrameSet/Login/");
                return;
            }
        }
        private BD_Users GetUser()
        {
            BD_Users admin = null;
            if (System.Web.HttpContext.Current.Request.Cookies["UserLoginInfo"] == null) return null;
            admin = new BD_Users();
            HttpCookie c = Request.Cookies["UserLoginInfo"];
            admin.LoginID = c.Values["LoginID"];
            admin.UserPwd = c.Values["UserPwd"];
            admin.ID = int.Parse(c.Values["ID"]);
            admin.Mobile = c.Values["Mobile"];
            admin.Data1 = c.Values["Data1"];
            admin.Data2 = c.Values["Data2"];
            admin.Data3 = c.Values["Data3"];
            admin.Data4 = c.Values["Data4"];
            admin.Data5 = c.Values["Data5"];
            admin.Data6 = c.Values["Data6"];
            admin.Data7 = c.Values["Data7"];
            admin.Data8 = c.Values["Data8"];
            admin.Data9 = c.Values["Data9"];
            admin.Data10 = c.Values["Data10"];
            admin.CDate = AliceDAL.DataType.ConvertToDateTime(c.Values["CDate"]);
            admin.BranchID = AliceDAL.DataType.ConvertToInt(c.Values["BranchID"]);
            return admin;
        }
    }
}