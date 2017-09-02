using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AliceDAL;
using Models;

namespace Web.Units
{
    public class WebAdminAreaBaseController : Controller
    {
        public WebAdminContext WebAdminContext = new WebAdminContext();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.Title = AliceDAL.IniHelper.GetValue("WebInfo", "WebName");
            WebAdminContext.IsHttpAjax = AliceDAL.Common.IsAjax();
            WebAdminContext.IP = AliceDAL.Common.GetIP();
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction) return;
            object s = filterContext.RouteData.DataTokens["area"] ?? "";
            if (s == null || String.IsNullOrEmpty(s.ToString()))
            {
                if (AliceDAL.Common.IsAjax()) filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else filterContext.Result = new RedirectResult("/WebAdmin/Account/Login/");
                return;
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            AliceDAL.Error.WriteErrorLog(filterContext.Exception);
            //filterContext.HttpContext.Response.Redirect("http://www.baidu.com");
        }
        public JsonResult AjaxResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}