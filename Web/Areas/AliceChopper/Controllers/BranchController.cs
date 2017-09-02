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
    public class BranchController : AdminBaseController
    {
        public ActionResult List(string txtTitle, string pcregion, string rdoState, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[BD_Branch]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult Add(BD_Branch model)
        {
            if (AliceDAL.Common.IsGet())
            {
                model.OrderCount = 10;
                return View(model);
            }

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.Points) || model.Points.Split(';').Length < 3 || String.IsNullOrEmpty(model.CenterLng) || String.IsNullOrEmpty(model.CenterLat))
            {
                ModelState.AddModelError("Title", "请重新勾画区域");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.CenterLng, model.CenterLat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "请重新勾画区域");
                return View(model);
            }
            model.Adcode = am.AdCode;
            model.City = am.City;
            model.CityCode = am.CityCode;
            model.District = am.District;
            model.Province = am.Province;
            model.Mobile = model.Mobile ?? "";
            model.StateMsg = model.StateMsg ?? "";
            model.OrderCount = model.OrderCount <= 0 ? 10 : model.OrderCount;
            string result = DAL.BD_Branch.Add(model);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else if (result == "error")
            {
                ModelState.AddModelError("Title", "系统错误");
                return View(model);
            }
            else
            {
                AddAdminLog("添加分公司", "添加分公司,分公司名称为:" + model.Title + ";详细地址是:" + model.District);
                return PromptView(Url.Action("List"), "添加成功");
            }
        }
        public ActionResult Edit(BD_Branch model, int fid = -1)
        {
            BD_Branch bb = DAL.BD_Branch.GetModel(fid);
            if (bb == null) return PromptView("此分公司不存在");
            if (AliceDAL.Common.IsGet()) return View(bb);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.Points) || model.Points.Split(';').Length < 3 || String.IsNullOrEmpty(model.CenterLng) || String.IsNullOrEmpty(model.CenterLat))
            {
                ModelState.AddModelError("Title", "请重新勾画区域");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.CenterLng, model.CenterLat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "请重新勾画区域");
                return View(model);
            }

            model.ID = bb.ID;
            model.Adcode = am.AdCode;
            model.City = am.City;
            model.CityCode = am.CityCode;
            model.District = am.District;
            model.Province = am.Province;
            model.Mobile = model.Mobile ?? "";
            model.StateMsg = model.StateMsg ?? "";
            model.OrderCount = model.OrderCount <= 0 ? 10 : model.OrderCount;
            string result = DAL.BD_Branch.Edit(model);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else if (result == "error")
            {
                ModelState.AddModelError("Title", "系统错误");
                return View(model);
            }
            else
            {
                AddAdminLog("修改分公司", "添加分公司,分公司名称为:" + model.Title + ";详细地址是:" + model.District);
                return PromptView(Url.Action("List"), "修改成功");
            }
        }
        public ActionResult ShowModel(int fid = -1)
        {
            BD_Branch bb = DAL.BD_Branch.GetModel(fid);
            if (bb == null) return Content("此分公司不存在");
            return View(bb);
        }
        public ActionResult ShowAll()
        {
            DataTable dt = DAL.PageModel.DataKeysList("[BD_Branch]", "[ID],[Title],[Points],[CenterLng],[CenterLat]", "", "ID", 0);
            return View(dt);
        }
        public ActionResult ChangeState(int bid = -1)
        {
            BD_Branch bb = DAL.BD_Branch.GetModel(bid);
            if (bb == null) return PromptView("此分公司不存在");
            DAL.BD_Branch.ChangeBranchState(bid, bb.BranchState == 0 ? 10 : 0);
            AddAdminLog("分公司更改状态", (bb.BranchState == 0 ? "启用" : "禁用") + "分公司,分公司ID为:" + bid + ";名称为:" + bb.Title);
            return RedirectToAction("List");
        }
    }
}
