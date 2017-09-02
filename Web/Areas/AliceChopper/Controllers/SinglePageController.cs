using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class SinglePageController : AdminBaseController
    {
        [ValidateInput(false)]
        public ActionResult Add(AB_SinglePage model)
        {
            if (AliceDAL.Common.IsGet()) return View(model);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "标题不能为空");
                return View(model);
            }
            AB_SinglePage pc = new AB_SinglePage()
            {
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Title = model.Title ?? "",
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID,
                Lan = model.Lan,
                SortID = model.SortID
            };
            if (new DAL.AB_SinglePage().Add(pc) > 0)
            {
                AddAdminLog("添加单页", "添加单页，单页为：" + pc.Title);
                return PromptView("单页添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "单页添加失败，请稍后再试");
                return View(model);
            }
        }

        [ValidateInput(false)]
        public ActionResult Edit(AB_SinglePage model, string tid)
        {
            AB_SinglePage m = new AB_SinglePage();
            System.Data.DataTable dt = DAL.PageModel.Table_Model("AB_SinglePage", String.Format("TypeID={0}", tid));
            if (dt == null) return PromptView("此单页不存在");
            m.Body = dt.Rows[0]["Body"].ToString();
            m.Description = dt.Rows[0]["Description"].ToString();
            m.Img = dt.Rows[0]["Img"].ToString();
            m.KeyWords = dt.Rows[0]["KeyWords"].ToString();
            m.Title = dt.Rows[0]["Title"].ToString();
            m.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
            m.TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
            m.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
            m.SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
            if (AliceDAL.Common.IsGet()) return View(m);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "单页标题不能为空");
                return View(model);
            }
            AB_SinglePage pc = new AB_SinglePage()
            {
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Title = model.Title ?? "",
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID,
                SortID = model.SortID,
                ID = m.ID
            };
            if (new DAL.AB_SinglePage().Edit(pc))
            {
                AddAdminLog("修改单页", "修改单页，单页ID为：" + pc.ID);
                return PromptView(Url.Action("List"), "单页修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "单页修改失败，请稍后再试");
                return View(model);
            }
        }
        public ActionResult List(int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            ListModel.CreatePage(model, "AB_SinglePage", strsql, page, WorkContext.PageSize,"SortID asc,ID");
            return View(model);
        }

    }
}
