using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Data;
using System.Web.Routing;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class LinksController : AdminBaseController
    {
        public ActionResult List(Links model, int tid = -1, string act = "list")
        {
            switch (act)
            {
                case "list":
                    ViewData["list"] = DAL.PageModel.DateList("Links", "", "SortID", 0);
                    if (AliceDAL.Common.IsGet()) return View(model);
                    if (String.IsNullOrEmpty(model.Title))
                    {
                        ModelState.AddModelError("Title", "链接名称不能为空");
                        return View(model);
                    }
                    model.Img = "";
                    if (new DAL.Links().Add(model) > 0)
                    {
                        AddAdminLog("添加友情链接", "添加友情链接,友情链接名称为:" + model.Title + ";地址是:" + model.Url);
                        ModelState.AddModelError("Msg", "添加成功");
                        ViewData["list"] = DAL.PageModel.DateList("Links", "", "SortID", 0);
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "添加失败，请稍候再试");
                        return View(model);
                    }
                case "edit":
                    ViewData["list"] = DAL.PageModel.DateList("Links", "", "SortID", 0);
                    DataTable dt = DAL.PageModel.Table_Model("Links", "ID=" + tid.ToString());
                    if (dt == null) return PromptView("此链接不存在");
                    Links m = new Links()
                    {
                        ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString()),
                        Img = dt.Rows[0]["Img"].ToString(),
                        LinkType = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["LinkType"].ToString()),
                        SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString()),
                        Title = dt.Rows[0]["Title"].ToString(),
                        Url = dt.Rows[0]["Url"].ToString()
                    };
                    if (AliceDAL.Common.IsGet()) return View(m);

                    model.ID = m.ID;
                    model.Img = m.Img;
                    if (new DAL.Links().Edit(model))
                    {
                        AddAdminLog("修改友情链接", "修改友情链接,友情链接原名称为:" + m.Title + ";原地址是:" + m.Url + ";现名称:" + model.Title + ";现地址是:" + model.Url);
                        return PromptView(Url.Action("List"), "修改成功！");
                    }
                    {
                        ModelState.AddModelError("Error", "修改失败，请稍候再试");
                        return View(m);
                    }
                case "del":
                    if (DAL.PageModel.Delete("Links", "ID=" + tid.ToString()) > 0)
                    {
                        return PromptView(Url.Action("List"), "删除成功！");
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "删除失败，请稍候再试");
                        return View();
                    }
                default:
                    return View();
            }
        }
    }
}
