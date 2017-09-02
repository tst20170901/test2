using System.Web.Mvc;

namespace Web.Areas.UsersManger.Controllers
{
    public class UsersMangerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UsersManger";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("UsersManger_default", "UsersManger/{controller}/{action}/{id}", new { controller = "UsersAdmin", action = "Index", area = "UsersManger", id = UrlParameter.Optional }, new { area = "UsersManger" }, new string[] { "Web.Areas.UsersManger.Controllers" });
        }
    }
}
