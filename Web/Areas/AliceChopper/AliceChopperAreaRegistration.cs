using System.Web.Mvc;

namespace Web.Areas.AliceChopper.Controllers
{
    public class AliceChopperAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AliceChopper";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("AliceChopper_default", "AliceChopper/{controller}/{action}/{id}", new { controller = "AliceAdmin", action = "Index", area = "AliceChopper", id = UrlParameter.Optional }, new { area = "AliceChopper" }, new string[] { "Web.Areas.AliceChopper.Controllers" });
        }
    }
}
