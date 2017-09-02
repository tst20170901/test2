using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Text;

namespace Web.Units
{
    public class UserBaseController : Controller
    {
        public UserWorkContext WorkContext = new UserWorkContext();
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WorkContext.UrlReferrer = AliceDAL.Common.GetUrlReferrer();
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
                    filterContext.Result = new RedirectResult("/Login/?returnUrl=" + WorkContext.UrlReferrer);
                return;
            }
        }
        public JsonResult AjaxResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(AliceDAL.Common.GetUrlReferrer(), message));
        }
        protected ViewResult PromptView(string backUrl, string message)
        {
            PromptModel pm = new PromptModel(backUrl, message);
            return View("prompt", pm);
        }
        protected ViewResult PromptView(string backUrl, string message, bool isAutoBack)
        {
            return View("prompt", new PromptModel(backUrl, message) { IsAutoBack = isAutoBack });
        }
        private BD_Users GetUser()
        {
            BD_Users admin = null;
            if (System.Web.HttpContext.Current.Request.Cookies["UserLoginInfo"] == null) return null;
            admin = DAL.BD_Users.GetUserByIDPassword(AliceDAL.DataType.ConvertToInt(AliceDAL.Common.GetCookie("UserLoginInfo", "ID")), AliceDAL.Common.GetCookie("UserLoginInfo", "UserPwd"));
            return admin;
        }
    }
}