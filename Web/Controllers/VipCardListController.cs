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
using AliceDAL;
namespace Web.Controllers
{
    public class VipCardListController : UserBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChongZhi()
        {
            lock (_locker)
            {
                MessageModel mm = new MessageModel();
                mm.code = "0";
                mm.msg = "会员卡不存在";
                string cardno = AliceDAL.Common.GetFormString("cardno");
                string cardpwd = AliceDAL.Common.GetFormString("cardpwd");
                string plate = AliceDAL.Common.GetFormString("plate").Replace(" ", "");
                if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(cardno)))
                {
                    mm.msg = "请输入卡号";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                else if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(cardpwd)))
                {
                    mm.msg = "请输入密码";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                else if (!String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(plate)))
                {
                    if (!AliceDAL.ValidateHelper.IsCarNo(plate))
                    {
                        mm.msg = "车牌号不正确";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                }
                string result = "";
                VipCard vc = DAL.VipCard.Exchange(cardno, cardpwd, out result);

                if (vc != null && vc.CardNo == cardno)
                {
                    if (vc.Uid > 0 || vc.CardState == 30)
                    {
                        mm.msg = "会员卡已被使用";
                        return Content(JsonConvert.SerializeObject(mm));
                    }

                    VipType viptype = DAL.VipType.GetModel(vc.TypeID);
                    if (viptype != null && viptype.LockPlate == 2)
                    {
                        if (String.IsNullOrEmpty(AliceDAL.Common.cutBadStr(plate)))
                        {
                            mm.msg = "请输入车牌号";
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                        else if (!AliceDAL.ValidateHelper.IsCarNo(plate))
                        {
                            mm.msg = "车牌号不正确";
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                    }
                    DateTime NewTDate = DateTime.Now.AddDays((vc.TDate - vc.CDate).Days);//根据有效期获取天数，修改有效期，从开卡日算起
                    if (DAL.VipCard.ChargeToUser(vc.ID, WorkContext.ID, WorkContext.Mobile, plate.ToUpper(), NewTDate) > 0)
                    {
                        mm.code = "1";
                        mm.msg = "充值成功";
                        MessageManger.SendMsg(WorkContext.Mobile, vc.SMSContent);
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    else
                    {
                        mm.msg = "充值失败，请稍候再试";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(mm));
                }
            }
        }
        public ActionResult OrderCardSubmit()
        {
            lock (_locker)
            {
                MessageModel mm = new MessageModel();
                mm.code = "0";
                mm.msg = "操作失败";
                VipCardOrders order = new VipCardOrders();

                int typeID = AliceDAL.Common.GetFormInt("typeID");
                int p = AliceDAL.Common.GetFormInt("price");
                int bid = AliceDAL.Common.GetFormInt("bid");
                string workno = AliceDAL.Common.GetFormString("workno");
                BD_Branch bb = DAL.BD_Branch.GetModel(bid);
                if (bid <= 0 || bb == null)
                {
                    mm.msg = "请重新拖动地图定位";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                if (!String.IsNullOrWhiteSpace(workno))
                {
                    int workcount = DAL.PageModel.DataCount("[WorkShop]", String.Format("[ID]={0} and [BranchID]={1}", AliceDAL.DataType.ConvertToInt(workno), bid));
                    if (workcount <= 0)
                    {
                        mm.msg = "此员工编号不存在";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                }
                VipType vt = DAL.VipType.GetModel(typeID);
                if (typeID == 999)
                {
                    if (p <= 0)
                    {
                        mm.msg = "充值金额不正确";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    AdminOrders ao = new AdminOrders();
                    ao.OrderAmount = p;
                    ao.OrderState = (int)OrderState.WaitPaying;
                    ao.Osn = DAL.AdminOrders.GenerateOSN(WorkContext.ID);
                    ao.TypeID = typeID;
                    ao.Uid = WorkContext.ID;
                    ao.Count = 1;
                    ao.PayName = "微信";
                    int oid = DAL.AdminOrders.CreateOrder(ao);
                    if (oid > 0)
                    {
                        mm.code = "1";
                        mm.msg = Url.Action("Pay", "WeiXinPay", new { oid = oid });
                    }
                    else
                    {
                        mm.msg = "购买失败";
                    }
                    return Content(JsonConvert.SerializeObject(mm));
                }
                else
                {
                    if (vt == null)
                    {
                        mm.msg = "套餐不存在";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    else if (vt.BranchID != bid)
                    {
                        mm.msg = "参数不正确，请手动定位！";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    else if (vt.Online != 2 && vt.Online != 3)
                    {
                        mm.msg = "参数不正确，请手动定位！";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    else if (vt.VipTypeState == 30 || vt.CardCount == 0)
                    {
                        mm.msg = "套餐已抢完！";
                        return Content(JsonConvert.SerializeObject(mm));
                    }
                    order.OrderState = (int)VipCardOrderState.WaitPaying;
                    order.Osn = DAL.VipCardOrders.GenerateOSN(WorkContext.ID);
                    order.VipTypeID = typeID;
                    order.Uid = WorkContext.ID;
                    order.Count = 1;
                    order.PayName = "微信";
                    order.BranchID = bid;
                    order.OrderAmount = vt.Price;
                    order.WorkNo = workno;
                    int oid = DAL.VipCardOrders.CreateOrder(order);
                    if (oid > 0)
                    {
                        DAL.VipType.ChangeCount(typeID);
                        mm.code = "1";
                        mm.msg = Url.Action("VipPay", "WeiXinPay", new { oid = oid });
                    }
                    else
                    {
                        mm.msg = "购买失败";
                    }
                    return Content(JsonConvert.SerializeObject(mm));
                }
            }
        }
        public ActionResult GetBranch()
        {
            MessageModel mm = new MessageModel();
            string lng = UrlParam.GetStringValue("a");
            string lat = UrlParam.GetStringValue("b");
            mm.msg = "不在服务范围";
            mm.code = "0";
            AddressModel m = AMap.Common.GetAddress(lng, lat);
            if (m != null && m.CityCode != "" && m.AdCode != "")
            {
                DataTable dt = new DataTable();
                //城市为邯郸不考虑区
                if (m.CityCode == "0310")
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode);//先不考虑缓存
                }
                else
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode, m.AdCode);//先不考虑缓存
                }
                bool result = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result = Web.AMap.Common.IsPtInPoly(AliceDAL.DataType.ConvertToDoubile(lng), AliceDAL.DataType.ConvertToDoubile(lat), row["Points"].ToString());
                        if (result)
                        {
                            mm.code = "1";
                            mm.data = row["ID"].ToString();
                            mm.msg = row["Title"].ToString();
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                    }
                    if (!result)
                    {
                        mm.msg = "不在服务范围";
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult ItemList()
        {
            int bid = Common.GetFormInt("bid");
            MessageModel mm = new MessageModel();
            mm.msg = "附近没有会员卡套餐";
            mm.code = "0";

            BD_Branch bb = DAL.BD_Branch.GetModel(bid);
            if (bb != null && bb.BranchState == 10)
            {
                BranchVipType bvt = new BranchVipType();
                DataTable dt = DAL.PageModel.DataKeysList("[uv_VipType]", "[ID],[Title],[Price],[BranchID],[CardCount]", "[BranchID]=" + bid.ToString() + " and [VipTypeState]=10 and [Online]=2", "Period desc,ID", 0);

                DataTable dt1 = DAL.PageModel.DataKeysList("[uv_VipType]", "[ID],[Title],[Price],[BranchID],[CardCount]", "[BranchID]=" + bid.ToString() + " and [VipTypeState]=10 and [Online]=3", "Period desc,ID", 0);

                if ((dt == null && dt1 == null) || (dt.Rows.Count == 0 && dt1.Rows.Count == 0))
                {
                    string rs1 = JsonConvert.SerializeObject(mm);
                    return Content(rs1);
                }

                bvt.VipTypeItemOnline = dt;
                bvt.VipTypeItemHot = dt1;

                mm.code = "1";
                mm.msg = "success";
                mm.data = bvt;
            }
            else
            {
                mm.msg = "请手动定位";
            }
            string rs = JsonConvert.SerializeObject(mm);
            return Content(rs);
        }
    }
}