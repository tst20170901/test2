using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
namespace Web.Areas.UsersManger.Controllers
{
    public class FrameSetController : UsersAreaBaseController
    {
        public ActionResult Login()
        {
            if (AliceDAL.Common.IsGet())
            {
                ViewBag.LoginID = AliceDAL.Common.GetCookie("UserLoginInfoLoginID");
                return View();
            }
            string LoginID = AliceDAL.Common.GetFormString("loginID");
            string password = AliceDAL.Common.GetFormString("password");
            string verifyCode = AliceDAL.Common.GetFormString("verifyCode");
            string result, url = "";
            if (String.IsNullOrEmpty(AliceDAL.Common.GetCookie("CheckCodeSum")))
            {
                url = "";
                result = "Cookies不能使用，无法使用本系统";
                return AjaxResult(url, result);
            }
            if (String.Compare(AliceDAL.SecureHelper.AESDecrypt(AliceDAL.Common.GetCookie("CheckCodeSum"), "duoduo1"), verifyCode, true) != 0)
            {
                url = "";
                result = "验证码错误！";
                return AjaxResult(url, result);
            }
            BD_Users admin = new DAL.BD_Users().Login(LoginID, AliceDAL.SecureHelper.MD5(password), out result);
            if (null != admin)
            {
                if (admin.Data2 != "2")
                {
                    url = "";
                    result = "用户类型错误，不能在此登录！";
                    return AjaxResult(url, result);
                }
                AliceDAL.Common.SetCookie("UserLoginInfoLoginID", admin.LoginID, 999999);
                HttpCookie cookie = new HttpCookie("UserLoginInfo");
                AliceDAL.Common.SetCookie("UserLoginInfo", "LoginID", admin.LoginID);
                AliceDAL.Common.SetCookie("UserLoginInfo", "UserPwd", admin.UserPwd);
                AliceDAL.Common.SetCookie("UserLoginInfo", "ID", admin.ID.ToString());
                AliceDAL.Common.SetCookie("UserLoginInfo", "Mobile", admin.Mobile);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data1", admin.Data1);//用户姓名
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data2", admin.Data2);//角色 1是普通客户 2是大客户，只能清洗限定车牌号
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data3", admin.Data3);//常用地址
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data4", admin.Data4);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data5", admin.Data5);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data6", admin.Data6);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data7", admin.Data7);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data8", admin.Data8);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data9", admin.Data9);
                AliceDAL.Common.SetCookie("UserLoginInfo", "Data10", admin.Data10);
                AliceDAL.Common.SetCookie("UserLoginInfo", "CDate", admin.CDate.ToString("yyyy-MM-dd"));
                AliceDAL.Common.SetCookie("UserLoginInfo", "BranchID", admin.BranchID.ToString());
                url = "/UsersManger/UsersAdmin/";
                result = "OK";
            }
            return AjaxResult(url, result);
        }
        public ActionResult LoginOut()
        {
            AliceDAL.Common.DeleteCookie("UserLoginInfo");
            AliceDAL.Common.DeleteCookie("CheckCodeSum");
            return RedirectToAction("Login");
        }
        public ActionResult VerifyImage(int size = 17)
        {
            Units.SecurityCode seccode = new Units.SecurityCode();
            seccode.Length = 4;
            seccode.FontSize = size;
            seccode.Chaos = true;
            seccode.BackgroundColor = seccode.BackgroundColor;
            seccode.ChaosColor = seccode.ChaosColor;
            seccode.Colors = seccode.Colors;
            seccode.Fonts = seccode.Fonts;
            seccode.Padding = seccode.Padding;
            string code = seccode.CreateVerifyCode(4);//取随机码
            AliceDAL.Common.SetCookie("CheckCodeSum", AliceDAL.SecureHelper.AESEncrypt(code, "duoduo1"));
            return File(seccode.CreateImage(code), "image/png");
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
