using System.Web.Mvc;

namespace Web.Areas.BusinessManger.Controllers
{
    public class BusinessMangerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BusinessManger";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("BusinessManger_default", "BusinessManger/{controller}/{action}/{id}", new { controller = "UsersAdmin", action = "Index", area = "BusinessManger", id = UrlParameter.Optional }, new { area = "BusinessManger" }, new string[] { "Web.Areas.BusinessManger.Controllers" });
        }
    }
}
