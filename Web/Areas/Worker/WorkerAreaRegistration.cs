using System.Web.Mvc;

namespace Web.Areas.Worker.Controllers
{
    public class WorkerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Worker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Worker_default", "Worker/{controller}/{action}/{id}", new { controller = "AliceAdmin", action = "Index", area = "Worker", id = UrlParameter.Optional }, new { area = "Worker" }, new string[] { "Web.Areas.Worker.Controllers" });
        }
    }
}
