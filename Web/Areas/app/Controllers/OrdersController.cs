using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using Models;
using Web.Areas.app.Models;
using AliceDAL;
using Newtonsoft.Json;
using System.Data;

namespace Web.Areas.app.Controllers
{
    public class OrdersController : AppBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult GetCarList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无车辆";
            List<UserCar> list = DAL.UserCar.GetList(WorkContext.ID);
            if (list != null && list.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult CarAdd()
        {
            MessageModel mm = new MessageModel();
            string plate = AliceDAL.Common.GetFormString("aliceplate");
            string brand = AliceDAL.Common.GetFormString("alicebrand");
            string model = AliceDAL.Common.GetFormString("alicemodel");
            string color = AliceDAL.Common.GetFormString("alicecolor");
            string username = AliceDAL.Common.GetFormString("aliceusername");
            int brandid = AliceDAL.Common.GetFormInt("alicebrandid");
            int modelid = AliceDAL.Common.GetFormInt("alicemodelid");
            if (String.IsNullOrWhiteSpace(plate))
            {
                mm.code = "0";
                mm.msg = "车牌号不能为空";
                return Content(JsonConvert.SerializeObject(mm));
            }
            else if (!ValidateHelper.IsCarNo(plate))
            {
                mm.code = "0";
                mm.msg = "车牌号不正确";
                return Content(JsonConvert.SerializeObject(mm));
            }
            UserCar uc = new UserCar()
            {
                BrandID = brandid,
                BrandShow = brand + " " + model,
                Color = color ?? "",
                Data1 = "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = modelid,
                Plate = plate,
                UserID = WorkContext.ID,
                UserName = String.IsNullOrWhiteSpace(username) ? WorkContext.Data1 : username
            };
            string result = DAL.UserCar.Add(uc);
            if (result == "exists")
            {
                mm.code = "0";
                mm.msg = "此车辆已经存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult CarEdit()
        {
            MessageModel mm = new MessageModel();
            string plate = AliceDAL.Common.GetFormString("aliceplate");
            string brand = AliceDAL.Common.GetFormString("alicebrand");
            string model = AliceDAL.Common.GetFormString("alicemodel");
            string color = AliceDAL.Common.GetFormString("alicecolor");
            string username = AliceDAL.Common.GetFormString("aliceusername");
            int brandid = AliceDAL.Common.GetFormInt("alicebrandid");
            int modelid = AliceDAL.Common.GetFormInt("alicemodelid");
            int aliceid = AliceDAL.Common.GetFormInt("aliceid");
            UserCar uc = DAL.UserCar.GetModel(aliceid);
            if (uc == null)
            {
                mm.code = "0";
                mm.msg = "此车辆不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            else if (String.IsNullOrWhiteSpace(plate))
            {
                mm.code = "0";
                mm.msg = "车牌号不能为空";
                return Content(JsonConvert.SerializeObject(mm));
            }
            else if (!ValidateHelper.IsCarNo(plate))
            {
                mm.code = "0";
                mm.msg = "车牌号不正确";
                return Content(JsonConvert.SerializeObject(mm));
            }
            UserCar ucmodel = new UserCar()
            {
                BrandID = brandid,
                BrandShow = brand + " " + model,
                Color = color,
                Data1 = "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = modelid,
                Plate = plate,
                UserID = WorkContext.ID,
                UserName = String.IsNullOrWhiteSpace(username) ? WorkContext.Data1 : username,
                ID = uc.ID
            };
            string result = DAL.UserCar.Edit(ucmodel);
            if (result == "exists")
            {
                mm.code = "0";
                mm.msg = "此车牌号已经存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult CarDelete()
        {
            MessageModel mm = new MessageModel();
            int aliceid = AliceDAL.Common.GetFormInt("aliceid");
            UserCar uc = DAL.UserCar.GetModel(aliceid);
            if (uc == null || uc.UserID != WorkContext.ID)
            {
                mm.code = "0";
                mm.msg = "此车辆不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            int result = DAL.PageModel.Delete("UserCar", "ID=" + aliceid.ToString());
            if (result <= 0)
            {
                mm.code = "0";
                mm.msg = "删除失败，请稍候再试";
                return Content(JsonConvert.SerializeObject(mm));
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult GetCarModelList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无品牌";
            List<Web.Models.CarsBrand> lis = CacheManager.Get(CacheKeys.CARMODELSLIST) as List<Web.Models.CarsBrand>;
            if (lis == null || lis.Count <= 0)
            {
                List<CarModels> list = DAL.CarModels.GetList();
                lis = new List<Web.Models.CarsBrand>();

                List<CarModels> temp = list.FindAll(x => x.ParentID == 0);
                foreach (var item in temp)
                {
                    Web.Models.CarsBrand c = new Web.Models.CarsBrand();
                    c.id = item.ID;
                    c.name = item.Name;
                    c.py = item.PinYin;
                    List<Web.Models.Cars> ls = new List<Web.Models.Cars>();
                    foreach (var i in list.FindAll(x => x.ParentID == item.ID))
                    {
                        Web.Models.Cars car = new Web.Models.Cars() { id = i.ID, name = i.Name, py = i.PinYin };
                        ls.Add(car);
                    }
                    c.models = ls;
                    lis.Add(c);
                }
                CacheManager.Insert(CacheKeys.CARMODELSLIST, lis);
            }
            if (lis != null && lis.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = lis;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult ChargeTypeList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无充值套餐";
            List<ChargeType> list = DAL.ChargeType.GetList("[State]=10");
            if (list != null && list.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult OrderList(int orderstate = -1, int page = 1)
        {
            MessageModel mm = new MessageModel();
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[Uid]={0} and [OrderState]<>0 and [OrderState]<>110", WorkContext.ID));
            if (orderstate >= 0) strsql.Append(String.Format(" and OrderState={0}", orderstate));
            else strsql.Append(String.Format(" and OrderState<>70", orderstate));
            ListModel.CreatePage(listmodel, "[uv_Orders]", strsql, page, 10);

            List<OrderModel> list = new List<OrderModel>();
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                foreach (DataRow item in listmodel.Page)
                {
                    OrderModel om = new OrderModel();
                    om.CDate = AliceDAL.DataType.ConvertToDateTimeStrAll(item["CDate"]);
                    om.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                    om.Mobile = item["Mobile"].ToString();
                    om.OrderState = AliceDAL.DataType.ConvertToInt(item["OrderState"].ToString());
                    om.Osn = item["Osn"].ToString();
                    om.UserName = item["UserName"].ToString();
                    om.ItemName = item["ItemName"].ToString();
                    om.Uid = AliceDAL.DataType.ConvertToInt(item["Uid"].ToString());
                    om.BookDate = item["Remark"].ToString();
                    om.Plate = item["Plate"].ToString().Substring(0, 7);
                    list.Add(om);
                }
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult OrderItems()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无服务项目";
            List<Wash_Item> list = DAL.Wash_Item.GetList("ItemState=10");
            if (list != null && list.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult OrderDetail()
        {
            int oid = AliceDAL.Common.GetFormInt("aliceoid");
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "订单不存在";
            Models.OrderInfoModel oim = new Models.OrderInfoModel();
            Orders OrderInfo = DAL.Orders.GetOrderInfoByID(oid);
            List<UserOrderActions> list = DAL.UserOrderActions.GetListByOrderID(oid,0);
            oim.UserActionsList = list;
            if (OrderInfo != null)
            {
                oim.ID = OrderInfo.ID;
                oim.Osn = OrderInfo.Osn;
                oim.PicBegin = OrderInfo.Data1;
                oim.PicAfter = OrderInfo.Data2;
                mm.code = "1";
                mm.msg = "success";
                mm.data = oim;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult OrderDel()
        {
            int oid = UrlParam.GetIntValue("aliceoid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            MessageModel mm = new MessageModel();
            if (order == null)
            {
                mm.code = "0";
                mm.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (order.OrderState != (int)UserOrderState.Canceled)
            {
                mm.code = "0";
                mm.msg = "订单未取消不能删除";
                return Content(JsonConvert.SerializeObject(mm));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Deleted);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = "本人",
                ActionType = "删除",
                ActionDes = "删除订单"
            });
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult Cancel()
        {
            int oid = UrlParam.GetIntValue("aliceoid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            MessageModel mm = new MessageModel();
            if (order == null)
            {
                mm.code = "0";
                mm.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (order.OrderState == (int)UserOrderState.Canceled)
            {
                mm.code = "0";
                mm.msg = "订单已取消";
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (order.OrderState != (int)UserOrderState.Payed)
            {
                mm.code = "0";
                mm.msg = "订单不能取消";
                return Content(JsonConvert.SerializeObject(mm));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Canceled);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = "本人",
                ActionType = "取消",
                ActionDes = "取消订单"
            });
            DAL.BD_Consumer.Add(new BD_Consumer()
            {
                CDate = DateTime.Now,
                Money = order.Amount,
                OrderID = order.ID,
                Remark = "退款",
                UserID = WorkContext.ID
            });
            DAL.BD_Users.EditUserWalletByUserID(WorkContext.ID, order.Amount, 0);
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult PreOrder()
        {
            MessageModel mm = new MessageModel();
            string lng = AliceDAL.Common.GetFormString("alicelng");
            string lat = AliceDAL.Common.GetFormString("alicelat");
            string msg = "附近没有可用服务";
            mm.code = "0";
            AddressModel m = AMap.Common.GetAddress(lng, lat);
            if (m != null && m.CityCode != "" && m.AdCode != "")
            {
                List<WorkShop> list = DAL.WorkShop.WorkShopForCode(m.CityCode, m.AdCode, (int)WorkShopState.Open);
                if (list != null)
                {
                    AMap.Common.InitDistance(list, lng, lat);
                    WorkShop ws = list.OrderBy(x => x.Distan).First();
                    if (ws != null)
                    {
                        if (ws.Distan <= ws.WorkRadius)
                        {
                            int t = TimeHelper.IsInTime(ws.StartTime, ws.EndTime);
                            if (t == 1)
                            {
                                msg = "预计今日" + DateTime.Now.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            else if (t == 2)
                            {
                                msg = "预计明日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            else
                            {
                                msg = "预计今日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            mm.code = "1";
                        }
                        else
                        {
                            msg = "超出服务范围";
                        }
                    }
                }
            }
            mm.msg = msg;
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult SubmitOrder()
        {
            MessageModel mm = new MessageModel();
            mm.msg = "新版APP紧急升级中，请使用微信下单！客服电话0318-8888235";
            mm.code = "0";
            return Content(JsonConvert.SerializeObject(mm));
            //lock (_locker)
            //{
            //    bool isFir = false;

            //    string[] itemid = Common.GetFormString("itemids").Split(',');//服务项目ID，以英文逗号隔开
            //    string[] couponid = Common.GetFormString("couponids").Split(',');//优惠券ID，以英文逗号隔开
            //    string lng = Common.GetFormString("alicelng");
            //    string lat = Common.GetFormString("alicelat");

            //    string mobile = Common.GetFormString("mobile");//手机号
            //    string name = Common.GetFormString("username");//用户姓名
            //    string address = Common.GetFormString("address");//详细地址
            //    int carid = Common.GetFormInt("carid"); //所选车辆
            //    string bookdate = Common.GetFormString("bookdate");//预定时间

            //    if (AliceDAL.IniHelper.GetValue("WebInfo", "Data7") == "1")
            //    {
            //        msg = AliceDAL.IniHelper.GetValue("WebInfo", "Data8");
            //        mm.msg = msg;
            //        return Content(JsonConvert.SerializeObject(mm));
            //    }

            //    UserCar uc = DAL.UserCar.GetModel(carid);
            //    if (uc == null || !ValidateHelper.IsCarNo(uc.Plate))
            //    {
            //        msg = "车辆不存在";
            //        mm.msg = msg;
            //        return Content(JsonConvert.SerializeObject(mm));
            //    }
            //    if (WorkContext.Data2 == "2")
            //    {
            //        UserCar uc1 = DAL.UserCar.UserExist(WorkContext.ID, uc.Plate.ToUpper());
            //        if (uc == null || !ValidateHelper.IsCarNo(uc.Plate))
            //        {
            //            msg = "该账户只能清洗特定车辆";
            //            mm.msg = msg;
            //            return Content(JsonConvert.SerializeObject(mm));
            //        }
            //    }

            //    if (itemid.Length <= 0)
            //    {
            //        msg = "请选择服务项目";
            //        mm.msg = msg;
            //        return Content(JsonConvert.SerializeObject(mm));
            //    }
            //    int workID = 0;
            //    msg = "附近没有可用服务";
            //    DateTime finishtime = DateTime.Now;
            //    AddressModel m = AMap.Common.GetAddress(lng, lat);
            //    if (m != null && m.CityCode != "" && m.AdCode != "")
            //    {
            //        List<WorkShop> list = DAL.WorkShop.WorkShopForCode(m.CityCode, m.AdCode, (int)WorkShopState.Open);
            //        if (list != null)
            //        {
            //            AMap.Common.InitDistance(list, lng, lat);
            //            WorkShop ws = list.OrderBy(x => x.Distan).First();
            //            if (ws != null)
            //            {
            //                if (ws.Distan <= ws.WorkRadius)
            //                {
            //                    int t = TimeHelper.IsInTime(ws.StartTime, ws.EndTime);
            //                    if (t == 1)
            //                    {
            //                        msg = "预计今日" + DateTime.Now.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                    }
            //                    else if (t == 2)
            //                    {
            //                        msg = "预计明日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                    }
            //                    else
            //                    {
            //                        msg = "预计今日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                    }
            //                    workID = ws.ID;
            //                }
            //                else
            //                {
            //                    msg = "超出服务范围";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            list = DAL.WorkShop.WorkShopForCode(m.CityCode, m.AdCode, (int)WorkShopState.Busy);
            //            if (list != null)
            //            {
            //                AMap.Common.InitDistance(list, lng, lat);
            //                WorkShop ws = list.OrderBy(x => x.Distan).First();
            //                if (ws != null)
            //                {
            //                    if (ws.Distan <= ws.WorkRadius)
            //                    {
            //                        int t = TimeHelper.IsInTime(ws.StartTime, ws.EndTime);
            //                        if (t == 1)
            //                        {
            //                            msg = "预计今日" + DateTime.Now.AddMinutes(ws.CostTime).AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                        }
            //                        else if (t == 2)
            //                        {
            //                            msg = "预计明日" + ws.StartTime.AddMinutes(ws.CostTime).AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                        }
            //                        else
            //                        {
            //                            msg = "预计今日" + ws.StartTime.AddMinutes(ws.CostTime).AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
            //                        }
            //                        workID = ws.ID;
            //                    }
            //                    else
            //                    {
            //                        msg = "超出服务范围";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        mm.msg = msg;
            //        return Content(JsonConvert.SerializeObject(mm));
            //    }
            //    Orders order = new Orders();

            //    order.BrandID = uc.BrandID;
            //    order.BrandShow = uc.BrandShow;
            //    order.CDate = DateTime.Now;
            //    order.StoreID = 0;
            //    order.Color = uc.Color;
            //    order.WorkID = workID;
            //    order.IP = Common.GetIP();
            //    order.Lat = lat;
            //    order.Lng = lng;
            //    order.Mobile = mobile;
            //    order.ModelID = uc.ModelID;
            //    order.OrderState = (int)UserOrderState.WaitPaying;
            //    order.Osn = "APP" + DAL.Orders.GenerateOSN(WorkContext.ID);
            //    order.Plate = uc.Plate;
            //    order.Uid = WorkContext.ID;
            //    order.UserName = name;
            //    order.Remark = bookdate;
            //    order.FinishDate = finishtime;
            //    order.Address = address;
            //    order.PaySn = "";
            //    order.PayTime = new DateTime(1900, 1, 1);
            //    order.PayName = "";

            //    List<Orders_Item> listitem = new List<Orders_Item>();
            //    foreach (var item in itemid)
            //    {
            //        Wash_Item wi = DAL.Wash_Item.GetModel(DataType.ConvertToInt(item));
            //        if (wi != null)
            //        {
            //            order.Amount += wi.Price;

            //            if (wi.SortID == 999)
            //            {
            //                Orders o = DAL.Orders.ExistOrder(WorkContext.ID);
            //                if (o != null)
            //                {
            //                    isFir = false;
            //                    msg = "对不起，此选项仅限首单用户";
            //                    mm.msg = msg;
            //                    return Content(JsonConvert.SerializeObject(mm));
            //                }
            //                else isFir = true;
            //            }

            //            Orders_Item oi = new Orders_Item()
            //            {
            //                CDate = DateTime.Now,
            //                ItemID = wi.ID,
            //                ItemName = wi.Title,
            //                Money = wi.Price,
            //                StoreID = 0,
            //                UserID = WorkContext.ID
            //            };
            //            listitem.Add(oi);
            //        }
            //    }

            //    int oid = DAL.Orders.CreateOrder(order, listitem);
            //    mm.msg = "success";
            //    mm.code = "1";
            //    mm.data = oid;
            //    return Content(JsonConvert.SerializeObject(mm));
            //}
        }
        public ActionResult PayOrder(int payType, string paySn)
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "支付失败";
            int oid = AliceDAL.Common.GetFormInt("aliceoid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            if (order == null)
            {
                mm.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (order.OrderState != (int)UserOrderState.WaitPaying)
            {
                mm.msg = "订单不能支付";
                return Content(JsonConvert.SerializeObject(mm));
            }
            switch (payType)
            {
                case 10://钱包
                    if (order.Amount > WorkContext.WalletMoney)
                    {
                        mm.msg = "钱包余额不足，请选择其他支付方式";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    if (WorkContext.Wallet.Money2 >= order.Amount)//优先使用赠额
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.Wallet.Money1, WorkContext.Wallet.Money2 - order.Amount);
                    }
                    else if (WorkContext.Wallet.Money1 >= order.Amount)
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.Wallet.Money1 - order.Amount, WorkContext.Wallet.Money2);
                    }
                    else
                    {
                        DAL.BD_Users.SetUserWalletByUserID(WorkContext.ID, WorkContext.Wallet.Money1 - (order.Amount - WorkContext.Wallet.Money2), 0);
                    }
                    DAL.Orders.PayOrder(oid, UserOrderState.Payed, "", DateTime.Now);
                    //MessageManger.OrderSuccess(order.Mobile, WorkContext.ID);
                    //MessageManger.FirstOrder(order.Mobile, WorkContext.ID);
                    DAL.BD_Consumer.Add(new BD_Consumer()
                    {
                        Money = -order.Amount,
                        OrderID = oid,//Orders ID
                        Remark = "钱包支付",
                        UserID = order.Uid
                    });
                    DAL.UserOrderActions.Add(new UserOrderActions()
                    {
                        Oid = oid,
                        Uid = order.Uid,
                        RealName = "本人",
                        ActionType = "支付",
                        ActionDes = "你使用钱包完成支付"
                    });
                    mm.code = "1";
                    mm.msg = "success";
                    return Content(JsonConvert.SerializeObject(mm));
                case 20://微信
                    DAL.Orders.PayOrder(order.ID, UserOrderState.Payed, paySn, DateTime.Now);
                    //MessageManger.OrderSuccess(order.Mobile, WorkContext.ID);
                    //MessageManger.FirstOrder(order.Mobile, order.Uid);
                    DAL.BD_Consumer.Add(new BD_Consumer()
                    {
                        Money = -order.Amount,
                        OrderID = order.ID,//Orders ID
                        Remark = "微信支付",
                        UserID = order.Uid
                    });
                    DAL.OrderActions.Add(new OrderActions()
                    {
                        Oid = order.ID,
                        Uid = order.Uid,
                        RealName = "本人",
                        ActionType = "支付",
                        ActionDes = "你使用微信支付订单成功，微信交易号为:" + paySn
                    });
                    mm.code = "1";
                    mm.msg = "success";
                    return Content(JsonConvert.SerializeObject(mm));
                case 30://支付宝
                    DAL.Orders.PayOrder(order.ID, UserOrderState.Payed, paySn, DateTime.Now);
                    DAL.BD_Consumer.Add(new BD_Consumer()
                    {
                        Money = -order.Amount,
                        OrderID = order.ID,
                        Remark = "支付宝支付",
                        UserID = order.Uid
                    });
                    DAL.OrderActions.Add(new OrderActions()
                    {
                        Oid = order.ID,
                        Uid = order.Uid,
                        RealName = "本人",
                        ActionType = "支付",
                        ActionDes = "你使用支付宝支付订单成功，支付宝交易号为:" + paySn
                    });
                    mm.code = "1";
                    mm.msg = "success";
                    return Content(JsonConvert.SerializeObject(mm));
                default:
                    mm.code = "0";
                    mm.msg = "未知错误";
                    return Content(JsonConvert.SerializeObject(mm));
            }
        }
        public ActionResult Reviews()
        {
            int oid = AliceDAL.Common.GetFormInt("aliceoid");
            string comment = AliceDAL.Common.GetFormString("alicecomment");
            int attitudestar = AliceDAL.Common.GetFormInt("attitudestar");
            int servicestar = AliceDAL.Common.GetFormInt("servicestar");
            int speedstar = AliceDAL.Common.GetFormInt("speedstar");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            MessageModel mm = new MessageModel();
            mm.code = "0";
            if (order == null)
            {
                mm.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            if (order.OrderState == (int)UserOrderState.Canceled || order.OrderState == (int)UserOrderState.WaitPaying)
            {
                mm.msg = "订单未支付，不允许评价";
                return Content(JsonConvert.SerializeObject(mm));
            }
            Reviews model = new Reviews();
            model.Body = AliceDAL.Common.cutBadStr(comment) ?? "";
            model.IP = AliceDAL.Common.GetIP();
            model.Oid = oid;
            model.StoreID = order.StoreID;
            model.Uid = WorkContext.ID;
            model.AttitudeStar = attitudestar == 0 ? 5 : attitudestar;
            model.ServiceStar = servicestar == 0 ? 5 : servicestar;
            model.SpeedStar = speedstar == 0 ? 5 : speedstar;
            string str = DAL.Reviews.Add(model);
            if (str == "exists")
            {
                mm.msg = "此订单已评价";
                return Content(JsonConvert.SerializeObject(mm));
            }
            else
            {
                mm.code = "1";
                mm.msg = "评价成功";
                return Content(JsonConvert.SerializeObject(mm));
            }
        }

        public ActionResult AddChargeOrder()
        {
            lock (_locker)
            {
                MessageModel mm = new MessageModel();
                mm.msg = "fail";
                mm.code = "0";
                AdminOrders order = new AdminOrders();
                int typeID = AliceDAL.Common.GetFormInt("alicetypeid");
                int p = AliceDAL.Common.GetFormInt("aliceprice");
                string payName = AliceDAL.Common.GetFormString("alicepayname");
                string mobile = AliceDAL.Common.GetFormString("alicemobile");

                if (payName != "微信" && payName != "支付宝")
                {
                    mm.msg = "请选择支付方式";
                }

                ChargeType ct = DAL.ChargeType.GetModel(typeID);
                if (typeID == 999)
                {
                    order.OrderAmount = p;
                }
                else
                {
                    if (ct == null) mm.msg = "套餐不存在";
                    order.OrderAmount = ct.Price;
                }
                order.OrderState = (int)OrderState.WaitPaying;
                order.Osn = "APP" + DAL.AdminOrders.GenerateOSN(WorkContext.ID);
                order.TypeID = typeID;
                order.Uid = WorkContext.ID;
                order.Count = 1;
                order.PayName = payName;
                int oid = DAL.AdminOrders.CreateOrder(order);
                if (oid > 0)
                {
                    mm.code = "1";
                    mm.msg = "success";
                    mm.data = oid;
                }
                return Content(JsonConvert.SerializeObject(mm));
            }
        }
    }
}
