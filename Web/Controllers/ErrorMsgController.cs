using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorMsgController : Controller
    {
        public ActionResult Index()
        {
            return View("404");
        }

    }
}
