using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Text;
using Web.Areas.Models;
using System.Data;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class WashItemController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int BranchID = -1, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_Wash_Item]", strsql, page, WorkContext.PageSize, "SortID desc,ID");
            return View(model);
        }
        public ActionResult Add(Wash_Item model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet())
            {
                model.Online = 10;
                model.IsMust = 0;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "项目名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "价格不正确");
                return View(model);
            }
            else if (model.BranID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            Wash_Item wi = new Wash_Item()
            {
                CDate = DateTime.Now,
                IsMust = model.IsMust,
                ItemState = 10,
                Price = model.Price,
                SortID = model.SortID,
                Title = model.Title,
                Online = model.Online,
                BranID = model.BranID,
                WorkPrice = model.WorkPrice
            };
            string s = "线上";
            switch (wi.Online)
            {
                case 10:
                    s = "线上";
                    break;
                case 20:
                    s = "线下";
                    break;
                case 30:
                    s = "通用";
                    break;
            }
            DAL.Wash_Item.Add(wi);
            AddAdminLog("添加服务项目", "添加服务项目,服务项目名称为:" + wi.Title + "（" + s + "）;价格为:" + wi.Price + ";工人提取:" + wi.WorkPrice + ";公司ID为" + wi.BranID);
            return PromptView(Url.Action("List"), "服务项目添加成功");
        }
        public ActionResult Edit(Wash_Item model, int fid = -1)
        {
            LoadBranch();
            Wash_Item wi = DAL.Wash_Item.GetModel(fid);
            if (wi == null) return PromptView("此服务项目不存在");
            if (AliceDAL.Common.IsGet()) return View(wi);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "价格不正确");
                return View(model);
            }
            else if (model.BranID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            Wash_Item item = new Wash_Item()
            {
                CDate = wi.CDate,
                IsMust = model.IsMust,
                ItemState = wi.ItemState,
                Price = model.Price,
                SortID = model.SortID,
                Title = model.Title,
                ID = wi.ID,
                Online = model.Online,
                BranID = model.BranID,
                WorkPrice = model.WorkPrice
            };
            DAL.Wash_Item.Edit(item);
            string s = "线上";
            switch (wi.Online)
            {
                case 10:
                    s = "线上";
                    break;
                case 20:
                    s = "线下";
                    break;
                case 30:
                    s = "通用";
                    break;
            }
            AddAdminLog("修改服务项目", "修改服务项目,服务项目ID为:" + fid + ";原名称为:" + wi.Title + "（" + s + "）;原价格为:" + wi.Price + ";原工人提取:" + wi.WorkPrice + ";现名称为:" + model.Title + "（" + (model.Online == 10 ? "线上" : "线下") + "）;现价格为:" + model.Price + ";现工人提取:" + model.WorkPrice);
            return PromptView(Url.Action("List"), "服务项目修改成功");
        }
        public ActionResult ChangeState(int fid = -1)
        {
            Wash_Item wi = DAL.Wash_Item.GetModel(fid);
            if (wi == null) return PromptView("此服务项目不存在");
            DAL.Wash_Item.ChangeState(fid, wi.ItemState == 0 ? 10 : 0);
            AddAdminLog("服务项目更改状态", (wi.ItemState == 0 ? "启用" : "禁用") + "服务项目,服务项目ID为:" + fid + ";名称为:" + wi.Title + ";价格为:" + wi.Price);
            //return PromptView("服务项目" + (wi.ItemState == 0 ? "启用" : "禁用") + "成功");
            return RedirectToAction("List");
        }
    }
}
