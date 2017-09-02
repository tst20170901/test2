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

namespace Web.Areas.WebWorker.Controllers
{
    public class AccountController : WebWorkerAreaBaseController
    {
        public ActionResult Login()
        {
            if (AliceDAL.Common.IsGet())
            {
                ViewBag.LoginID = AliceDAL.Common.GetCookie("WorkerLoginInfoLoginID");
                return View();
            }
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
                WorkShop user = new DAL.WorkShop().Login(LoginID, AliceDAL.SecureHelper.MD5(password), out result);
                if (user != null)
                {
                    AliceDAL.Common.SetCookie("WorkerLoginInfoLoginID", user.Mobile, 999999);
                    HttpCookie cookie = new HttpCookie("WebWorkerUserInfo");
                    AliceDAL.Common.SetCookie("WebWorkerUserInfo", "ID", user.ID.ToString(), 527040);
                    AliceDAL.Common.SetCookie("WebWorkerUserInfo", "UserPwd", AliceDAL.SecureHelper.MD5(user.UserPwd + user.ID.ToString()), 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Title", user.Title, 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Mobile", user.Mobile, 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Phone", user.Phone, 527040);                   
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "BranchID", user.BranchID.ToString(), 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "TypeID", user.TypeID.ToString(), 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "WorkState", user.WorkState.ToString(), 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Lat", user.Lat1, 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Lng", user.Lng1, 527040);
                    //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "ReDate", user.ReDate.ToString("yyyy-MM-dd"), 527040);
                    url = "/webworker/usersadmin/";
                    result = "OK";
                }
            }
            return AjaxResult(url, result);
        }

        public ActionResult CheckOrder()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "检测失败";
            if (AliceDAL.Common.IsGet()) return View();
            string plate = AliceDAL.Common.GetFormString("Plate").ToUpper();
            if (!AliceDAL.ValidateHelper.IsCarNo(plate))
            {
                mm.msg = "车牌号码格式不正确！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            CheckOrderInfoModel coim = new CheckOrderInfoModel();
            Orders orderInfo = DAL.Orders.ExistOrder(plate);
            if (orderInfo == null)
            {
                mm.msg = "系统中不存在该车牌号的订单";
                return Content(JsonConvert.SerializeObject(mm));
            }
            coim.Address = orderInfo.Address;
            coim.BrandShow = orderInfo.BrandShow;
            coim.CDate = orderInfo.CDate.ToString();
            coim.Color = orderInfo.Color;
            coim.Mobile = orderInfo.Mobile;
            coim.Plate = orderInfo.Plate;
            coim.UserName = orderInfo.UserName;
            WorkShop ws = DAL.WorkShop.WorkShopByID(orderInfo.WorkID);
            if (ws != null)
            {
                coim.WorkMobile = ws.Phone;
                coim.WorkName = ws.Title;
            }
            UserOrderState uos = (UserOrderState)orderInfo.OrderState;
            switch (uos)
            {
                case UserOrderState.WaitPaying:
                    coim.OrderState = "未支付";
                    break;
                case UserOrderState.Payed:
                    coim.OrderState = "支付成功";
                    break;
                case UserOrderState.Assigned:
                    coim.OrderState = "派单完成";
                    break;
                case UserOrderState.Started:
                    coim.OrderState = "开始洗车";
                    break;
                case UserOrderState.Finished:
                    coim.OrderState = "完成洗车";
                    break;
                case UserOrderState.Canceled:
                    coim.OrderState = "已取消";
                    break;
                case UserOrderState.Deleted:
                    coim.OrderState = "已删除";
                    break;
                case UserOrderState.Void:
                    coim.OrderState = "已作废";
                    break;
                default:
                    break;
            }
            coim.WashItem = Comm.GetItemNoPrice(orderInfo.ID.ToString());
            mm.data = coim;
            mm.code = "1";
            string s = JsonConvert.SerializeObject(mm);
            return Content(s);
        }
        public ActionResult CheckWorkShopID()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "检测失败";
            string plate = AliceDAL.Common.GetFormString("WorkShopNo");

            WorkShop ws = DAL.WorkShop.WorkShopForMobile(plate);
            if (ws == null)
            {
                mm.msg = "系统中不存在该洗车工";
                return Content(JsonConvert.SerializeObject(mm));
            }
            CheckOrderInfoModel coim = new CheckOrderInfoModel();
            Orders orderInfo = DAL.Orders.GetWorkOrder(ws.ID);
            if (orderInfo != null)
            {
                coim.Address = orderInfo.Address;
                coim.BrandShow = orderInfo.BrandShow;
                coim.CDate = orderInfo.CDate.ToString();
                coim.Color = orderInfo.Color;
                coim.Mobile = orderInfo.Mobile;
                coim.Plate = orderInfo.Plate;
                coim.UserName = orderInfo.UserName;
                coim.WashItem = Comm.GetItemNoPrice(orderInfo.ID.ToString());
                UserOrderState uos = (UserOrderState)orderInfo.OrderState;
                switch (uos)
                {
                    case UserOrderState.WaitPaying:
                        coim.OrderState = "未支付";
                        break;
                    case UserOrderState.Payed:
                        coim.OrderState = "支付成功";
                        break;
                    case UserOrderState.Assigned:
                        coim.OrderState = "派单完成";
                        break;
                    case UserOrderState.Started:
                        coim.OrderState = "开始洗车";
                        break;
                    case UserOrderState.Finished:
                        coim.OrderState = "完成洗车";
                        break;
                    case UserOrderState.Canceled:
                        coim.OrderState = "已取消";
                        break;
                    case UserOrderState.Deleted:
                        coim.OrderState = "已删除";
                        break;
                    case UserOrderState.Void:
                        coim.OrderState = "已作废";
                        break;
                    default:
                        break;
                }
            }

            coim.WorkMobile = ws.Phone;
            coim.WorkName = ws.Title;
            switch (ws.WorkState)
            {
                case 0:
                    coim.WorkState = "停工";
                    break;
                case 10:
                    coim.WorkState = "空闲";
                    break;
                case 30:
                    coim.WorkState = "忙碌";
                    break;
                case 50:
                    coim.WorkState = "休息";
                    break;
            }
            mm.data = coim;
            mm.code = "1";
            string s = JsonConvert.SerializeObject(mm);
            return Content(s);
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
