using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
namespace Web.Areas.AliceChopper.Controllers
{
    public class AliceChopperController : AreaBaseController
    {
        public ActionResult Login()
        {
            if (AliceDAL.Common.IsGet()) return View();
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
            if (String.Compare(AliceDAL.SecureHelper.AESDecrypt(AliceDAL.Common.GetCookie("CheckCodeSum"), "duoduo"), verifyCode, true) != 0)
            {
                url = "";
                result = "验证码错误！";
                return AjaxResult(url, result);
            }
            BD_Admin admin = new DAL.BD_Admin().Login(LoginID, AliceDAL.SecureHelper.MD5(password), out result);
            if (null != admin)
            {
                HttpCookie cookie = new HttpCookie("LoginInfo");
                AliceDAL.Common.SetCookie("LoginInfo", "LoginID", admin.LoginID);
                AliceDAL.Common.SetCookie("LoginInfo", "Password", admin.Password);
                AliceDAL.Common.SetCookie("LoginInfo", "ID", admin.ID.ToString());
                AliceDAL.Common.SetCookie("LoginInfo", "RoleID", admin.RoleID.ToString());
                AliceDAL.Common.SetCookie("LoginInfo", "BranchID", admin.BranchID.ToString());
                AliceDAL.Common.SetCookie("LoginInfo", "NickName", admin.NickName);
                AliceDAL.Common.SetCookie("LoginInfo", "Data1", admin.Data1);
                AliceDAL.Common.SetCookie("LoginInfo", "Data2", admin.Data2);
                AliceDAL.Common.SetCookie("LoginInfo", "Data3", admin.Data3);
                AliceDAL.Common.SetCookie("LoginInfo", "Data4", admin.Data4);
                AliceDAL.Common.SetCookie("LoginInfo", "Data5", admin.Data5);
                AliceDAL.Common.SetCookie("LoginInfo", "Data6", admin.Data6);
                AliceDAL.Common.SetCookie("LoginInfo", "Data7", admin.Data7);
                AliceDAL.Common.SetCookie("LoginInfo", "Data8", admin.Data8);
                AliceDAL.Common.SetCookie("LoginInfo", "Data9", admin.Data9);
                AliceDAL.Common.SetCookie("LoginInfo", "Data10", admin.Data10);
                url = Url.Action("Index", "AliceAdmin");
                result = "OK";
            }
            string s = AjaxResult(url, result).Data.ToString();
            return AjaxResult(url, result);
        }
        public ActionResult LoginOut()
        {
            AliceDAL.Common.DeleteCookie("LoginInfo");
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
            AliceDAL.Common.SetCookie("CheckCodeSum", AliceDAL.SecureHelper.AESEncrypt(code, "duoduo"));
            return File(seccode.CreateImage(code), "image/png");
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
