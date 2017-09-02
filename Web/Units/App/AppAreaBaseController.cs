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
    public class AppAreaBaseController : Controller
    {
        public AppContext WorkContext = new AppContext();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.Title = AliceDAL.IniHelper.GetValue("WebInfo", "WebName");
            WorkContext.IsHttpAjax = AliceDAL.Common.IsAjax();
            WorkContext.IP = AliceDAL.Common.GetIP();
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction) return;
            object s = filterContext.RouteData.DataTokens["area"] ?? "";
            if (s == null || String.IsNullOrEmpty(s.ToString()))
            {
                if (AliceDAL.Common.IsAjax())
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/app/Account/Login/");
                return;
            }
        }
        public JsonResult AjaxResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}