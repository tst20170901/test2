using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Models;
namespace Web.Areas.AliceChopper.Controllers
{
    public class BusiTypeController : AdminBaseController
    {
        public ActionResult List()
        {
            List<Business_Types> list = DAL.Business_Types.List();
            return View(list);
        }

        public ActionResult Add(Business_Types model)
        {
            Load();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (DAL.Business_Types.GetCateIdByName(model.TypeName) > 0)
            {
                ModelState.AddModelError("TypeName", "名称已经存在");
                return View(model);
            }
            Business_Types m = new Business_Types()
            {
                SortID = model.SortID,
                TypeName = model.TypeName,
                ParentID = model.ParentID,
                Img = model.Img ?? ""
            };
            DAL.Business_Types.Add(m);
            AddAdminLog("添加分类", "添加分类,分类为:" + model.TypeName);
            return PromptView("/AliceChopper/BusiType/List", "分类添加成功");
        }
        public ActionResult Edit(Business_Types model, int tid = -1)
        {
            Load();
            Business_Types m = DAL.Business_Types.GetCateIdByID(tid);
            if (m == null) return PromptView("此分类不存在");
            if (AliceDAL.Common.IsGet()) return View(m);

            int cateId2 = DAL.Business_Types.GetCateIdByName(model.TypeName);
            if (cateId2 > 0 && cateId2 != tid) ModelState.AddModelError("TypeName", "名称已经存在");
            if (model.ParentID == m.ID) ModelState.AddModelError("ParentID", "不能将自己作为父分类");
            if (model.ParentID != 0 && DAL.Business_Types.GetCateIdByID(model.ParentID) == null) ModelState.AddModelError("ParentID", "父分类不存在");
            if (model.ParentID != 0 && DAL.Business_Types.GetChildTypesList(m.ID, m.Layer, true).Exists(x => x.ID == model.ParentID)) ModelState.AddModelError("ParentCateId", "不能将分类调整到自己的子分类下");

            if (ModelState.IsValid)
            {
                int oldPar = m.ParentID;
                m.SortID = model.SortID;
                m.TypeName = model.TypeName;
                m.ParentID = model.ParentID;
                m.Img = model.Img ?? "";
                DAL.Business_Types.Edit(m, oldPar);
                AddAdminLog("修改分类", "修改分类,分类为ID:" + model.ID);
                return PromptView("/AliceChopper/BusiType/List", "分类修改成功");
            }
            return View(model);
        }

        public ActionResult Delete(int tid = -1)
        {
            int result = DAL.Business_Types.DeleteByID(tid);
            if (result == -3)
                return PromptView("分类不存在");
            if (result == -2)
                return PromptView("删除失败，请先转移或删除此分类下的子分类");
            AddAdminLog("删除分类", "删除分类,分类ID为:" + tid);
            return PromptView("分类删除成功");
        }
        private void Load()
        {
            ViewData["busitypeList"] = DAL.Business_Types.List();
        }
    }
}
