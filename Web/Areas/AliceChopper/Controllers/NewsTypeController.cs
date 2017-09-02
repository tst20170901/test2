using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class NewsTypeController : AdminBaseController
    {
        public ActionResult List(int parentid = 0)
        {
            List<Art_Common_Types> list = DAL.Art_Common_Types.GetChildTypesList(parentid, 1, true);
            return View(list);
        }

        public ActionResult Add(Art_Common_Types model)
        {
            Load(AliceDAL.UrlParam.GetIntValue("ParentID"));
            if (AliceDAL.Common.IsGet()) return View(model);
            if (DAL.Art_Common_Types.GetCateIdByName(model.TypeName) > 0)
            {
                ModelState.AddModelError("TypeName", "名称已经存在");
                return View(model);
            }
            Art_Common_Types m = new Art_Common_Types()
            {
                SortID = model.SortID,
                TypeName = model.TypeName,
                ParentID = model.ParentID == 0 ? AliceDAL.UrlParam.GetIntValue("ParentID") : model.ParentID
            };
            DAL.Art_Common_Types.Add(m);
            AddAdminLog("添加文章分类", "添加文章分类,分类为:" + model.TypeName);
            return PromptView("/AliceChopper/NewsType/List?ParentID=" + AliceDAL.UrlParam.GetStringValue("ParentID"), "分类添加成功");
        }
        public ActionResult Edit(Art_Common_Types model, int tid = -1)
        {
            Load(AliceDAL.UrlParam.GetIntValue("ParentID"));
            Art_Common_Types m = DAL.Art_Common_Types.GetCateIdByID(tid);
            if (m == null) return PromptView("此分类不存在");
            if (AliceDAL.Common.IsGet()) return View(m);

            int cateId2 = DAL.Art_Common_Types.GetCateIdByName(model.TypeName);
            if (cateId2 > 0 && cateId2 != tid) ModelState.AddModelError("TypeName", "名称已经存在");
            if (model.ParentID == m.ID) ModelState.AddModelError("ParentID", "不能将自己作为父分类");
            if (model.ParentID != 0 && DAL.Art_Common_Types.GetCateIdByID(model.ParentID) == null) ModelState.AddModelError("ParentID", "父分类不存在");
            if (model.ParentID != 0 && DAL.Art_Common_Types.GetChildTypesList(m.ID, m.Layer, true).Exists(x => x.ID == model.ParentID)) ModelState.AddModelError("ParentCateId", "不能将分类调整到自己的子分类下");

            if (ModelState.IsValid)
            {
                int oldPar = m.ParentID;
                m.SortID = model.SortID;
                m.TypeName = model.TypeName;
                m.ParentID = model.ParentID == 0 ? AliceDAL.UrlParam.GetIntValue("ParentID") : model.ParentID;
                DAL.Art_Common_Types.Edit(m, oldPar);
                AddAdminLog("修改文章分类", "修改文章分类,分类为ID:" + model.ID);
                return PromptView("/AliceChopper/NewsType/List?ParentID=" + AliceDAL.UrlParam.GetStringValue("ParentID"), "分类修改成功");
            }
            return View(model);
        }

        public ActionResult Delete(int tid = -1)
        {
            int result = DAL.Art_Common_Types.DeleteByID(tid);
            if (result == -3)
                return PromptView("分类不存在");
            if (result == -2)
                return PromptView("删除失败，请先转移或删除此分类下的子分类");
            if (result == 0)
                return PromptView("删除失败，请先转移或删除此分类下的文章");
            AddAdminLog("删除文章分类", "删除文章分类,分类ID为:" + tid);
            return PromptView("文章分类删除成功");
        }
        private void Load(int ParentID)
        {
            ViewData["newstypeListchild"] = DAL.Art_Common_Types.GetChildTypesList(ParentID, 1, true);
        }
    }
}
