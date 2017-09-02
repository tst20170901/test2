using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Web.Units;
using Alice.WebControls.Mvc;
using Web.Areas.Models;
using Models;
using AliceDAL;

namespace Web.Areas.AliceChopper.Controllers
{
    public class UserController : AdminBaseController
    {
        public ActionResult UserList(string loginID, string nickname, int BranchID = -1, int page = 1)
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
            if (!String.IsNullOrEmpty(loginID)) AliceDAL.Common.SqlString(strsql, String.Format("[LoginID] like '%{0}%'", AliceDAL.Common.cutBadStr(loginID)));
            if (!String.IsNullOrEmpty(nickname)) AliceDAL.Common.SqlString(strsql, String.Format("[NickName] like '%{0}%'", AliceDAL.Common.cutBadStr(nickname)));
            ListModel.CreatePage(model, "[uv_BD_Admin]", strsql, page, WorkContext.PageSize);
            return View(model);
        }

        public ViewResult Delete(int[] uid)
        {
            if (uid.Length >= 0 && WorkContext.ID == uid[0]) return PromptView("不能删除当前用户");
            DAL.PageModel.Delete("BD_Admin", "ID", uid);
            AddAdminLog("删除用户", "删除用户,用户ID为:" + AliceDAL.Common.Ints2String(uid));
            return PromptView("用户删除成功");
        }
        public ActionResult Edit(BD_Admin model, int uid = -1, int page = 1)
        {
            LoadBranch();
            LoadRole();
            DataTable dt = DAL.PageModel.Table_Model("[BD_Admin]", String.Format("ID={0}", uid.ToString()));
            if (dt == null || dt.Rows.Count <= 0) return PromptView("用户不存在");
            else
            {
                BD_Admin db = new BD_Admin();
                db.Data1 = dt.Rows[0]["Data1"].ToString();
                db.Data2 = dt.Rows[0]["Data2"].ToString();
                db.Data3 = dt.Rows[0]["Data3"].ToString();
                db.Data4 = dt.Rows[0]["Data4"].ToString();
                db.Data5 = dt.Rows[0]["Data5"].ToString();
                db.Data6 = dt.Rows[0]["Data6"].ToString();
                db.Data7 = dt.Rows[0]["Data7"].ToString();
                db.Data8 = dt.Rows[0]["Data8"].ToString();
                db.Data9 = dt.Rows[0]["Data9"].ToString();
                db.Data10 = dt.Rows[0]["Data10"].ToString();
                db.RoleID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["RoleID"].ToString());
                db.BranchID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BranchID"].ToString());
                db.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                db.LoginID = dt.Rows[0]["LoginID"].ToString();
                db.NickName = dt.Rows[0]["NickName"].ToString();
                db.Password = dt.Rows[0]["Password"].ToString();
                if (AliceDAL.Common.IsGet())
                {
                    return View(db);
                }
                else
                {
                    DataTable tp = DAL.PageModel.DateList("BD_Admin", String.Format("ID!={0} and NickName='{1}'", uid.ToString(), model.NickName), "ID", 1);
                    if (tp != null && tp.Rows.Count > 0)
                    {
                        ModelState.AddModelError("LoginID", "昵称已存在");
                        return View(model);
                    }
                    if (model.BranchID <= 0)
                    {
                        ModelState.AddModelError("LoginID", "请选择分公司");
                        return View(model);
                    }
                    else if (model.RoleID <= 1)
                    {
                        ModelState.AddModelError("LoginID", "请选择角色");
                        return View(model);
                    }
                    if (ModelState.IsValid)
                    {
                        db.NickName = model.NickName;
                        db.RoleID = model.RoleID;
                        db.BranchID = model.BranchID;
                        if (!String.IsNullOrWhiteSpace(model.Password)) db.Password = AliceDAL.SecureHelper.MD5(model.Password);
                        if (new DAL.BD_Admin().Edit(db))
                        {
                            AddAdminLog("修改用户", "修改用户,用户ID为:" + uid);
                            return PromptView(Url.Action("UserList", new { page = page.ToString() }), "用户修改成功");
                        }
                        else
                        {
                            return PromptView("用户修改失败");
                        }
                    }
                }
            }
            return View(model);
        }
        public ActionResult Add(BD_Admin model)
        {
            LoadBranch();
            LoadRole();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "密码不能为空");
                return View(model);
            }
            else if (DAL.PageModel.DateList("BD_Admin", String.Format("LoginID='{0}'", model.LoginID ?? ""), "ID", 1) != null)
            {
                ModelState.AddModelError("LoginID", "登录名已经存在");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("LoginID", "请选择分公司");
                return View(model);
            }
            else if (model.RoleID <= 1)
            {
                ModelState.AddModelError("LoginID", "请选择角色");
                return View(model);
            }

            BD_Admin db = new BD_Admin()
            {
                Data1 = "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                Data6 = "",
                Data7 = "",
                Data8 = "",
                Data9 = "",
                Data10 = "",
                RoleID = model.RoleID,
                BranchID = model.BranchID,
                LoginID = model.LoginID,
                NickName = model.NickName ?? "",
                Password = AliceDAL.SecureHelper.MD5(model.Password)
            };
            string result = new DAL.BD_Admin().Add(db);
            if (result == "exists")
            {
                ModelState.AddModelError("UserName", "登录名已经存在");
            }
            else if (result == "ok")
            {
                AddAdminLog("添加用户", String.Format("添加用户,用户为:{0}({1})", db.NickName, db.LoginID));
                return PromptView("用户添加成功");
            }
            return View(model);
        }
        public ActionResult AdminGroupList(string GroupName, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[ID]>1");
            if (!String.IsNullOrEmpty(GroupName)) AliceDAL.Common.SqlString(strsql, String.Format("[Title] like '%{0}%'", AliceDAL.Common.cutBadStr(GroupName)));
            ListModel.CreatePage(model, "[BD_BearActionsAllot]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult AdminGroupAdd(Models.ActionsAllotModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError("Title", "管理员组名称不能为空");
                return View(model);
            }

            BD_BearActionsAllot bb = new BD_BearActionsAllot()
            {
                Title = model.Title,
                ActionList = "aliceadminindex,aliceadminwebinit," + AliceDAL.Common.Strings2String(model.ActionList).ToLower()
            };
            bb.ActionList = bb.ActionList.TrimEnd(new char[] { ',' });
            string result = DAL.BD_BearActionsAllot.Add(bb);
            if (result == "-2")
            {
                ModelState.AddModelError("Title", "管理员组名称已经存在");
            }
            else if (result == "-1")
            {
                ModelState.AddModelError("Title", "系统错误，请稍候再试");
            }
            else
            {
                AddAdminLog("添加管理员组", String.Format("添加管理员组,名称为:{0},权限为:{1}", bb.Title, bb.ActionList));
                return PromptView(Url.Action("AdminGroupList"), "添加成功");
            }
            return View(model);
        }
        public ActionResult AdminGroupEdit(Models.ActionsAllotModel model, int aid = -1)
        {
            Load();
            BD_BearActionsAllot bb = DAL.BD_BearActionsAllot.Model(aid);
            if (bb == null) return PromptView("此管理员组不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.ActionList = bb.ActionList.Split(',');
                model.Title = bb.Title;
                return View(model);
            }

            if (String.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError("Title", "管理员组名称不能为空");
                return View(model);
            }
            BD_BearActionsAllot bba = new BD_BearActionsAllot()
            {
                Title = model.Title,
                ActionList = "aliceadminindex,aliceadminwebinit," + AliceDAL.Common.Strings2String(model.ActionList).ToLower(),
                ID = bb.ID
            };
            bba.ActionList = bba.ActionList.TrimEnd(new char[] { ',' });
            string result = DAL.BD_BearActionsAllot.Edit(bba);
            if (result == "-2")
            {
                ModelState.AddModelError("Title", "管理员组名称已经存在");
            }
            else if (result == "1")
            {
                AddAdminLog("管理员组更改", String.Format("管理员组更改,名称为:{0},权限为:{1},原名称为:{2},原权限为{3}", bba.Title, bba.ActionList, bb.Title, bb.ActionList));
                return PromptView(Url.Action("AdminGroupList"), "修改成功");
            }
            return View(model);
        }
        private void Load()
        {
            ViewData["ActionTree"] = DAL.BD_BearAdminActions.GetAdminActionTree();
        }
        private void LoadRole()
        {
            ViewData["RoleList"] = DAL.PageModel.DataKeysList("[BD_BearActionsAllot]", "[ID],[Title]", "[ID]>1", "ID", 0);
        }
    }
}
