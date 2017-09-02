using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using AliceDAL;
using Models;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SeoInit.InitHead("", "", "", this);
            return View();
        }
        public ActionResult Temp()
        {
            //string base64 = @"";
            //string s = ImageBase64Helper.Base64StringToImage(base64).Replace("\\n", "\n");
            return Content("看什么看没见过测试啊");
        }
    }
}