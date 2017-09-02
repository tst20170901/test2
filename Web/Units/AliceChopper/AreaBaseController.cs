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
    public class AreaBaseController : Controller
    {
        public AdminWorkContext WorkContext = new AdminWorkContext();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.Title = AliceDAL.IniHelper.GetValue("WebInfo", "WebName");
            WorkContext.IsHttpAjax = AliceDAL.Common.IsAjax();
            WorkContext.IP = AliceDAL.Common.GetIP();
            WorkContext.Url = AliceDAL.Common.GetUrl();
            WorkContext.UrlReferrer = AliceDAL.Common.GetUrlReferrer();
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction) return;
            object s = filterContext.RouteData.DataTokens["area"] ?? "";
            if (s == null || String.IsNullOrEmpty(s.ToString()))
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/AliceChopper/AliceChopper/Login/");
                return;
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            AliceDAL.Error.WriteErrorLog(filterContext.Exception);
            if (WorkContext.IsHttpAjax)
                filterContext.Result = AjaxResult(new { state = "error", msg = "系统错误,请联系管理员" });
            else
                filterContext.Result = new ViewResult() { ViewName = "error" };
        }
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(AliceDAL.Common.GetUrlReferrer(), message));
        }
        public JsonResult AjaxResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
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
        protected void AddAdminLog(string operation)
        {
            AddAdminLog(operation, "");
        }
        protected void AddAdminLog(string operation, string description)
        {
            BD_Logs model = new BD_Logs();
            model.Description = description;
            model.IP = WorkContext.IP;
            model.LoginID = WorkContext.LoginID;
            model.NickName = WorkContext.NickName + "(" + WorkContext.Branch.Title + ")";
            DAL.BD_Logs.Add(model);
        }
    }
}