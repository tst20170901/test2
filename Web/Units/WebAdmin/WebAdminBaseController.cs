using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class WebAdminBaseController : WebAdminAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            WebAdminContext.IP = AliceDAL.Common.GetIP();
            BD_Admin admin = GetUser();
            if (admin != null)
            {
                WebAdminContext.Data1 = admin.Data1;
                WebAdminContext.Data2 = admin.Data2;
                WebAdminContext.Data3 = admin.Data3;
                WebAdminContext.Data4 = admin.Data4;
                WebAdminContext.Data5 = admin.Data5;
                WebAdminContext.Data6 = admin.Data6;
                WebAdminContext.Data7 = admin.Data7;
                WebAdminContext.Data8 = admin.Data8;
                WebAdminContext.Data9 = admin.Data9;
                WebAdminContext.Data10 = admin.Data10;
                WebAdminContext.LoginID = admin.LoginID;
                WebAdminContext.NickName = admin.NickName;
                WebAdminContext.Password = admin.Password;
                WebAdminContext.ID = admin.ID;
                WebAdminContext.RoleID = admin.RoleID;
                WebAdminContext.BranchID = admin.BranchID;
                WebAdminContext.Branch = DAL.BD_Branch.GetModel(admin.BranchID);
                WebAdminContext.PageSize = 10;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            if (WebAdminContext.ID <= 0 && String.IsNullOrEmpty(WebAdminContext.LoginID))
            {
                if (WebAdminContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/WebAdmin/Account/Login/");
                return;
            }
            //如果当前用户不是管理员
            if (WebAdminContext.RoleID < 1)
            {
                if (!WebAdminContext.IsHttpAjax)
                {
                    filterContext.Result = new RedirectResult("/WebAdmin/Account/Login/");
                    return;
                }
            }

            //如果当前用户不属于公司
            if (WebAdminContext.BranchID < 1 || WebAdminContext.Branch == null)
            {
                if (WebAdminContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/WebAdmin/Account/Login/");
                return;
            }
        }
        protected ActionResult MessageModel(string code, string content)
        {
            return Content(string.Format("{0}\"code\":\"{1}\",\"msg\":\"{2}\",\"data\":null{3}", "{", code, content, "}"));
        }
        private BD_Admin GetUser()
        {
            BD_Admin admin = new BD_Admin();
            admin.LoginID = AliceDAL.Common.GetCookie("aliceinfo", "LoginID");
            admin.Password = AliceDAL.Common.GetCookie("aliceinfo", "Password");
            admin.ID = AliceDAL.DataType.ConvertToInt(AliceDAL.Common.GetCookie("aliceinfo", "ID"));
            admin.RoleID = AliceDAL.DataType.ConvertToInt(AliceDAL.Common.GetCookie("aliceinfo", "RoleID"));
            admin.BranchID = AliceDAL.DataType.ConvertToInt(AliceDAL.Common.GetCookie("aliceinfo", "BranchID"));
            admin.NickName = AliceDAL.Common.GetCookie("aliceinfo", "NickName");
            admin.Data1 = AliceDAL.Common.GetCookie("aliceinfo", "Data1");
            admin.Data2 = AliceDAL.Common.GetCookie("aliceinfo", "Data2");
            admin.Data3 = AliceDAL.Common.GetCookie("aliceinfo", "Data3");
            admin.Data4 = AliceDAL.Common.GetCookie("aliceinfo", "Data4");
            admin.Data5 = AliceDAL.Common.GetCookie("aliceinfo", "Data5");
            admin.Data6 = AliceDAL.Common.GetCookie("aliceinfo", "Data6");
            admin.Data7 = AliceDAL.Common.GetCookie("aliceinfo", "Data7");
            admin.Data8 = AliceDAL.Common.GetCookie("aliceinfo", "Data8");
            admin.Data9 = AliceDAL.Common.GetCookie("aliceinfo", "Data9");
            admin.Data10 = AliceDAL.Common.GetCookie("aliceinfo", "Data10");
            return admin;
        }
    }
}