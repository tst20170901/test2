using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using System.Text;
using Web.Models;
using Models;

namespace Web.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index(int nid = 2, int page = 1)
        {
            Art_Common_Types model = DAL.Art_Common_Types.GetCateIdByID(nid);
            if (model != null)
            {
                Models.ListModel listmodel = new Models.ListModel();
                StringBuilder strsql = new StringBuilder(String.Format("TypeID={0}", model.ID));
                ListModel.CreatePage(listmodel, "Art_Common", strsql, page, 15);
                listmodel.DataType = model;
                SeoInit.InitHead(model.TypeName + "-" + AliceDAL.IniHelper.GetValue("WebInfo", "WebName"), "", "", this);
                return View(listmodel);
            }
            else
            {
                return View("404");
            }
        }
        public ActionResult ShowNews(int id)
        {
            DataTable dt = DAL.PageModel.Table_Model("Art_Common", String.Format("ID={0}", id.ToString()));
            if (dt != null)
            {
                Art_Common model = new Art_Common()
                {
                    Author = dt.Rows[0]["Author"].ToString(),
                    Body = dt.Rows[0]["Body"].ToString(),
                    CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString()),
                    Description = dt.Rows[0]["Description"].ToString(),
                    Hit = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hit"].ToString()),
                    Hot = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hot"].ToString()),
                    KeyWords = dt.Rows[0]["KeyWords"].ToString(),
                    Img = dt.Rows[0]["Img"].ToString(),
                    Source = dt.Rows[0]["Source"].ToString(),
                    Title = dt.Rows[0]["Title"].ToString(),
                    TitleSpell = dt.Rows[0]["TitleSpell"].ToString(),
                    TitleWeb = dt.Rows[0]["TitleWeb"].ToString(),
                    TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString()),
                    ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString())
                };
                Art_Common_Types act = DAL.Art_Common_Types.GetCateIdByID(model.TypeID);
                ViewBag.ID = act.ID;
                ViewBag.TypeName = act.TypeName;
                SeoInit.InitHead(String.IsNullOrWhiteSpace(model.TitleWeb) ? model.Title : model.TitleWeb, model.KeyWords, model.Description, this);
                DAL.PageModel.Hit("Art_Common", "Hit=Hit+1", "ID=" + model.ID.ToString());
                return View(model);
            }
            else return View("404");
        }
    }
}
