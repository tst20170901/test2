﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using AliceDAL;
using Models;
using System.Text;
using Newtonsoft.Json;
namespace Web.Controllers
{
    public class ProductController : UserBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult ProductOrder()
        {
            Pro_Case model = DAL.Pro_Case.GetModel(AliceDAL.UrlParam.GetIntValue("pid"));
            if (model == null)
            {
                ViewBag.Message = "商品不存在";
                ViewBag.BackUrl = Url.Action("ProductList", "BearMall");
                return View("MessageShow");
            }
            Models.ProductIndexModel pim = new Models.ProductIndexModel();
            pim.OrderInfo = DAL.Pro_Orders.GetLastestOrder(WorkContext.ID);
            pim.Money = WorkContext.wallet.Money1 + WorkContext.wallet.Money2;
            pim.Product = model;
            return View(pim);
        }
        public ActionResult ProductOrderPost()
        {
            lock (_locker)
            {
                #region 收集提交数据
                Models.MessageModel mm = new Models.MessageModel();
                mm.msg = "商品不存在";
                mm.code = "0";
                Pro_Case model = DAL.Pro_Case.GetModel(Common.GetFormInt("pid"));
                if (model == null)
                {
                    return Content(JsonConvert.SerializeObject(mm));
                }
                string mobile = Common.GetFormString("mobile");
                string name = Common.GetFormString("name");
                string address = Common.GetFormString("address");
                int paymode = Common.GetFormInt("paymode");
                string comment = Common.GetFormString("comment");
                #endregion

                #region 订单实体
                Pro_Orders order = new Pro_Orders();
                order.BrandID = -1;
                order.BrandShow = "";
                order.CDate = DateTime.Now;
                order.StoreID = 1;
                order.Color = "";
                order.WorkID = 0;// workID;
                order.BusinessID = model.BusinessID;
                order.IP = Common.GetIP();
                order.Lat = "";
                order.Lng = "";
                order.Mobile = mobile;
                order.ModelID = -1;
                order.OrderState = (int)UserProOrderState.WaitPaying;
                order.Osn = "Pro_" + DAL.Orders.GenerateOSN(WorkContext.ID);
                order.Plate = "";
                order.Uid = WorkContext.ID;
                order.UserName = name;
                order.Remark = "";
                order.FinishDate = DateTime.Now.AddHours(1);
                order.Address = address;
                switch (paymode)
                {
                    case 10:
                        order.PayName = "钱包";
                        break;
                    case 30:
                        order.PayName = "支付宝";
                        break;
                    default:
                        order.PayName = "微信";
                        break;
                }
                order.PaySn = "";
                order.PayTime = new DateTime(1900, 1, 1);
                #endregion

                #region 订单项目计算
                List<Pro_Orders_Item> listitem = new List<Pro_Orders_Item>();
                order.Amount = model.Price;
                Pro_Orders_Item oi = new Pro_Orders_Item()
                {
                    CDate = DateTime.Now,
                    ItemID = model.ID,
                    ItemName = model.Title,
                    Money = model.Price,
                    StoreID = 1,
                    UserID = WorkContext.ID,
                    WorkPrice = model.Price
                };
                listitem.Add(oi);

                if (listitem.Count <= 0)
                {
                    mm.msg = "请选择商品";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                #endregion

                #region 订单实付金额及钱包判断
                decimal all = order.Amount;
                if (paymode == 10)
                {
                    if (order.Amount > WorkContext.wallet.Money1 + WorkContext.wallet.Money2)
                    {
                        mm.msg = "钱包余额不足，请选择其他支付方式";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                }
                order.Data4 = comment;
                #endregion
                int oid = DAL.Pro_Orders.CreateNewOrder(order, listitem);

                #region 支付方式判断
                if (paymode == 10)
                {
                    if (WorkContext.wallet.Money2 >= order.Amount)//优先使用赠额
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.wallet.Money1, WorkContext.wallet.Money2 - order.Amount);
                    }
                    else if (WorkContext.wallet.Money1 >= order.Amount)
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.wallet.Money1 - order.Amount, WorkContext.wallet.Money2);
                    }
                    else
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.wallet.Money1 - (order.Amount - WorkContext.wallet.Money2), 0);
                    }
                    DAL.Pro_Orders.PayOrder(oid, UserProOrderState.Payed, "", DateTime.Now);
                    DAL.BD_Consumer.Add(new BD_Consumer()
                   {
                       Money = -order.Amount,
                       OrderID = oid,//Orders ID
                       Remark = "钱包支付",
                       UserID = order.Uid
                   });
                    DAL.UserOrderActions.Add(new UserOrderActions()
                    {
                        Oid = 0,
                        POid = oid,
                        Uid = order.Uid,
                        RealName = "本人",
                        ActionType = "支付",
                        ActionDes = "你使用钱包完成支付"
                    });
                    MessageManger.ProOrderSuccess(order.BusinessID, order.Osn, oid);
                }
                else if (paymode == 30)
                {
                    mm.code = "1";
                    mm.msg = "zfb";
                    mm.data = oid;
                    return Content(JsonConvert.SerializeObject(mm));
                }
                else
                {
                    mm.code = "1";
                    mm.msg = "wx";
                    mm.data = oid;
                    return Content(JsonConvert.SerializeObject(mm));
                }
                #endregion
                mm.code = "1";
                mm.msg = "qb";
                mm.data = oid;
                return Content(JsonConvert.SerializeObject(mm));
            }
        }
        public ActionResult PayResult()
        {
            Models.ProPayResultModel pr = new Models.ProPayResultModel();
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders orderInfo = DAL.Pro_Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.Uid != WorkContext.ID)
                return View(pr);
            pr.OrderInfo = orderInfo;
            pr.Orders_Items = DAL.Pro_Orders.GetItemListByOrderID(oid);
            return View(pr);
        }
    }
}