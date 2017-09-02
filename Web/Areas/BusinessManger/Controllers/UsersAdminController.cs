using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Web.Areas.Models;
using System.Text;
using System.IO;
using Models;
using System.Data;

namespace Web.Areas.BusinessManger.Controllers
{
    public class UsersAdminController : BusinessBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditInfo(BD_Business model)
        {
            ViewData["businessTypeList"] = DAL.Business_Types.List();
            BD_Business bb = DAL.BD_Business.GetModel(WorkContext.ID);
            if (bb == null) return PromptView("此商家不存在");
            if (AliceDAL.Common.IsGet()) return View(bb);

            if (String.IsNullOrEmpty(model.BusinessName))
            {
                ModelState.AddModelError("BusinessName", "商家名称不能为空");
                return View(model);
            }
            BD_Business mm = new BD_Business()
            {
                BusinessName = model.BusinessName,
                SortID = bb.SortID,
                BranchID = bb.BranchID,
                ID = bb.ID,
                Data1 = model.Data1 ?? "",
                TypeID = model.TypeID
            };
            if (!String.IsNullOrWhiteSpace(model.Password)) mm.Password = AliceDAL.SecureHelper.MD5(model.Password);
            else mm.Password = bb.Password;
            mm.Data2 = mm.Data3 = mm.Data4 = mm.Data5 = mm.Data6 = mm.Data7 = mm.Data8 = mm.Data9 = mm.Data10 = "";
            string result = DAL.BD_Business.Edit(mm);
            if (result == "-2")
            {
                ModelState.AddModelError("BusinessName", "商家名称已存在");
                return View(model);
            }
            else
            {
                //AddAdminLog("商家更改", "商家ID为:" + bid + ";名称为:" + model.BusinessName + ";原名称为:" + bb.BusinessName);
                return PromptView("商家修改成功");
            }
        }
        #region 修改密码
        public ActionResult EditPassword()
        {
            if (AliceDAL.Common.IsGet()) return View();

            string password = AliceDAL.Common.GetFormString("password");
            string newpassword = AliceDAL.Common.GetFormString("newpassword");
            string repassword = AliceDAL.Common.GetFormString("repassword");

            if (AliceDAL.Common.cutBadStr(password) == "")
            {
                return PromptView("请填写旧密码！");
            }
            else if (AliceDAL.Common.cutBadStr(newpassword) == "")
            {
                return PromptView("请填写新密码！");

            }
            else if (AliceDAL.Common.cutBadStr(repassword) == "")
            {
                return PromptView("请确认新密码！");
            }
            else if (repassword != newpassword)
            {
                return PromptView("两次密码不一致，请重新填写！");
            }
            if (AliceDAL.SecureHelper.MD5(password) != WorkContext.Password)
            {
                return PromptView("旧密码不正确！");
            }
            else
            {
                int result = DAL.BD_Users.EditPwd(WorkContext.LoginID, AliceDAL.SecureHelper.MD5(newpassword));
                if (result > 0)
                {

                    AliceDAL.Common.SetCookie("UserLoginInfo", "LoginID", WorkContext.LoginID);
                    AliceDAL.Common.SetCookie("UserLoginInfo", "UserPwd", AliceDAL.SecureHelper.MD5(newpassword));
                    return PromptView("修改成功，下次登录请使用新密码！");
                }
                else
                {
                    return PromptView("修改失败！");
                }
            }
        }
        #endregion
        public ActionResult WebInit()
        {
            return View();
        }
        #region 分类的
        public ActionResult CategoryList()
        {
            List<Pro_Types> list = DAL.Pro_Types.GetList("[BusinessID]=" + WorkContext.ID.ToString());
            return View(list);
        }

        public ActionResult CategoryAdd(Pro_Types model)
        {
            Load();
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
                BusinessID = WorkContext.ID
            };
            DAL.Pro_Types.Add(m);
            return PromptView("/BusinessManger/UsersAdmin/CategoryList", "分类添加成功");
        }
        public ActionResult CategoryEdit(Pro_Types model, int tid = -1)
        {
            Load();
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
                return PromptView("/BusinessManger/UsersAdmin/CategoryList", "分类修改成功");
            }
            return View(model);
        }

        public ActionResult CategoryDelete(int tid = -1)
        {
            int result = DAL.Pro_Types.DeleteByID(tid);
            if (result == -3)
                return PromptView("分类不存在");
            if (result == -2)
                return PromptView("删除失败，请先转移或删除此分类下的子分类");
            if (result == 0)
                return PromptView("删除失败，请先转移或删除此分类下的产品");
            return PromptView("分类删除成功");
        }
        private void Load()
        {
            ViewData["categoryList"] = DAL.Pro_Types.GetList("[BusinessID]=" + WorkContext.ID.ToString());
        }
        #endregion
        #region 商品的
        public ActionResult ProductList(string txtTitle, int parentid = 0, int page = 1)
        {
            Load();
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[BusinessID]=" + WorkContext.ID.ToString());
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (parentid > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", parentid.ToString()));
            ListModel.CreatePage(model, "uv_Pro_Case", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult ProductDelete(int[] pid)
        {
            DAL.PageModel.Delete("Pro_Case", "ID", pid);
            //AddAdminLog("删除产品", "删除产品,产品ID为:" + AliceDAL.Common.Ints2String(pid));
            return PromptView("产品删除成功");
        }
        [ValidateInput(false)]
        public ActionResult ProductAdd(Pro_Case model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.Author = WorkContext.BusinessName;
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
                BusinessID = WorkContext.ID,
                ProType = model.ProType
            };
            if (DAL.Pro_Case.Add(pc) > 0)
            {
                //AddAdminLog("添加产品", "添加产品，产品为：" + pc.Title);
                return PromptView("产品添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "产品添加失败，请稍后再试");
                return View(model);
            }
        }

        [ValidateInput(false)]
        public ActionResult ProductEdit(Pro_Case model, int pid = -1)
        {
            Load();
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
                //AddAdminLog("修改产品", "修改产品，产品ID为：" + pc.ID);
                return PromptView("/BusinessManger/UsersAdmin/ProductList", "产品修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "产品修改失败，请稍后再试");
                return View(model);
            }
        }
        public ActionResult ProChangeState(int cid = -1, int page = 1)
        {
            Pro_Case pc = DAL.Pro_Case.GetModel(cid);
            if (pc == null) return PromptView("此商品不存在");
            DAL.Pro_Case.ChangeState(cid, pc.ProState == 30 ? 10 : 30);
            //AddAdminLog("商品状态", (pc.ProState == 30 ? "上架" : "下架") + "商品,商品ID为:" + cid + ";名称为:" + pc.Title);
            return Redirect(Url.Action("ProductList", new { page = page.ToString() }));
        }
        #endregion
    }
}
