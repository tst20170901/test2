using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("MapNews", "{map}/xiaoxiong/News/{title}/{id}", new { controller = "News", action = "ShowNews" }, new { id = @"\d+" }, new string[] { "Web.Controllers" });
            routes.MapRoute("News", "News/{title}/{id}", new { controller = "News", action = "ShowNews" }, new { id = @"\d+" }, new string[] { "Web.Controllers" });
            routes.MapRoute("MapArea", "{map}/xiaoxiong/{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new string[] { "Web.Controllers" });
            routes.MapRoute("PageSize", "{controller}/{action}/{pagesize}/{pageindex}", new { pageindex = UrlParameter.Optional }, new { action = "PageSize" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new string[] { "Web.Controllers" });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AliceDAL.CacheManager.Clear();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }
    }
}