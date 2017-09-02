using System.Web.Mvc;

namespace Web.Areas.app.Controllers
{
    public class appAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "app";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("app_default", "app/{controller}/{action}/{id}", new { controller = "AliceAdmin", action = "Index", area = "app", id = UrlParameter.Optional }, new { area = "app" }, new string[] { "Web.Areas.app.Controllers" });
        }
    }
}
