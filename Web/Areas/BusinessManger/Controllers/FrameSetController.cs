using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
namespace Web.Areas.BusinessManger.Controllers
{
    public class FrameSetController : BusinessAreaBaseController
    {
        public ActionResult Login()
        {
            if (AliceDAL.Common.IsGet())
            {
                ViewBag.LoginID = AliceDAL.Common.GetCookie("BusinessInfoLoginID");
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
            BD_Business admin = DAL.BD_Business.Login(LoginID, AliceDAL.SecureHelper.MD5(password), out result);
            if (null != admin)
            {
                if (admin.BusinessState ==0)
                {
                    url = "";
                    result = "商户已禁用！";
                    return AjaxResult(url, result);
                }


                AliceDAL.Common.SetCookie("BusinessInfoLoginID", admin.LoginID, 999999);
                HttpCookie cookie = new HttpCookie("BusinessUserInfo");
                AliceDAL.Common.SetCookie("BusinessUserInfo", "ID", admin.ID.ToString(), 527040);
                AliceDAL.Common.SetCookie("BusinessUserInfo", "Password", AliceDAL.SecureHelper.MD5(admin.Password + admin.ID.ToString()), 527040);
                url = "/BusinessManger/UsersAdmin/";
                result = "OK";
            }
            return AjaxResult(url, result);
        }
        public ActionResult LoginOut()
        {
            AliceDAL.Common.DeleteCookie("BusinessUserInfo");
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
