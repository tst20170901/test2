using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using Models;
using AliceDAL;

namespace Web.Areas.AliceChopper.Controllers
{
    public class AdminOrdersController : AdminBaseController
    {
        public ActionResult List(int os = 0, int tid = 0, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (os > 0) AliceDAL.Common.SqlString(strsql, String.Format("OrderState={0}", os.ToString()));
            if (tid > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", tid.ToString()));
            ListModel.CreatePage(model, "[uv_AdminOrders]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult OrderInfo()
        {
            int oid = UrlParam.GetIntValue("oid");
            Models.AdminOrderInfoModel oim = new Models.AdminOrderInfoModel();
            oim.OrderInfo = DAL.AdminOrders.GetOrderInfoByID(oid);
            List<OrderActions> list = DAL.OrderActions.GetListByOrderID(oid);
            oim.ActionsList = list;
            oim.UserInfo = DAL.BD_Users.GetUserInfoByID(oim.OrderInfo.Uid);
            return View(oim);
        }

        public ActionResult CancelOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            int type = UrlParam.GetIntValue("type");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            DAL.Orders.ChangeOrder(oid, UserOrderState.Canceled);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "取消",
                ActionDes = "取消订单"
            });
            if (type == 0)
            {
                //DAL.Coupons.ChangeCoupons(order.CouponID, CouponState.Submitted);
            }
            return RedirectToAction("OrderInfo", new { oid = oid });
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
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "派单",
                ActionDes = "系统派单"
            });
            return RedirectToAction("OrderInfo", new { oid = oid });
        }
        public ActionResult StartOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            DAL.Orders.ChangeOrder(oid, UserOrderState.Started);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "洗车开始",
                ActionDes = "开始洗车"
            });
            return RedirectToAction("OrderInfo", new { oid = oid });
        }
        public ActionResult FinishOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            DAL.Orders.ChangeOrder(oid, UserOrderState.Finished);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "洗车完成",
                ActionDes = "洗车完成"
            });
            return RedirectToAction("OrderInfo", new { oid = oid });
        }
    }
}
