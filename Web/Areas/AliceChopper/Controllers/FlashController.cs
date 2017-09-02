using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Data;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class FlashController : AdminBaseController
    {
        public ActionResult TypeList(FlashType model, int tid = -1, string act = "list")
        {
            switch (act)
            {
                case "list":
                    ViewData["list"] = DAL.PageModel.DateList("FlashType", "", "SortID", 0);
                    if (AliceDAL.Common.IsGet()) return View(model);
                    if (String.IsNullOrEmpty(model.TypeName))
                    {
                        ModelState.AddModelError("TypeName", "名称不能为空");
                        return View(model);
                    }
                    string result = new DAL.FlashType().Add(model);
                    if ("1" == result)
                    {
                        ModelState.AddModelError("Msg", "添加成功");
                        ViewData["list"] = DAL.PageModel.DateList("FlashType", "", "SortID", 0);
                        return View();
                    }
                    else if ("-2" == result)
                    {
                        ModelState.AddModelError("Error", "名称已经存在");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "添加失败，请稍候再试");
                        return View(model);
                    }
                case "edit":
                    ViewData["list"] = DAL.PageModel.DateList("FlashType", "", "SortID", 0);
                    DataTable dt = DAL.PageModel.Table_Model("FlashType", "ID=" + tid.ToString());
                    if (dt == null) return PromptView("此分类不存在");
                    FlashType m = new FlashType()
                    {
                        ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString()),
                        ParentID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ParentID"].ToString()),
                        SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString()),
                        TypeName = dt.Rows[0]["TypeName"].ToString()
                    };
                    if (AliceDAL.Common.IsGet()) return View(m);

                    model.ID = m.ID;
                    model.ParentID = m.ParentID;
                    string res = new DAL.FlashType().Edit(model);
                    if ("1" == res)
                    {
                        return PromptView("/AliceChopper/Flash/TypeList", "修改成功！");
                    }
                    else if ("-2" == res)
                    {
                        ModelState.AddModelError("Error", "名称已经存在");
                        return View(m);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "修改失败，请稍候再试");
                        return View(m);
                    }
                case "del":
                    if (new DAL.FlashType().Delete(tid))
                    {
                        return PromptView("/AliceChopper/Flash/TypeList", "删除成功！");
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

        public ActionResult FlashAdd(Flash model)
        {
            Load();
            if (AliceDAL.Common.IsGet()) return View(model);

            Flash m = new Flash()
            {
                Img = model.Img ?? "/Content/images/noimage.jpg",
                Title = model.Title,
                TypeID = model.TypeID,
                Url = model.Url,
                SortID = model.SortID
            };
            new DAL.Flash().Add(m);
            AddAdminLog("添加幻灯片", "添加幻灯片,图片为:" + m.Img);
            return PromptView("添加成功");
        }

        public ActionResult Edit(Flash model, int tid = -1)
        {
            Load();
            DataTable dt = DAL.PageModel.Table_Model("Flash", "ID=" + tid.ToString());
            if (dt == null) return PromptView("此幻灯片不存在");
            Flash m = new Flash();
            m.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
            m.Img = dt.Rows[0]["Img"].ToString();
            m.SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
            m.Title = dt.Rows[0]["Title"].ToString();
            m.TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
            m.Url = dt.Rows[0]["Url"].ToString();
            if (AliceDAL.Common.IsGet()) return View(m);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "文章标题不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("TypeID", "请选择分类");
                return View(model);
            }
            model.ID = m.ID;
            if (new DAL.Flash().Edit(model))
            {
                AddAdminLog("修改幻灯片", "修改幻灯片,幻灯片ID:" + model.ID);
                return PromptView("/AliceChopper/Flash/FlashList", "分类修改成功");
            }
            else
            {
                return PromptView("幻灯片修改失败，请稍后再试");
            }
        }

        public ActionResult FlashList()
        {
            DataTable dt = DAL.PageModel.DateList("Flash", "", "SortID", 0);
            return View(dt);
        }

        public ActionResult Delete(int tid = -1)
        {
            if (DAL.PageModel.Delete("Flash", "ID=" + tid.ToString()) > 0)
            {
                AddAdminLog("删除幻灯片", "删除幻灯片,幻灯片ID为:" + tid);
                return PromptView("删除成功");
            }
            else
            {
                return PromptView("删除失败，请稍后再试");
            }
        }
        private void Load()
        {
            ViewData["flashtypeList"] = DAL.PageModel.DateList("FlashType", "ParentID=0", "SortID", 0);
        }
    }
}
