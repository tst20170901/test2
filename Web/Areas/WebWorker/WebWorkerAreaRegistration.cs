using System.Web.Mvc;

namespace Web.Areas.WebWorker.Controllers
{
    public class WebWorkerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebWorker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("WebWorker_default", "WebWorker/{controller}/{action}/{id}", new { controller = "UsersAdmin", action = "Index", area = "WebWorker", id = UrlParameter.Optional }, new { area = "WebWorker" }, new string[] { "Web.Areas.WebWorker.Controllers" });
        }
    }
}
