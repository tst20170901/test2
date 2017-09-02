using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class WebWorkerBaseController : WebWorkerAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            WebWorkContext.IP = AliceDAL.Common.GetIP();
            WorkShop admin = GetUser();
            if (admin != null)
            {
                WebWorkContext.ID = admin.ID;
                WebWorkContext.Title = admin.Title;
                WebWorkContext.Mobile = admin.Mobile;
                WebWorkContext.Phone = admin.Phone;
                WebWorkContext.UserPwd = admin.UserPwd;
                WebWorkContext.BranchID = admin.BranchID;
                WebWorkContext.TypeID = admin.TypeID;
                WebWorkContext.WorkState = admin.WorkState;
                WebWorkContext.Lat = admin.Lat;
                WebWorkContext.Lng = admin.Lng;
                WebWorkContext.ReDate = admin.ReDate;
                WebWorkContext.Branch = DAL.BD_Branch.GetModel(admin.BranchID);
                WebWorkContext.PageSize = 10;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            if (WebWorkContext.ID <= 0 && String.IsNullOrEmpty(WebWorkContext.Mobile))
            {
                if (WebWorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/WebWorker/Account/Login/");
                return;
            }
        }
        protected ActionResult MessageModel(string code, string content)
        {
            return Content(string.Format("{0}\"code\":\"{1}\",\"msg\":\"{2}\",\"data\":null{3}", "{", code, content, "}"));
        }
        private WorkShop GetUser()
        {
            WorkShop model = null;
            //if (System.Web.HttpContext.Current.Request.Cookies["WebWorkerUserInfo"] == null) return null;
            model = DAL.WorkShop.GetUserByApp(AliceDAL.DataType.ConvertToInt(HttpUtility.UrlDecode(AliceDAL.Common.GetCookie("WebWorkerUserInfo", "ID"))), AliceDAL.Common.GetCookie("WebWorkerUserInfo", "UserPwd"));
            //HttpCookie c = Request.Cookies["WebWorkerUserInfo"];
            //model.ID = int.Parse(HttpUtility.UrlDecode(c.Values["ID"]));
            //model.Title = HttpUtility.UrlDecode(c.Values["Title"]);
            //model.Mobile = HttpUtility.UrlDecode(c.Values["Mobile"]);
            //model.Phone = HttpUtility.UrlDecode(c.Values["Phone"]);
            //model.UserPwd = HttpUtility.UrlDecode(c.Values["UserPwd"]);
            //model.BranchID = int.Parse(c.Values["BranchID"]);
            //model.TypeID = int.Parse(c.Values["TypeID"]);
            //model.WorkState = int.Parse(c.Values["WorkState"]);
            //model.Lat = HttpUtility.UrlDecode(c.Values["Lat"]);
            //model.Lng = HttpUtility.UrlDecode(c.Values["Lng"]);
            //model.ReDate = AliceDAL.DataType.ConvertToDateTime(c.Values["ReDate"]);
            return model;
        }
    }
}