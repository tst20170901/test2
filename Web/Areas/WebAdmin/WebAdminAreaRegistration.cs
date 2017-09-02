using System.Web.Mvc;

namespace Web.Areas.WebAdmin.Controllers
{
    public class WebAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("WebAdmin_default", "WebAdmin/{controller}/{action}/{id}", new { controller = "UsersAdmin", action = "Index", area = "WebAdmin", id = UrlParameter.Optional }, new { area = "WebAdmin" }, new string[] { "Web.Areas.WebAdmin.Controllers" });
        }
    }
}
