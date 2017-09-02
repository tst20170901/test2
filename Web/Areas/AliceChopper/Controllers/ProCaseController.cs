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
    public class ProCaseController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int bid, int parentid = 0, int page = 1)
        {
            Load(bid);
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[BusinessID]=" + bid.ToString());
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (parentid > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", parentid.ToString()));
            ListModel.CreatePage(model, "uv_Pro_Case", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        private void Load(int bid)
        {
            ViewData["categoryList"] = DAL.Pro_Types.GetList("[BusinessID]=" + bid.ToString());
        }
        public ActionResult Delete(int[] pid)
        {
            DAL.PageModel.Delete("Pro_Case", "ID", pid);
            AddAdminLog("删除产品", "删除产品,产品ID为:" + AliceDAL.Common.Ints2String(pid));
            return PromptView("产品删除成功");
        }
        [ValidateInput(false)]
        public ActionResult Add(Pro_Case model, int bid)
        {
            Load(bid);
            if (AliceDAL.Common.IsGet())
            {
                model.Author = WorkContext.NickName;
                model.Source = "本站";
                model.ProType = 0;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "产品标题不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("TypeID", "请选择分类");
                return View(model);
            }
            Pro_Case pc = new Pro_Case()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Price = model.Price,
                Price1 = model.Price1,
                BranchID = WorkContext.BranchID,
                SortID = model.SortID,
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
                BusinessID = bid,
                ProType = model.ProType
            };
            if (DAL.Pro_Case.Add(pc) > 0)
            {
                AddAdminLog("添加产品", "添加产品，产品为：" + pc.Title);
                return PromptView("产品添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "产品添加失败，请稍后再试");
                return View(model);
            }
        }

        [ValidateInput(false)]
        public ActionResult Edit(Pro_Case model, int bid, int pid = -1)
        {
            Load(bid);
            Pro_Case m = new Pro_Case();
            System.Data.DataTable dt = DAL.PageModel.Table_Model("Pro_Case", String.Format("ID={0}", pid.ToString()));
            if (dt == null) return PromptView("此产品不存在");
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
            m.SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
            m.Price = AliceDAL.DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
            m.Price1 = AliceDAL.DataType.ConvertToDecimal(dt.Rows[0]["Price1"].ToString());
            m.BusinessID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString());
            m.ProType = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ProType"].ToString());
            if (AliceDAL.Common.IsGet()) return View(m);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "产品标题不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("TypeID", "请选择分类");
                return View(model);
            }
            Pro_Case pc = new Pro_Case()
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
                Price = model.Price,
                Price1 = model.Price1,
                BranchID = WorkContext.BranchID,
                SortID = model.SortID,
                ID = m.ID,
                BusinessID = m.BusinessID,
                ProType = model.ProType
            };
            if (DAL.Pro_Case.Edit(pc) > 0)
            {
                AddAdminLog("修改产品", "修改产品，产品ID为：" + pc.ID);
                return PromptView("/AliceChopper/ProCase/List?bid=" + bid, "产品修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "产品修改失败，请稍后再试");
                return View(model);
            }
        }
        public ActionResult ProChangeState(int bid, int cid = -1, int page = 1)
        {
            Pro_Case pc = DAL.Pro_Case.GetModel(cid);
            if (pc == null) return PromptView("此商品不存在");
            DAL.Pro_Case.ChangeState(cid, pc.ProState == 30 ? 10 : 30);
            AddAdminLog("商品状态", (pc.ProState == 30 ? "上架" : "下架") + "商品,商品ID为:" + cid + ";名称为:" + pc.Title);
            return Redirect(Url.Action("List", new { page = page.ToString(), bid = bid }));
        }
    }
}
