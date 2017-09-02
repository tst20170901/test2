using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Models;
namespace Web.Areas.AliceChopper.Controllers
{
    public class CategoryController : AdminBaseController
    {
        public ActionResult List(int bid)
        {
            List<Pro_Types> list = DAL.Pro_Types.GetList("[BusinessID]=" + bid.ToString());
            return View(list);
        }

        public ActionResult Add(Pro_Types model, int bid)
        {
            Load(bid);
            if (AliceDAL.Common.IsGet()) return View(model);
            if (DAL.Pro_Types.GetCateIdByName(model.TypeName) > 0)
            {
                ModelState.AddModelError("TypeName", "名称已经存在");
                return View(model);
            }
            Pro_Types m = new Pro_Types()
            {
                SortID = model.SortID,
                TypeName = model.TypeName,
                ParentID = model.ParentID,
                BusinessID = bid
            };
            DAL.Pro_Types.Add(m);
            AddAdminLog("添加分类", "添加分类,分类为:" + model.TypeName);
            return PromptView("/AliceChopper/Category/List?bid=" + bid.ToString(), "分类添加成功");
        }
        public ActionResult Edit(Pro_Types model, int bid, int tid = -1)
        {
            Load(bid);
            Pro_Types m = DAL.Pro_Types.GetCateIdByID(tid);
            if (m == null) return PromptView("此分类不存在");
            if (AliceDAL.Common.IsGet()) return View(m);

            int cateId2 = DAL.Pro_Types.GetCateIdByName(model.TypeName);
            if (cateId2 > 0 && cateId2 != tid) ModelState.AddModelError("TypeName", "名称已经存在");
            if (model.ParentID == m.ID) ModelState.AddModelError("ParentID", "不能将自己作为父分类");
            if (model.ParentID != 0 && DAL.Pro_Types.GetCateIdByID(model.ParentID) == null) ModelState.AddModelError("ParentID", "父分类不存在");
            if (model.ParentID != 0 && DAL.Pro_Types.GetChildTypesList(m.ID, m.Layer, true).Exists(x => x.ID == model.ParentID)) ModelState.AddModelError("ParentCateId", "不能将分类调整到自己的子分类下");

            if (ModelState.IsValid)
            {
                int oldPar = m.ParentID;
                m.SortID = model.SortID;
                m.TypeName = model.TypeName;
                m.ParentID = model.ParentID;
                DAL.Pro_Types.Edit(m, oldPar);
                AddAdminLog("修改分类", "修改分类,分类为ID:" + model.ID);
                return PromptView("/AliceChopper/Category/List?bid=" + bid.ToString(), "分类修改成功");
            }
            return View(model);
        }

        public ActionResult Delete(int tid = -1)
        {
            int result = DAL.Pro_Types.DeleteByID(tid);
            if (result == -3)
                return PromptView("分类不存在");
            if (result == -2)
                return PromptView("删除失败，请先转移或删除此分类下的子分类");
            if (result == 0)
                return PromptView("删除失败，请先转移或删除此分类下的产品");
            AddAdminLog("删除分类", "删除分类,分类ID为:" + tid);
            return PromptView("分类删除成功");
        }
        private void Load(int bid)
        {
            ViewData["categoryList"] = DAL.Pro_Types.GetList("[BusinessID]=" + bid.ToString());
        }
    }
}
