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
    public class NewsController : AdminBaseController
    {
        public ActionResult List(int parentid, string txtTitle, int parid = 0, int page = 1)
        {
            Load(parentid);
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("TypeID<>8");
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (parid > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", parid.ToString()));
            ListModel.CreatePage(model, "Art_Common", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult HotList(string txtTitle, int BranchID = -1, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", "8"));
            ListModel.CreatePage(model, "uv_Art_Common", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        private void Load(int ParentID)
        {
            ViewData["newstypeListchild"] = DAL.Art_Common_Types.GetChildTypesList(ParentID, 1, true);
        }
        public ActionResult Delete(int[] pid)
        {
            DAL.PageModel.Delete("Art_Common", "ID", pid);
            AddAdminLog("删除文章", "删除文章,文章ID为:" + AliceDAL.Common.Ints2String(pid));
            return PromptView("文章删除成功");
        }
        [ValidateInput(false)]
        public ActionResult Add(Art_Common model)
        {
            Load(AliceDAL.UrlParam.GetIntValue("ParentID"));
            if (AliceDAL.Common.IsGet())
            {
                model.Author = WorkContext.NickName;
                model.Source = "本站";
                return View(model);
            }
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
            Art_Common pc = new Art_Common()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hot = 0,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID
            };
            if (new DAL.Art_Common().Add(pc) > 0)
            {
                AddAdminLog("添加文章", "添加文章，文章为：" + pc.Title);
                return PromptView("文章添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "文章添加失败，请稍后再试");
                return View(model);
            }
        }
        public ActionResult HotAdd(Art_Common model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet())
            {
                model.Author = WorkContext.NickName;
                model.Source = "本站";
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "文章标题不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            Art_Common pc = new Art_Common()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hot = model.Hot,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = 8,
                BranchID = model.BranchID
            };
            if (new DAL.Art_Common().Add(pc) > 0)
            {
                AddAdminLog("添加信息", "添加信息，信息为：" + pc.Title);
                return PromptView("信息添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "信息添加失败，请稍后再试");
                return View(model);
            }
        }
        [ValidateInput(false)]
        public ActionResult Edit(Art_Common model, int pid = -1)
        {
            Load(AliceDAL.UrlParam.GetIntValue("ParentID"));
            Art_Common m = new Art_Common();
            System.Data.DataTable dt = DAL.PageModel.Table_Model("Art_Common", String.Format("ID={0}", pid.ToString()));
            if (dt == null) return PromptView("此文章不存在");
            m.Author = dt.Rows[0]["Author"].ToString();
            m.Body = dt.Rows[0]["Body"].ToString();
            m.Description = dt.Rows[0]["Description"].ToString();
            m.Img = dt.Rows[0]["Img"].ToString();
            m.KeyWords = dt.Rows[0]["KeyWords"].ToString();
            m.Source = dt.Rows[0]["Source"].ToString();
            m.Title = dt.Rows[0]["Title"].ToString();
            m.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
            m.TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
            m.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
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
            Art_Common pc = new Art_Common()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hit = 0,
                Hot = 0,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID,
                ID = m.ID
            };
            if (new DAL.Art_Common().Edit(pc) > 0)
            {
                AddAdminLog("修改文章", "修改文章，文章ID为：" + pc.ID);
                return PromptView("/AliceChopper/News/List?ParentID=" + AliceDAL.UrlParam.GetStringValue("ParentID"), "文章修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "文章修改失败，请稍后再试");
                return View(model);
            }
        }

        [ValidateInput(false)]
        public ActionResult HotEdit(Art_Common model, int pid = -1)
        {
            LoadBranch();
            Art_Common m = new Art_Common();
            System.Data.DataTable dt = DAL.PageModel.Table_Model("Art_Common", String.Format("ID={0}", pid.ToString()));
            if (dt == null) return PromptView("此文章不存在");
            m.Author = dt.Rows[0]["Author"].ToString();
            m.Body = dt.Rows[0]["Body"].ToString();
            m.Description = dt.Rows[0]["Description"].ToString();
            m.Img = dt.Rows[0]["Img"].ToString();
            m.KeyWords = dt.Rows[0]["KeyWords"].ToString();
            m.Source = dt.Rows[0]["Source"].ToString();
            m.Title = dt.Rows[0]["Title"].ToString();
            m.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
            m.TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
            m.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
            m.BranchID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BranchID"].ToString());
            m.Hot = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hot"].ToString());
            m.Hit = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hit"].ToString());

            if (AliceDAL.Common.IsGet()) return View(m);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "文章标题不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            Art_Common pc = new Art_Common()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hit = m.Hit,
                Hot = model.Hot,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = 8,
                BranchID = model.BranchID,
                ID = m.ID
            };
            if (new DAL.Art_Common().Edit(pc) > 0)
            {
                AddAdminLog("修改信息", "修改信息，信息ID为：" + pc.ID);
                return PromptView("/AliceChopper/News/HotList?ParentID=8", "信息修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "信息修改失败，请稍后再试");
                return View(model);
            }
        }

    }
}
