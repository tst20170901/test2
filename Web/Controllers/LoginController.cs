using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
using System.Data;
namespace Web.Areas.AliceChopper.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (AliceDAL.Common.IsGet())
            {
                AliceDAL.Common.DeleteCookie("UserLoginInfo");
                return View();
            }
            string LoginID = AliceDAL.Common.GetFormString("loginID");
            string password = AliceDAL.Common.GetFormString("password");
            string returnurl = AliceDAL.Common.GetFormString("returnUrl");
            if (String.IsNullOrEmpty(returnurl))
            {
                returnurl = "/BearClient";
            }
            string result, url = "";
            if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }
            BD_Users admin = new DAL.BD_Users().LoginCode(LoginID, password, out result);
            if (null != admin)
            {
                AliceDAL.Common.SetCookie("UserLoginInfo", "UserPwd", admin.UserPwd, 527040);
                AliceDAL.Common.SetCookie("UserLoginInfo", "ID", admin.ID.ToString(), 527040);
                url = returnurl;
                result = "OK";
            }
            return AjaxResult(url, result);
        }
        public ActionResult NewPhone()
        {
            if (AliceDAL.Common.IsGet()) return View();
            string LoginID = AliceDAL.Common.GetFormString("loginID");
            string password = AliceDAL.Common.GetFormString("password");
            string result, url = "";
            if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }
            //BD_Users admin = new DAL.BD_Users().LoginCode(LoginID, password, out result);
            //if (null != admin)
            //{
            //    //if (admin.Data9 != "1")
            //    //{
            //    //    url = "";
            //    //    result = "该活动仅限新注册用户参加！";
            //    //    return AjaxResult(url, result);
            //    //}
            //    int count = DAL.PageModel.DataCount("Coupons", String.Format("[Uid]={0} and [TypeID]={1}", admin.ID, 12));//12为新注册用户5元券的ID
            //    if (count <= 0)
            //    {
            //        Coupons cou = new Coupons()
            //        {
            //            CouponState = (int)CouponState.Submitted,
            //            OriginalPrice = 5,
            //            Price = 5,
            //            TypeID = 12,
            //            Uid = admin.ID,
            //            Count = 6,
            //            TDate = DateTime.Now.AddDays(180)
            //        };
            //        for (int i = 0; i < cou.Count; i++)
            //        {
            //            DAL.Coupons.CreateCoupons(cou);
            //        }
            //        string con = "【小熊洗车】尊敬的小熊用户您好，非常感谢您试用小熊移动洗车服务，小熊账户30元优惠券已到帐，可立即消费，祝您洗车愉快！客服电话:0318-8888235";
            //        MessageManger.PostMsg(LoginID, con);
            //    }

            //    HttpCookie cookie = new HttpCookie("UserLoginInfo");
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "LoginID", admin.LoginID, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "UserPwd", admin.UserPwd, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "ID", admin.ID.ToString(), 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Mobile", admin.Mobile, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data1", admin.Data1, 527040);//用户姓名
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data2", admin.Data2, 527040);//角色 1是普通客户 2是大客户，只能清洗限定车牌号
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data3", admin.Data3, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data4", admin.Data4, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data5", admin.Data5, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data6", admin.Data6, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data7", admin.Data7, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data8", admin.Data8, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data9", admin.Data9, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "Data10", admin.Data10, 527040);
            //    AliceDAL.Common.SetCookie("UserLoginInfo", "CDate", admin.CDate.ToString("yyyy-MM-dd"), 527040);
            //    url = Url.Action("Result", "NewPhone", new { userID = admin.ID });
            result = "OK";
            //}
            return AjaxResult(url, result);
        }
        public ActionResult Reg()
        {
            if (AliceDAL.Common.IsGet()) return View();
            string LoginID = AliceDAL.Common.GetFormString("loginID");
            string password = AliceDAL.Common.GetFormString("password");
            string repassword = AliceDAL.Common.GetFormString("repassword");
            string result = "";
            string url = "";
            if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }
            else if (password != repassword)
            {
                url = "";
                result = "两次密码不一致！";
                return AjaxResult(url, result);
            }
            BD_Users user = new BD_Users();
            user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";
            user.LoginID = LoginID;
            user.Mobile = LoginID;
            user.UserPwd = AliceDAL.SecureHelper.MD5(password);

            string res = DAL.BD_Users.Add(user);
            if (res == "exists loginid" || res == "exists mobile")
            {
                result = "此手机号码已被注册";
            }
            else if (res == "error")
            {
                result = "网络错误，请稍候";
            }
            else
            {
                result = "注册成功";
                url = "/Login/";
            }

            return AjaxResult(url, result);
        }
        public ActionResult LoginOut()
        {
            AliceDAL.Common.DeleteCookie("UserLoginInfo");
            AliceDAL.Common.DeleteCookie("CheckCodeSumUser");
            return RedirectToAction("Index");
        }
        public ActionResult NewPhoneSendMsg()
        {
            string LoginID = AliceDAL.UrlParam.GetStringValue("loginID");
            string result, url = "";
            if (String.IsNullOrWhiteSpace(LoginID))
            {
                url = "";
                result = "手机号码不能为空！";
                return AjaxResult(url, result);
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }

            try
            {
                //BD_Users user = new BD_Users();
                //user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = "";

                //user.LoginID = LoginID;
                //user.Mobile = LoginID;
                //user.UserPwd = AliceDAL.Common.CreateRandomValue(6, true);

                //string code = AliceDAL.Common.CreateRandomValue(4, true);
                //user.Data10 = code;
                //string con = "【小熊洗车】您的手机验证码为：" + code + "。本信息请勿回复，如需帮助请拨打客服电话：0318-8888235，祝您洗车愉快。";
                //MessageManger.SendMsg(LoginID, con);
                //DAL.BD_Users.EditCode(user);
                //url = "success";
                result = "";
                return AjaxResult(url, result);
            }
            catch (Exception ex)
            {
                url = "error";
                result = "系统错误，请稍候再试！";
                return AjaxResult(url, result);
            }
        }
        public ActionResult SendMsg()
        {
            string LoginID = AliceDAL.UrlParam.GetStringValue("loginID");
            string result, url = "";
            if (String.IsNullOrWhiteSpace(LoginID))
            {
                url = "";
                result = "手机号码不能为空！";
                return AjaxResult(url, result);
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }

            try
            {
                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = "";

                user.LoginID = LoginID;
                user.Mobile = LoginID;
                user.UserPwd = AliceDAL.SecureHelper.MD5("123456");

                string code = AliceDAL.Common.CreateRandomValue(4, true);
                user.Data10 = code;
                //string con = "【小熊洗车】您的手机验证码为：" + code + "。本信息请勿回复，如需帮助请拨打客服电话：0318-8888235，祝您洗车愉快。";
                string con = "【小熊洗车】您的手机验证码为：" + code + "。小熊洗车，您身边的贴心车管家。全 国 加 盟，招 商 热 线：18731817777。";
                MessageManger.PostMsg(LoginID, con);
                DAL.BD_Users.EditCode(user);
                url = "success";
                result = "验证码发送成功";
                return AjaxResult(url, result);
            }
            catch (Exception ex)
            {
                url = "error";
                result = "系统错误，请稍候再试！";
                return AjaxResult(url, result);
            }
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
            AliceDAL.Common.SetCookie("CheckCodeSumUser", AliceDAL.SecureHelper.AESEncrypt(code, "duoduo"));
            return File(seccode.CreateImage(code), "image/png");
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
