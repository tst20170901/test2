using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using AliceDAL;
using Models;
using System.Text;
namespace Web.Controllers
{
    public class SiteMapController : Controller
    {
        public ActionResult Index()
        {
            SeoInit.InitHead("", "", "", this);
            ViewBag.Citys = InitMap();
            return View();
        }
        [NonAction]
        public string InitMap()
        {
            string result = CacheManager.Get(CacheKeys.CITYSTR) as string;
            if (result == null)
            {
                string[] s = { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "W", "X", "Y", "Z" };
                string[] hot = AliceDAL.IniHelper.GetValue("WebInfo", "HotCity").Split(',');
                Dictionary<string, string> dic = AliceDAL.CityData.GetCityPY();
                StringBuilder sb = new StringBuilder();
                foreach (string item in s)
                {
                    sb.Append("<dt>" + item + "</dt><dd>");
                    foreach (string key in dic.Keys)
                    {
                        if (key.StartsWith(item.ToLower()))
                        {
                            if (hot.Contains(dic[key])) sb.Append("<a href=\"" + Url.Action("Index", "Home", new { @map = key }) + "\" class=\"redLink\">" + dic[key] + "</a>");
                            else sb.Append("<a href=\"" + Url.Action("Index", "Home", new { @map = key }) + "\">" + dic[key] + "</a>");
                        }
                    }
                    sb.Append("</dd>");
                }
                result = sb.ToString();
                CacheManager.Insert(CacheKeys.CITYSTR, result);
            }
            return result;
        }
    }
}