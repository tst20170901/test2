using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using Models;

namespace Web.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index(int id = 1)
        {
            DataTable dt = DAL.PageModel.Table_Model("AB_SinglePage", "ID=" + id.ToString());
            if (dt != null)
            {
                AB_SinglePage model = new AB_SinglePage();
                model.Body = dt.Rows[0]["Body"].ToString();
                model.Description = dt.Rows[0]["Description"].ToString();
                model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                model.Img = dt.Rows[0]["Img"].ToString();
                model.KeyWords = dt.Rows[0]["KeyWords"].ToString();
                model.Title = dt.Rows[0]["Title"].ToString();
                model.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
                SeoInit.InitHead(model.TitleWeb, model.KeyWords, model.Description, this);
                return View(model);
            }
            else
            {
                return View("404");
            }
        }
    }
}
