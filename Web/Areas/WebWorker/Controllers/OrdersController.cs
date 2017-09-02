using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Models;
using AliceDAL;
using Newtonsoft.Json;
using System.Data;
using Web.Areas.WebWorker.Models;
using Web.Areas.Models;
using Web.Units;

namespace Web.Areas.WebWorker.Controllers
{
    public class OrdersController : WebWorkerBaseController
    {
        private static object _locker = new object();//锁对象
        [ValidateInput(false)]
        public ActionResult OrdersList1(string txtSDate, string txtEDate)
        {
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[WorkID]={0} and ([OrderState]=10 or [OrderState]=30 or [OrderState]=50)", WebWorkContext.ID));

            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) > DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtSDate).AddDays(1).ToString("yyyy/MM/dd")));
            }
            ListModel.CreatePage(listmodel, "[uv_Worker_Orders]", strsql, 1, 5, "[OrderState] asc,ID");
            return View(listmodel);
        }
        public ActionResult AjaxOrderList1()
        {
            int page = Common.GetFormInt("page");
            string txtSDate = Common.GetFormString("sd");
            string txtEDate = Common.GetFormString("ed");

            MessageModel mm = new MessageModel();
            mm.msg = "没有了";
            mm.code = "0";
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[WorkID]={0} and ([OrderState]=10 or [OrderState]=30 or [OrderState]=50)", WebWorkContext.ID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) > DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtSDate).AddDays(1).ToString("yyyy/MM/dd")));
            }
            ListModel.CreatePage(listmodel, "[uv_Worker_Orders]", strsql, page, 5, "[OrderState] asc,ID");
            if (page > listmodel.Page.TotalPageCount)
            {
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in listmodel.Page)
                {
                    UserOrderState uos = (UserOrderState)AliceDAL.DataType.ConvertToInt(item["OrderState"].ToString());
                    string paystate = "";
                    switch (uos)
                    {
                        case UserOrderState.WaitPaying:
                            paystate = "等待支付";
                            break;
                        case UserOrderState.Payed:
                            paystate = "<span style=\"color:#047548\">支付成功</span>";
                            break;
                        case UserOrderState.Assigned:
                            paystate = "<span style=\"color:#ff6a00\">接单完成</span>";
                            break;
                        case UserOrderState.Started:
                            paystate = "<span style=\"color:#b6ff00\">开始洗车</span>";
                            break;
                        case UserOrderState.Finished:
                            paystate = "<span style=\"color:#00F\">洗车完成</span>";
                            break;
                        case UserOrderState.Canceled:
                            paystate = "<span style=\"color:#CCC\">已取消</span>";
                            break;
                        default:
                            break;
                    }
                    if (AliceDAL.DataType.ConvertToDecimal(item["WorkMoney"].ToString()) > 0)
                    {
                        paystate += "可收益" + item["WorkMoney"].ToString() + "元";
                    }
                    sb.Append("<div class=\"proItem\">" +
                              "  <div class=\"order-msg\"><p class=\"title\">" + item["UserName"].ToString() + "</p><p class=\"mobile\"><a href=\"tel:" + item["Mobile"].ToString() + "\">" + item["Mobile"].ToString() + "</a><span></span></p><p class=\"order-data\">" + AliceDAL.DataType.ConvertToDateTimeStr(item["CDate"]) + "</p><p class=\"order-add\">" + item["Address"].ToString() + "</p></div>" +
                              "  <div class=\"proBt\">" + paystate +
                              "    <a href=\"" + Url.Action("OrderInfo", "Orders", new { id = item["ID"].ToString() }) + "\">查看订单</a>" +
                              "  </div>" +
                              "  <div class=\"h10p\"></div>" +
                              "</div>");
                }
                mm.code = "1";
                mm.msg = "success";
                mm.data = sb.ToString();
            }
            else
            {
                mm.msg = "没有了";
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult OrdersList2(string txtSDate, string txtEDate)
        {
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[WorkID]={0} and [OrderState]=70", WebWorkContext.ID));

            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) > DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("FinishDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtSDate).AddDays(1).ToString("yyyy/MM/dd")));
            }
            ListModel.CreatePage(listmodel, "[uv_Worker_Orders]", strsql, 1, 5, "[OrderState] asc,ID");
            return View(listmodel);
        }
        public ActionResult AjaxOrderList2()
        {
            int page = Common.GetFormInt("page");
            string txtSDate = Common.GetFormString("sd");
            string txtEDate = Common.GetFormString("ed");

            MessageModel mm = new MessageModel();
            mm.msg = "没有了";
            mm.code = "0";
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[WorkID]={0} and [OrderState]=70", WebWorkContext.ID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) > DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("FinishDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtSDate).AddDays(1).ToString("yyyy/MM/dd")));
            }
            ListModel.CreatePage(listmodel, "[uv_Worker_Orders]", strsql, page, 5, "[OrderState] asc,ID");
            if (page > listmodel.Page.TotalPageCount)
            {
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in listmodel.Page)
                {
                    UserOrderState uos = (UserOrderState)AliceDAL.DataType.ConvertToInt(item["OrderState"].ToString());
                    string paystate = "";
                    switch (uos)
                    {
                        case UserOrderState.WaitPaying:
                            paystate = "等待支付";
                            break;
                        case UserOrderState.Payed:
                            paystate = "<span style=\"color:#047548\">支付成功</span>";
                            break;
                        case UserOrderState.Assigned:
                            paystate = "<span style=\"color:#ff6a00\">接单完成</span>";
                            break;
                        case UserOrderState.Started:
                            paystate = "<span style=\"color:#b6ff00\">开始洗车</span>";
                            break;
                        case UserOrderState.Finished:
                            paystate = "<span style=\"color:#00F\">洗车完成</span>";
                            break;
                        case UserOrderState.Canceled:
                            paystate = "<span style=\"color:#CCC\">已取消</span>";
                            break;
                        default:
                            break;
                    }
                    if (AliceDAL.DataType.ConvertToDecimal(item["WorkMoney"].ToString()) > 0)
                    {
                        paystate += "收益" + item["WorkMoney"].ToString() + "元";
                    }
                    sb.Append("<div class=\"proItem\">" +
                              "  <div class=\"order-msg\"><p class=\"title\">" + item["UserName"].ToString() + "</p><p class=\"mobile\"><a href=\"tel:" + item["Mobile"].ToString() + "\">" + item["Mobile"].ToString() + "</a><span></span></p><p class=\"order-data\">" + AliceDAL.DataType.ConvertToDateTimeStr(item["CDate"]) + "</p><p class=\"order-add\">" + item["Address"].ToString() + "</p></div>" +
                              "  <div class=\"proBt\">" + paystate +
                              "    <a href=\"" + Url.Action("OrderInfo", "Orders", new { id = item["ID"].ToString() }) + "\">查看订单</a>" +
                              "  </div>" +
                              "  <div class=\"h10p\"></div>" +
                              "</div>");
                }
                mm.code = "1";
                mm.msg = "success";
                mm.data = sb.ToString();
            }
            else
            {
                mm.msg = "没有了";
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult OrderInfo(int id)
        {
            OrderInfoModel om = new OrderInfoModel();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(id);
            if (orderInfo == null || orderInfo.WorkID != WebWorkContext.ID)
            {
                ViewBag.Message = "订单不存在";
                ViewBag.BackUrl = Url.Action("OrdersList1");
                return View();
            }
            if (orderInfo.OrderState == 10)
            {
                AliceDAL.Common.SetCookie("alicemaxnewid", id.ToString(), 527040);
            }
            om.OrderInfo = orderInfo;
            om.OrderItems = DAL.Orders.GetItemListByOrderID(id);
            return View(om);
        }
        public ActionResult CarInfo(int id)
        {
            CarInfo carInfo = new Models.CarInfo();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(id);
            UserCar uu = DAL.UserCar.UserExist(orderInfo.Uid, orderInfo.Plate);
            if (orderInfo == null || orderInfo.WorkID != WebWorkContext.ID)
            {
                ViewBag.Message = "订单不存在";
                ViewBag.BackUrl = Url.Action("OrdersList2");
                return View("MessageShow");
            }
            if (AliceDAL.Common.IsGet())
            {

                carInfo.OrderID = orderInfo.ID;
                carInfo.BrandShow = orderInfo.BrandShow;
                carInfo.Color = orderInfo.Color;
                carInfo.DueTime = "";
                carInfo.Plate = orderInfo.Plate;
                carInfo.SafeCompany = "";
                if (uu != null && (String.IsNullOrEmpty(uu.Data4) || uu.Data4 == WebWorkContext.Mobile))
                {
                    carInfo.SafeCompany = uu.Data2;
                    carInfo.DueTime = uu.Data3;
                }
                return View(carInfo);
            }
            int oid = AliceDAL.Common.GetFormInt("oid");
            string plate = AliceDAL.Common.GetFormString("plate");
            string brandshow = AliceDAL.Common.GetFormString("bss");
            string color = AliceDAL.Common.GetFormString("color");
            string company = AliceDAL.Common.GetFormString("company");
            string duetime = AliceDAL.Common.GetFormString("duetime");
            string vinno = AliceDAL.Common.GetFormString("vinno");
            DAL.Orders.EditOrderCar(oid, brandshow, color, plate);
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "保存失败";
            if (uu != null && !String.IsNullOrEmpty(uu.Data4) && uu.Data4 != WebWorkContext.Mobile)
            {
                mm.msg = "订单车型保存成功，保险信息已由其他洗车工录入";
                return Content(JsonConvert.SerializeObject(mm));
            }
            UserCar uc = new UserCar();
            uc.BrandID = uu == null ? -1 : uu.BrandID;
            uc.BrandShow = brandshow;
            uc.Color = color;
            uc.Data1 = uu == null ? "" : uu.Data1;
            uc.Data2 = company;//保险公司
            uc.Data3 = duetime;//到期时间
            uc.Data4 = WebWorkContext.Mobile;
            uc.Data5 = vinno;//大架号
            uc.Plate = plate;
            uc.ModelID = uu == null ? -1 : uu.ModelID;
            uc.UserID = orderInfo.Uid;
            uc.UserName = orderInfo.UserName;
            int i = DAL.UserCar.EditSafeCompany(uc);

            if (i > 0)
            {
                mm.code = "1";
                mm.msg = "保存成功";
            }
            return Content(JsonConvert.SerializeObject(mm));

        }
        public ActionResult TrunOrder()
        {
            MessageModel m = new Models.MessageModel();
            int oid = Common.GetFormInt("oid");
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WebWorkContext.ID)
            {
                m.code = "0";
                m.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (orderInfo.OrderState != (int)UserOrderState.Assigned)
            {
                m.code = "0";
                m.msg = "状态异常，无法流转";
                return Content(JsonConvert.SerializeObject(m));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Payed);
            DAL.Orders.GetWorker(oid, 0);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = orderInfo.Uid,
                RealName = "洗车技师(" + WebWorkContext.Title + ")",
                ActionType = "订单流转",
                ActionDes = "订单流转"
            });
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult AssignOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            DAL.Orders.ChangeOrder(oid, UserOrderState.Assigned);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = "洗车技师(" + WebWorkContext.Title + ")",
                ActionType = "接单",
                ActionDes = "技师接单"
            });
            return RedirectToAction("OrderInfo", new { id = oid });
        }
        public ActionResult StartOrder()
        {
            MessageModel m = new Models.MessageModel();
            int oid = Common.GetFormInt("oid");
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WebWorkContext.ID)
            {
                m.code = "0";
                m.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (orderInfo.OrderState != (int)UserOrderState.Assigned)
            {
                m.code = "0";
                m.msg = "状态异常，无法开始洗车";
                return Content(JsonConvert.SerializeObject(m));
            }
            string result = AliceDAL.Common.GetFormString("images").TrimEnd(',');
            if (String.IsNullOrWhiteSpace(result))
            {
                m.code = "0";
                m.msg = "请选择照片";
                return Content(JsonConvert.SerializeObject(m));
            }
            string[] img = result.Split(',');
            if (img.Length <= 0)
            {
                m.code = "0";
                m.msg = "请选择照片";
                return Content(JsonConvert.SerializeObject(m));
            }
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < img.Length; i++)
                {
                    sb.Append(ImageBase64Helper.Base64StringToImage(img[i]) + ",");
                }
            }
            catch
            {
                m.code = "0";
                m.msg = "所选图片异常";
                return Content(JsonConvert.SerializeObject(m));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Started);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = orderInfo.Uid,
                RealName = "洗车技师(" + WebWorkContext.Title + ")",
                ActionType = "开始洗车",
                ActionDes = "开始洗车"
            });
            DAL.Orders.UpdateStartImg(oid, sb.ToString().TrimEnd(','));
            m.msg = "开始洗车";
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult FinishOrder()
        {
            MessageModel m = new Models.MessageModel();
            int oid = Common.GetFormInt("oid");
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WebWorkContext.ID)
            {
                m.code = "0";
                m.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (orderInfo.OrderState != (int)UserOrderState.Started)
            {
                m.code = "0";
                m.msg = "状态异常，无法完成洗车";
                return Content(JsonConvert.SerializeObject(m));
            }
            string result = AliceDAL.Common.GetFormString("images").TrimEnd(',');
            if (String.IsNullOrWhiteSpace(result))
            {
                m.code = "0";
                m.msg = "请选择照片";
                return Content(JsonConvert.SerializeObject(m));
            }
            string[] img = result.Split(',');
            if (img.Length <= 0)
            {
                m.code = "0";
                m.msg = "请选择照片";
                return Content(JsonConvert.SerializeObject(m));
            }
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < img.Length; i++)
                {
                    sb.Append(ImageBase64Helper.Base64StringToImage(img[i]) + ",");
                }
            }
            catch
            {
                m.code = "0";
                m.msg = "所选图片异常";
                return Content(JsonConvert.SerializeObject(m));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Finished);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = orderInfo.Uid,
                RealName = "洗车技师(" + WebWorkContext.Title + ")",
                ActionType = "洗车完成",
                ActionDes = "洗车完成"
            });
            DAL.Orders.UpdateEndImg(oid, sb.ToString().TrimEnd(','));
            //洗车完成，变为空闲
            DAL.WorkShop.ChangeWorkShop(WebWorkContext.ID, WorkShopState.Open);
            Web.Units.MessageManger.OrderFinish(orderInfo.Mobile);
            m.msg = "洗车完成";
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult OrdersAdd()
        {
            if (AliceDAL.Common.IsGet())
            {
                List<Wash_Item> list = DAL.Wash_Item.GetList("[BranID]=" + WebWorkContext.BranchID + " and [ItemState]=10 and ([Online]=10 or [Online]=30)");
                Models.OrderInfoModel oim = new OrderInfoModel();
                oim.WashItems = list;
                return View(oim);
            }
            lock (_locker)
            {
                MessageModel mm = new MessageModel();
                mm.msg = "提交失败";
                mm.code = "0";
                string mobile = AliceDAL.Common.GetFormString("mobile").Replace(" ", "");
                string plate = AliceDAL.Common.GetFormString("plate").Replace(" ", "");
                string username = AliceDAL.Common.GetFormString("username");
                string[] itemid = Common.GetFormString("itemIds").Split(',');
                if (String.IsNullOrEmpty(mobile) || !ValidateHelper.IsMobile(mobile))
                {
                    mm.msg = "手机号码不正确";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                if (String.IsNullOrEmpty(username))
                {
                    mm.msg = "昵称不能为空";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                if (String.IsNullOrEmpty(plate) || !ValidateHelper.IsMobile(mobile))
                {
                    mm.msg = "手机号码不正确";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                if (itemid.Length <= 0)
                {
                    mm.msg = "请选择服务项目";
                    return Content(JsonConvert.SerializeObject(mm));
                }

                Orders order = new Orders();
                int uid = 0;
                BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
                if (bu == null)
                {
                    order.Data3 = "手动线下";
                    BD_Users user = new BD_Users();
                    user.Data1 = username;
                    user.Data2 = "1";
                    user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";
                    user.LoginID = mobile;
                    user.Mobile = mobile;
                    user.UserPwd = AliceDAL.SecureHelper.MD5("123456");
                    string result = DAL.BD_Users.Add(user);
                    order.Uid = AliceDAL.DataType.ConvertToInt(result);
                    uid = order.Uid;
                }
                else
                {
                    order.Uid = bu.ID;
                    uid = bu.ID;
                }
                #region 订单实体
                order.BrandID = -1;
                order.BrandShow = "";
                order.CDate = DateTime.Now;
                order.StoreID = WebWorkContext.BranchID;
                order.Color = "";
                order.WorkID = WebWorkContext.ID;
                order.IP = Common.GetIP();
                order.Lat = WebWorkContext.Lat;
                order.Lng = WebWorkContext.Lng;
                order.Mobile = mobile;
                order.ModelID = -1;
                order.OrderState = (int)UserOrderState.Finished;
                order.Osn = "WO" + DAL.Orders.GenerateOSN(WebWorkContext.ID);
                order.Plate = plate;
                order.UserName = username;
                order.Remark = WebWorkContext.Title + WebWorkContext.Mobile + "线下录入";
                order.Data3 = "手动线下";
                order.FinishDate = DateTime.Now;
                order.Address = "";
                order.PayName = "钱包";
                order.PaySn = "";
                order.PayTime = DateTime.Now;
                List<Orders_Item> listitem = new List<Orders_Item>();
                foreach (var item in itemid)
                {
                    Wash_Item wi = DAL.Wash_Item.GetModel(AliceDAL.DataType.ConvertToInt(item));
                    if (wi != null)
                    {
                        order.Amount += wi.Price;
                        Orders_Item oi = new Orders_Item()
                        {
                            CDate = DateTime.Now,
                            ItemID = wi.ID,
                            ItemName = wi.Title,
                            Money = wi.Price,
                            StoreID = WebWorkContext.BranchID,
                            UserID = order.Uid,
                            WorkPrice = wi.WorkPrice
                        };
                        listitem.Add(oi);
                    }
                }
                int oid = DAL.Orders.CreateNewOrder(order, listitem);
                mm.code = "1";
                mm.msg = "下单成功";
                return Content(JsonConvert.SerializeObject(mm));
                #endregion
            }
        }
    }
}
