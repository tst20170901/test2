using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
using AliceDAL;
using Newtonsoft.Json;
using Web.Areas.WebWorker.Models;
using System.Data;

namespace Web.Areas.WebAdmin.Controllers
{
    public class AccountController : WebAdminAreaBaseController
    {
        public ActionResult Login()
        {
            if (AliceDAL.Common.IsGet())
            {
                ViewBag.LoginID = AliceDAL.Common.GetCookie("WebAdminLoginInfoLoginID");
                return View();
            }
            try
            {
                string LoginID = AliceDAL.Common.GetFormString("loginID");
                string password = AliceDAL.Common.GetFormString("password");
                string result, url = "";

                if (string.IsNullOrWhiteSpace(LoginID))
                {
                    result = "帐号不能为空";
                }
                else if (string.IsNullOrWhiteSpace(password))
                {
                    result = "密码不能为空";
                }
                else
                {
                    BD_Admin admin = new DAL.BD_Admin().Login(LoginID, AliceDAL.SecureHelper.MD5(password), out result);
                    if (null != admin)
                    {
                        AliceDAL.Common.SetCookie("WorkerLoginInfoLoginID", admin.LoginID, 999999);
                        AliceDAL.Common.SetCookie("aliceinfo", "LoginID", admin.LoginID, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Password", admin.Password, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "ID", admin.ID.ToString(), 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "RoleID", admin.RoleID.ToString(), 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "BranchID", admin.BranchID.ToString(), 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "NickName", admin.NickName, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data1", admin.Data1, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data2", admin.Data2, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data3", admin.Data3, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data4", admin.Data4, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data5", admin.Data5, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data6", admin.Data6, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data7", admin.Data7, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data8", admin.Data8, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data9", admin.Data9, 527040);
                        AliceDAL.Common.SetCookie("aliceinfo", "Data10", admin.Data10, 527040);
                        url = "/webadmin/usersadmin/";
                        result = "OK";
                    }
                    return AjaxResult(url, result);
                }
                return AjaxResult(url, result);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return View();
            }
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
