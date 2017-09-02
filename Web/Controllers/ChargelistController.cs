using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using System.Net;
using System.IO;
using System.Text;
using Web.Models;
using Newtonsoft.Json;
using Models;
namespace Web.Controllers
{
    public class ChargelistController : UserBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult Index()
        {
            List<ChargeType> list = DAL.ChargeType.GetList("[State]=10");
            ChargeTypeListModel clm = new ChargeTypeListModel();
            clm.list = list;
            return View(clm);
        }

        public ActionResult List()
        {
            UserConsumerListModel uclm = new UserConsumerListModel();
            List<BD_Consumer> list = DAL.BD_Consumer.GetListByUserID(WorkContext.ID);
            uclm.List = list;
            return View(uclm);
        }
        public ActionResult ListMoveCar()
        {
            DataTable dt = DAL.PageModel.DateList("[Edo_CheckLogs]", "[Mobile]='"+WorkContext.LoginID+"'", "[ID]", 0);
            return View(dt);
        }
        public ActionResult ChongZhi()
        {
            lock (_locker)
            {
                string state = "error";
                string msg = "";
                string cardno = AliceDAL.Common.GetFormString("cardno");
                string cardpwd = AliceDAL.Common.GetFormString("cardpwd");
                string plate = AliceDAL.Common.GetFormString("plate");
                if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(cardno)))
                {
                    msg = "请输入卡号";
                    return AjaxResult(state, msg);
                }
                else if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(cardpwd)))
                {
                    msg = "请输入密码";
                    return AjaxResult(state, msg);
                }
                else if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(plate)))
                {
                    msg = "请输入车牌号";
                    return AjaxResult(state, msg);
                }
                else if (!AliceDAL.ValidateHelper.IsCarNo(plate))
                {
                    msg = "车牌号不正确";
                    return AjaxResult(state, msg);
                }

                string result = "";
                VipCard vc = DAL.VipCard.Exchange(cardno, cardpwd, out result);
                if (vc != null && vc.CardNo == cardno)
                {
                    if (vc.Uid > 0 || vc.CardState == 30)
                    {
                        msg = "会员卡已被使用";
                        return AjaxResult(state, msg);
                    }
                    DateTime NewTDate = DateTime.Now.AddDays((vc.TDate - vc.CDate).Days);//根据有效期获取天数，修改有效期，从开卡日算起
                    if (DAL.VipCard.ChargeToUser(vc.ID, WorkContext.ID, WorkContext.Mobile, plate.ToUpper(), NewTDate) > 0)
                    {
                        state = "success";
                        msg = "充值成功";
                        MessageManger.SendMsg(WorkContext.Mobile, vc.SMSContent);
                        return AjaxResult(state, msg);
                    }
                    else
                    {
                        msg = "充值失败，请稍候再试";
                        return AjaxResult(state, msg);
                    }
                }
                else
                {
                    return AjaxResult(state, result);
                }
            }
        }

        public ActionResult AddChargeOrder()
        {
            lock (_locker)
            {
                string state = "error";
                string msg = "";
                int typeID = AliceDAL.Common.GetFormInt("typeID");
                ChargeType ct = DAL.ChargeType.GetModel(typeID);
                if (ct == null) msg = "套餐不存在";
                AdminOrders order = new AdminOrders();
                order.OrderAmount = ct.Price;
                order.OrderState = (int)OrderState.WaitPaying;
                order.Osn = DAL.AdminOrders.GenerateOSN(WorkContext.ID);
                order.TypeID = typeID;
                order.Uid = WorkContext.ID;
                order.Count = 1;
                int oid = DAL.AdminOrders.CreateOrder(order);
                if (oid > 0)
                {
                    state = "success";
                    msg = Url.Action("Pay", "Alipay", new { oid = oid });
                }
                else
                {
                    msg = "购买失败";
                }
                return AjaxResult(state, msg);
            }
        }
        public ActionResult AddChargeOrderWX()
        {
            lock (_locker)
            {
                string state = "error";
                string msg = "";
                AdminOrders order = new AdminOrders();

                int typeID = AliceDAL.Common.GetFormInt("typeID");
                int p = AliceDAL.Common.GetFormInt("price");
                ChargeType ct = DAL.ChargeType.GetModel(typeID);
                if (typeID == 999)
                {
                    if (p <= 0)
                    {
                        msg = "充值金额不正确";
                        return AjaxResult(state, msg);
                    }
                    order.OrderAmount = p;
                }
                else
                {
                    if (ct == null) msg = "套餐不存在";
                    order.OrderAmount = ct.Price;
                }
                order.OrderState = (int)OrderState.WaitPaying;
                order.Osn = DAL.AdminOrders.GenerateOSN(WorkContext.ID);
                order.TypeID = typeID;
                order.Uid = WorkContext.ID;
                order.Count = 1;
                order.PayName = "微信";
                int oid = DAL.AdminOrders.CreateOrder(order);
                if (oid > 0)
                {
                    state = "success";
                    msg = Url.Action("Pay", "WeiXinPay", new { oid = oid });
                }
                else
                {
                    msg = "购买失败";
                }
                return AjaxResult(state, msg);
            }
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { state = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}