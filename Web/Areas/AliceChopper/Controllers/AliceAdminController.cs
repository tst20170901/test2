using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Models;
using Web.Areas.AliceChopper.Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class AliceAdminController : AdminBaseController
    {
        public ActionResult Index()
        {
            ViewData["ActionTree"] = DAL.BD_BearAdminActions.GetAdminActionTree();
            return View();
        }
        #region 网站基本信息
        [HttpGet]
        public ActionResult WebInfo()
        {
            WebConfigInfo model = new WebConfigInfo();
            model.WebName = AliceDAL.IniHelper.GetValue("WebInfo", "WebName");
            model.CompanyName = AliceDAL.IniHelper.GetValue("WebInfo", "CompanyName");
            model.KeyWords = AliceDAL.IniHelper.GetValue("WebInfo", "KeyWords");
            model.Description = AliceDAL.IniHelper.GetValue("WebInfo", "Description");
            model.Url = AliceDAL.IniHelper.GetValue("WebInfo", "URL");
            model.TimeSpanForLoc = AliceDAL.IniHelper.GetValue("WebInfo", "TimeSpanForLoc");
            model.Phone = AliceDAL.IniHelper.GetValue("WebInfo", "Phone");
            model.Mobile = AliceDAL.IniHelper.GetValue("WebInfo", "Mobile");
            model.Fax = AliceDAL.IniHelper.GetValue("WebInfo", "Fax");
            model.Address = AliceDAL.IniHelper.GetValue("WebInfo", "Address");
            model.Code = AliceDAL.IniHelper.GetValue("WebInfo", "Code");
            model.Email = AliceDAL.IniHelper.GetValue("WebInfo", "Email");
            model.LinkMan = AliceDAL.IniHelper.GetValue("WebInfo", "LinkMan");
            model.Data1 = AliceDAL.IniHelper.GetValue("WebInfo", "Data1");
            model.Data2 = AliceDAL.IniHelper.GetValue("WebInfo", "Data2");
            model.Data3 = AliceDAL.IniHelper.GetValue("WebInfo", "Data3");
            model.Data4 = AliceDAL.IniHelper.GetValue("WebInfo", "Data4");
            model.Data5 = AliceDAL.IniHelper.GetValue("WebInfo", "Data5");
            model.Data6 = AliceDAL.IniHelper.GetValue("WebInfo", "Data6");
            model.Data7 = AliceDAL.IniHelper.GetValue("WebInfo", "Data7");
            model.Data8 = AliceDAL.IniHelper.GetValue("WebInfo", "Data8");
            model.Data9 = AliceDAL.IniHelper.GetValue("WebInfo", "Data9");
            model.Data10 = AliceDAL.IniHelper.GetValue("WebInfo", "Data10");
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult WebInfo(WebConfigInfo model)
        {
            if (ModelState.IsValid)
            {
                AliceDAL.IniHelper.SetValue("WebInfo", "WebName", model.WebName);
                AliceDAL.IniHelper.SetValue("WebInfo", "CompanyName", model.CompanyName);
                AliceDAL.IniHelper.SetValue("WebInfo", "KeyWords", model.KeyWords);
                AliceDAL.IniHelper.SetValue("WebInfo", "Description", model.Description);
                AliceDAL.IniHelper.SetValue("WebInfo", "URL", model.Url);
                AliceDAL.IniHelper.SetValue("WebInfo", "TimeSpanForLoc", model.TimeSpanForLoc);
                AliceDAL.IniHelper.SetValue("WebInfo", "Phone", model.Phone);
                AliceDAL.IniHelper.SetValue("WebInfo", "Mobile", model.Mobile);
                AliceDAL.IniHelper.SetValue("WebInfo", "Fax", model.Fax);
                AliceDAL.IniHelper.SetValue("WebInfo", "Address", model.Address);
                AliceDAL.IniHelper.SetValue("WebInfo", "Code", model.Code);
                AliceDAL.IniHelper.SetValue("WebInfo", "Email", model.Email);
                AliceDAL.IniHelper.SetValue("WebInfo", "LinkMan", model.LinkMan);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data1", model.Data1);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data2", model.Data2);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data3", model.Data3);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data4", model.Data4);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data5", model.Data5);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data6", model.Data6);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data7", model.Data7);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data8", model.Data8);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data9", model.Data9);
                AliceDAL.IniHelper.SetValue("WebInfo", "Data10", model.Data10);
                AddAdminLog("修改站点设置信息");
                return PromptView(Url.Action("WebInfo"), "修改站点设置信息成功");
            }
            return View(model);
        }
        #endregion

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
                int result = new DAL.BD_Admin().EditPwd(WorkContext.LoginID, AliceDAL.SecureHelper.MD5(newpassword));
                if (result > 0)
                {
                    AddAdminLog("修改密码", WorkContext.NickName + "修改密码，ID为" + WorkContext.ID.ToString());
                    AliceDAL.Common.SetCookie("LoginInfo", "LoginID", WorkContext.LoginID);
                    AliceDAL.Common.SetCookie("LoginInfo", "Password", AliceDAL.SecureHelper.MD5(newpassword));
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
        public ActionResult WebSet(BranchConfig model)
        {
            if (WorkContext.Branch == null)
            {
                return PromptView("所属平台为空！");
            }
            if (AliceDAL.Common.IsGet())
            {
                if (WorkContext.Branch.BranchState == 0)
                {
                    return PromptView("平台被禁用，请联系总管理员！");
                }
                model.BranchState = WorkContext.Branch.BranchState;
                model.OrderCount = WorkContext.Branch.OrderCount;
                model.StateMsg = WorkContext.Branch.StateMsg;
                return View(model);
            }
            WorkContext.Branch.OrderCount = model.OrderCount;
            WorkContext.Branch.StateMsg = model.StateMsg;
            WorkContext.Branch.BranchState = model.BranchState;
            DAL.BD_Branch.ChangeBranchState(WorkContext.Branch.ID, model.BranchState, model.StateMsg, model.OrderCount);
            return View(model);
        }
    }
}
