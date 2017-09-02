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
using AliceDAL;

namespace Web.Areas.UsersManger.Controllers
{
    public class OrdersController : BigUserBaseController
    {
        public ActionResult OrdersAdd()
        {
            if (AliceDAL.Common.IsGet())
            {
                List<Wash_Item> list = new List<Wash_Item>();
                list = DAL.Wash_Item.GetList("ItemState=10 and BranID=" + WorkContext.BranchID);
                Models.BigOrderModel bom = new Models.BigOrderModel();
                bom.Items = list;
                bom.Cars = DAL.UserCar.GetList(WorkContext.ID);
                bom.group = DAL.PageModel.DateList("[CarGroup]", "UserID=" + WorkContext.ID, "SortID", 0);
                bom.Money = WorkContext.wallet.Money1 + WorkContext.wallet.Money2;
                return View(bom);
            }
            string bp = AliceDAL.Common.GetFormString("bigplate");
            string remark = AliceDAL.Common.GetFormString("remark");
            int l = bp.Split('；').Length - 1 == 0 ? 1 : bp.Split('；').Length - 1;
            string address = AliceDAL.Common.GetFormString("address");
            string[] itemid = AliceDAL.Common.GetFormString("witems").Split(',');
            decimal money = AliceDAL.DataType.ConvertToDecimal(AliceDAL.Common.GetFormString("allmoney"));


            Orders order = new Orders();

            order.BrandID = -1;
            order.BrandShow = "";
            order.CDate = DateTime.Now;
            order.StoreID = 0;
            order.Color = "";
            order.WorkID = 0;
            order.IP = AliceDAL.Common.GetIP();
            order.Lat = "";
            order.Lng = "";
            order.Mobile = WorkContext.Mobile;
            order.ModelID = -1;
            order.OrderState = (int)UserOrderState.WaitPaying;
            order.Osn = DAL.Orders.GenerateOSN(WorkContext.ID);
            order.Plate = bp;
            order.Uid = WorkContext.ID;
            order.UserName = WorkContext.Data1;
            order.Remark = remark;
            order.FinishDate = DateTime.Now.AddDays(1);
            order.Address = address;

            order.PayName = "钱包";
            order.PaySn = "";
            order.PayTime = new DateTime(1900, 1, 1);

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
                        StoreID = 0,
                        UserID = WorkContext.ID,
                        WorkPrice = wi.WorkPrice
                    };
                    listitem.Add(oi);
                }
            }
            order.Amount = order.Amount * l == money ? money : order.Amount * l;

            int oid = DAL.Orders.CreateOrder(order, listitem);



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
            DAL.Orders.PayOrder(oid, UserOrderState.Payed, "", DateTime.Now);
            //MessageManger.OrderSuccess(WorkContext.Mobile, WorkContext.ID);
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
            return PromptView("下单成功");
        }
        public ActionResult List(string txtUserName, string txtMobile, string ddlState, string txtSDate, string txtEDate, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[Uid]=" + WorkContext.ID.ToString());
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) > DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, txtEDate));
            }

            if (!String.IsNullOrEmpty(txtUserName)) AliceDAL.Common.SqlString(strsql, String.Format("UserName like '%{0}%'", AliceDAL.Common.cutBadStr(txtUserName)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (!String.IsNullOrEmpty(ddlState)) AliceDAL.Common.SqlString(strsql, String.Format("OrderState={0}", AliceDAL.Common.cutBadStr(ddlState)));
            ListModel.CreatePage(model, "[Orders]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult CancelOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            if (order == null)
            {
                return PromptView(Url.Action("List"), "订单不存在");
            }
            if (order.OrderState == (int)UserOrderState.Canceled)
            {
                return RedirectToAction("OrderInfo", "Orders", new { oid = oid });
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
            return RedirectToAction("OrderInfo", "Orders", new { oid = oid });
        }
        public ActionResult OrderInfo()
        {
            int oid = UrlParam.GetIntValue("oid");
            Models.OrderInfoModel oim = new Models.OrderInfoModel();
            oim.OrderInfo = DAL.Orders.GetOrderInfoByID(oid);
            List<UserOrderActions> list = DAL.UserOrderActions.GetListByOrderID(oid,0);
            oim.UserActionsList = list;
            oim.OrderItems = DAL.Orders.GetItemListByOrderID(oid);
            return View(oim);
        }
    }
}
