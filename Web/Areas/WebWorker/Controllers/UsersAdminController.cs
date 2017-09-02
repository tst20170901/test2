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
using Web.Areas.WebWorker.Models;
using Newtonsoft.Json;
using AliceDAL;

namespace Web.Areas.WebWorker.Controllers
{
    public class UsersAdminController : WebWorkerBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult Index()
        {
            StringBuilder strsql = new StringBuilder();

            AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", WebWorkContext.BranchID));
            AliceDAL.Common.SqlString(strsql, "[OrderState]=10 and [WorkID]=0");
            DataTable dt = DAL.PageModel.DataKeysList("[Orders]", "[ID],[Osn],[UserName],[Mobile],[Plate],[Lng],[Lat],[Remark],[CDate],[Data4]", strsql.ToString(), "ID", 100);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    sb.Append("{\"ID\":" + item["ID"].ToString() + ",\"Osn\":\"" + item["Osn"].ToString() + "\",\"UserName\":\"" + item["UserName"].ToString() + "\"," +
                               "\"Mobile\":\"" + AliceDAL.Common.HideMobile(item["Mobile"].ToString()) + "\",\"Plate\":\"" + item["Plate"] + "\",");
                    if (!String.IsNullOrEmpty(item["Lng"].ToString())) sb.Append("\"Pos\":[" + item["Lng"].ToString() + "," + item["Lat"].ToString() + "],");
                    else sb.Append("\"Pos\":[" + WebWorkContext.Branch.CenterLng + "," + WebWorkContext.Branch.CenterLat + "],");
                    DateTime time = AliceDAL.DataType.ConvertToDateTime(item["CDate"].ToString());
                    #region 时间颜色
                    int m = (int)(DateTime.Now - time).TotalMinutes;
                    string ico = "";
                    if (m <= 5)
                    {
                        ico = "1.png";
                    }
                    else if (m > 5 && m <= 10)
                    {
                        ico = "2.png";
                    }
                    else if (m > 10 && m <= 15)
                    {
                        ico = "3.png";
                    }
                    else if (m > 15 && m <= 20)
                    {
                        ico = "4.png";
                    }
                    else if (m > 20 && m <= 25)
                    {
                        ico = "5.png";
                    }
                    else if (m > 25 && m <= 30)
                    {
                        ico = "6.png";
                    }
                    else
                    {
                        ico = "7.png";
                    }
                    #endregion
                    sb.Append("\"Icon\":\"/Content/images/" + ico + "\",\"Remark\":\"" + item["Remark"].ToString() + item["Data4"].ToString() + "\",\"Details\":\"" + Web.Units.Comm.GetItemNoPrice(item["ID"].ToString()) + "\"},");
                }
            }
            if (sb.Length > 5) sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            ViewBag.OrderPoints = sb.ToString();
            return View();
        }
        public ActionResult GetOrder(int id)
        {
            lock (_locker)
            {
                Orders orderInfo = DAL.Orders.GetOrderInfoByID(id);
                if (orderInfo == null)
                {
                    ViewBag.Message = "订单不存在";
                    ViewBag.BackUrl = Url.Action("Index");
                    return View();
                }
                WorkShop ws = DAL.WorkShop.WorkShopByID(WebWorkContext.ID);
                if (ws.WorkState != 10 && ws.WorkState != 30)
                {
                    ViewBag.Message = "员工未在上班状态";
                    ViewBag.BackUrl = Url.Action("WebInit");
                    return View();
                }
                if (orderInfo.WorkID > 0)
                {
                    WorkShop ws1 = DAL.WorkShop.WorkShopByID(orderInfo.WorkID);
                    ViewBag.Message = "手慢了，订单被" + ws1.Title + "抢走了~~";
                    ViewBag.BackUrl = Url.Action("Index");
                    return View();
                }
                if (orderInfo.OrderState != (int)UserOrderState.Payed)
                {
                    ViewBag.Message = "订单异常，不能抢单";
                    ViewBag.BackUrl = Url.Action("Index");
                    return View();
                }
                if (orderInfo.StoreID != WebWorkContext.BranchID)
                {
                    ViewBag.Message = "不能跨域抢单";
                    ViewBag.BackUrl = Url.Action("Index");
                    return View();
                }
                int c = DAL.PageModel.DataCount("[Orders]", String.Format("[WorkID]={0} and ([OrderState]=10 or [OrderState]=30 or [OrderState]=50)", WebWorkContext.ID));
                if (c > 0)
                {
                    ViewBag.Message = "请完成手中订单后再抢单";
                    ViewBag.BackUrl = Url.Action("Index");
                    return View();
                }

                DAL.Orders.GetWorker(id, WebWorkContext.ID);
                DAL.Orders.ChangeOrder(id, UserOrderState.Assigned);
                MessageManger.OrderAssign(orderInfo.Mobile, ws.Title, ws.Phone);
                DAL.UserOrderActions.Add(new UserOrderActions()
                {
                    Oid = id,
                    Uid = orderInfo.Uid,
                    RealName = "洗车技师(" + WebWorkContext.Title + ")",
                    ActionType = "接单",
                    ActionDes = "技师接单"
                });
                //接单后，状态变忙碌
                DAL.WorkShop.ChangeWorkShop(WebWorkContext.ID, WorkShopState.Busy);
                ViewBag.Message = "抢单成功！";
                ViewBag.BackUrl = Url.Action("OrderInfo", "Orders", new { id = id });
                return View();
            }
        }
        public ActionResult IsFirstOrder()
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

            UserModel um = new UserModel();
            um.Plate = plate;
            Orders orderInfo = DAL.Orders.ExistOrder(plate);
            int userID = 0;
            if (null != orderInfo)
            {
                userID = orderInfo.Uid;
                um.OrderUserName = orderInfo.UserName;
                um.OrderBrandShow = orderInfo.BrandShow;
                um.OrderItem = Comm.GetItemNoPrice(orderInfo.ID.ToString());
                um.OrderMobile = SecureHelper.AESEncrypt(orderInfo.Mobile, "aliceplate");
                um.OrderColor = orderInfo.Color;
                mm.code = "1";
                mm.msg = "此车辆不是首单！";
            }
            else
            {
                mm.msg = "此车辆是首单！";
            }

            DataTable dtuc = DAL.PageModel.DateList("[uv_UserModel_Car]", "[Plate]='" + plate + "'", "ID", 1);
            if (dtuc != null && dtuc.Rows.Count > 0)
            {
                userID = AliceDAL.DataType.ConvertToInt(dtuc.Rows[0]["UserID"].ToString());
                um.BrandShow = dtuc.Rows[0]["BrandShow"].ToString();
                um.Color = dtuc.Rows[0]["Color"].ToString();
                um.Mobile = SecureHelper.AESEncrypt(dtuc.Rows[0]["Mobile"].ToString(), "aliceplate");
                um.Money = AliceDAL.DataType.ConvertToDecimal(dtuc.Rows[0]["Money1"].ToString()) + AliceDAL.DataType.ConvertToDecimal(dtuc.Rows[0]["Money2"].ToString());
                um.UserName = String.IsNullOrEmpty(dtuc.Rows[0]["UserName2"].ToString()) ? dtuc.Rows[0]["UserName1"].ToString() : dtuc.Rows[0]["UserName2"].ToString();
            }

            if (userID > 0)
            {
                List<Web.Models.UserVipCardModel> lstuvcm = Comm.GetVipCard(userID, WebWorkContext.BranchID);
                StringBuilder sb = new StringBuilder("无");
                if (lstuvcm != null && lstuvcm.Count > 0)
                {
                    sb.Clear();
                    foreach (var item in lstuvcm)
                    {
                        sb.Append("<div class=\"couponitem\">" +
                                  "  <div class=\"couponbd\">" +
                                  "    <h1>" + item.Title + " (车牌号：" + item.Plate + ")</h1>" +
                                  (item.CardCount == 99 ? "<div class=\"cishu\">不限次</div>" : "<div class=\"cishu\">可用 " + (item.CardCount - item.UserCount) + " 次 / 共 " + item.CardCount + " 次</div>") +
                                  "    <div class=\"busi\"> " + item.BusinessName + " </div>" +
                                  "    <div class=\"tdate\">有效期：" + item.CDate + "  00:00 -  " + item.TDate + "  23:59</div>" +
                                  "    <div class=\"carddes\"> " + item.VipDes + " </div>" +
                                  "  </div></div>");
                    }

                }
                um.VipCard = sb.ToString();
                List<IGrouping<int, Web.Models.UserCouponsModel>> lstucm = Comm.GetCoupons(userID, WebWorkContext.BranchID);
                StringBuilder sb1 = new StringBuilder("无");
                if (lstucm != null && lstucm.Count > 0)
                {
                    sb1.Clear();
                    foreach (var item in lstucm)
                    {
                        sb1.Append("<div class=\"couponitem\">" +
                                   "  <div class=\"couponbd1\">" +
                                   "    <h1>" + item.ElementAt(0).Title + "<span>（" + item.ElementAt(0).Price + "元）数量：" + item.Count() + "</span></h1>" +
                                   "    <div class=\"date\">" + item.ElementAt(0).CDate + " - " + item.ElementAt(0).TDate + "</div>" +
                                   "  </div></div>");
                    }

                }
                um.Coupons = sb1.ToString();
            }
            mm.data = um;//添加上次检测时间
            string s = JsonConvert.SerializeObject(mm);
            return Content(s);
        }
        public ActionResult WorkNotice()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "提醒失败";
            string plate = AliceDAL.Common.GetFormString("Plate").ToUpper();
            string orderMobile = AliceDAL.Common.GetFormString("OrderMobile");
            string mobile = AliceDAL.Common.GetFormString("Mobile");
            if (!AliceDAL.ValidateHelper.IsCarNo(plate))
            {
                mm.msg = "车牌号码格式不正确！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            string m = "";
            if (!String.IsNullOrEmpty(mobile))
            {
                m = SecureHelper.AESDecrypt(mobile, "aliceplate");
            }
            else if (!String.IsNullOrEmpty(orderMobile))
            {
                m = SecureHelper.AESDecrypt(orderMobile, "aliceplate");
            }
            if (String.IsNullOrEmpty(m) || !ValidateHelper.IsMobile(m))
            {
                mm.msg = "参数不正确，请重新检测！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            NoticeLogs nl = DAL.NoticeLogs.GetNoticeByMobile(m);
            if (nl != null)
            {
                if (nl.CDate.AddMonths(1) > DateTime.Now)
                {
                    mm.msg = "一个月内已经提醒过，请勿重复提醒！";
                    string ss = JsonConvert.SerializeObject(mm);
                    return Content(ss);
                }
            }
            string sms = String.Format("【小熊洗车】小熊特别提醒：您的爱车（{0}）已脏，脏车变新车，感受新汽质。赠加满玻璃水（小熊技师{1}）客服热线：0318-8888235", AliceDAL.Common.HidePlate(plate), WebWorkContext.Phone);
            int result = DAL.NoticeLogs.Add(new NoticeLogs()
            {
                BranchID = WebWorkContext.BranchID,
                Mobile = m,
                Plate = plate,
                SMSContent = sms,
                WorkID = WebWorkContext.ID,
                WorkName = WebWorkContext.Title
            });
            if (result > 0)
            {
                MessageManger.PostMsg(m, sms);
                mm.msg = "提醒成功！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            else
            {
                mm.msg = "提醒失败！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
        }
        public ActionResult WorkNoticeNew()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "提醒失败";
            string plate = AliceDAL.Common.GetFormString("Plate").ToUpper();
            string mobile = AliceDAL.Common.GetFormString("Mobile");
            if (!AliceDAL.ValidateHelper.IsCarNo(plate))
            {
                mm.msg = "车牌号码格式不正确！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            if (String.IsNullOrEmpty(mobile))
            {
                mm.msg = "手机号码不能为空！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            else if (!ValidateHelper.IsMobile(mobile))
            {
                mm.msg = "手机号码不正确！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            NoticeLogs nl = DAL.NoticeLogs.GetNoticeByMobile(mobile);
            if (nl != null)
            {
                if (nl.CDate.AddMonths(1) > DateTime.Now)
                {
                    mm.msg = "一个月内已经提醒过，请勿重复提醒！";
                    string ss = JsonConvert.SerializeObject(mm);
                    return Content(ss);
                }
            }
            string sms = String.Format("【小熊洗车】小熊特别提醒：您的爱车（{0}）已脏，仅10元脏车变新车，感受新汽质。赠加满玻璃水（小熊技师{1}）客服热线：0318-8888235", AliceDAL.Common.HidePlate(plate), WebWorkContext.Phone);
            int result = DAL.NoticeLogs.Add(new NoticeLogs()
            {
                BranchID = WebWorkContext.BranchID,
                Mobile = mobile,
                Plate = plate,
                SMSContent = sms,
                WorkID = WebWorkContext.ID,
                WorkName = WebWorkContext.Title
            });
            if (result > 0)
            {
                MessageManger.PostMsg(mobile, sms);
                mm.msg = "提醒成功！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
            else
            {
                mm.msg = "提醒失败！";
                string ss = JsonConvert.SerializeObject(mm);
                return Content(ss);
            }
        }
        public ActionResult WebInit()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "操作失败";
            WorkShop ws = DAL.WorkShop.WorkShopByID(WebWorkContext.ID);
            if (ws == null)
            {
                mm.msg = "员工不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }

            if (AliceDAL.Common.IsGet())
            {
                ViewBag.WorkState = ws.WorkState;
                return View();
            }
            int onwork = AliceDAL.Common.GetFormInt("OnWork");
            if (onwork != 10 && onwork != 50)
            {
                mm.msg = "参数异常";
                return Content(JsonConvert.SerializeObject(mm));
            }

            if (ws.WorkState == 0)
            {
                mm.code = "2";
                mm.msg = "停工状态不允许修改";
                return Content(JsonConvert.SerializeObject(mm));
            }
            int result = DAL.WorkShop.ChangeWorkShop(WebWorkContext.ID, onwork == 10 ? WorkShopState.Open : WorkShopState.Rest);
            if (result > 0)
            {
                mm.msg = "操作成功";
                //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "WorkState", onwork.ToString(), 527040);
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult PostLocation()
        {
            string Lat = AliceDAL.Common.GetFormString("lat");
            string Lng = AliceDAL.Common.GetFormString("lng");
            MessageModel m = new Models.MessageModel();
            m.code = "0";
            if (String.IsNullOrWhiteSpace(Lat) || String.IsNullOrWhiteSpace(Lng))
            {
                m.code = "0";
                m.msg = "经纬度不能为空";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (DAL.WorkShop.PostLocation(WebWorkContext.ID, Lat, Lng) <= 0)
            {
                m.code = "0";
                m.msg = "fail";
            }
            int oid = DAL.Orders.GetLastestOid(WebWorkContext.ID);
            int cookid = AliceDAL.DataType.ConvertToInt(AliceDAL.Common.GetCookie("alicemaxnewid"));
            if (oid > 0)
            {
                if (oid != cookid)
                {
                    m.code = "1";
                }
            }
            m.data = oid;
            //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Lat", Lat, 527040);
            //AliceDAL.Common.SetCookie("WebWorkerUserInfo", "Lng", Lng, 527040);
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult OrdersAccounting(string txtSDate, string txtEDate)
        {
            StringBuilder strsql = new StringBuilder(String.Format("[StoreID]={0} and [WorkID]>0 and [OrderState]=70", WebWorkContext.BranchID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[FinishDate] between '{0}' and '{1}'", txtSDate, txtEDate));
            }
            else
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[FinishDate] between '{0}' and '{1}'", DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.AddDays(1).ToString("yyyy/MM/dd")));
            }
            string sql = "SELECT o.WorkID, w.Title,o.TotalMoney,w.Phone FROM " +
                         "(select WorkID,SUM(WorkMoney) as TotalMoney from uv_Worker_Orders where " + strsql.ToString() + " group by WorkID) AS o LEFT OUTER JOIN [WorkShop] AS w ON o.WorkID = w.ID  order by o.TotalMoney desc";
            DataTable dt = DAL.PageModel.DataListBySQL(sql);
            return View(dt);
        }
    }
}
