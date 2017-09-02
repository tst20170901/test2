using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using Models;
using Web.Areas.Worker.Models;
using AliceDAL;
using Newtonsoft.Json;
using System.Data;

namespace Web.Areas.Worker.Controllers
{
    public class OrdersController : WorkerBaseController
    {
        [ValidateInput(false)]
        public ActionResult OrdersList(int page = 1)
        {
            MessageModel m = new Models.MessageModel();
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[WorkID]={0}", WorkContext.ID));
            ListModel.CreatePage(listmodel, "[Orders]", strsql, page, 6);

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
                    list.Add(om);
                }
                m.data = list;
            }
            return Content(JsonConvert.SerializeObject(m));
        }
        [ValidateInput(false)]
        public ActionResult PostLocation()
        {
            string Lat = AliceDAL.Common.GetFormString("Lat");
            string Lng = AliceDAL.Common.GetFormString("Lng");

            MessageModel m = new Models.MessageModel();

            if (String.IsNullOrWhiteSpace(Lat) || String.IsNullOrWhiteSpace(Lng))
            {
                m.code = "0";
                m.msg = "经纬度不能为空";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (DAL.WorkShop.PostLocation(WorkContext.ID, Lat, Lng) <= 0)
            {
                m.code = "0";
                m.msg = "fail";
            }
            m.data = DAL.Orders.GetLastestOid(WorkContext.ID);
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult OrdersShow(int oid)
        {
            MessageModel m = new MessageModel();
            OrderInfoModel om = new OrderInfoModel();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WorkContext.ID)
            {
                m.code = "0";
                m.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(m));
            }
            OrderInfoModel model = new OrderInfoModel();
            model.OrderInfo = orderInfo;
            model.OrderItems = DAL.Orders.GetItemListByOrderID(oid);
            model.UserActionsList = DAL.UserOrderActions.GetListByOrderID(oid,0);
            m.data = model;
            return Content(JsonConvert.SerializeObject(m));
        }
        /// <summary>
        /// 确认订单
        /// </summary>
        public ActionResult ConfirmOrder(int oid = -1)
        {
            MessageModel m = new Models.MessageModel();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WorkContext.ID)
            {
                m.code = "0";
                m.msg = "订单不存在";
                return Content(JsonConvert.SerializeObject(m));
            }
            if (orderInfo.OrderState == (int)UserOrderState.WaitPaying)
            {
                m.code = "0";
                m.msg = "买家还未付款，不能确认订单";
                return Content(JsonConvert.SerializeObject(m));
            }
            else if (orderInfo.OrderState != (int)UserOrderState.Payed)
            {
                m.code = "0";
                m.msg = "订单无需重复确认";
                return Content(JsonConvert.SerializeObject(m));
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Assigned);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = orderInfo.Uid,
                RealName = "洗车技师(" + WorkContext.Title + ")",
                ActionType = "派单",
                ActionDes = "系统派单"
            });
            //接单后，状态变忙碌
            DAL.WorkShop.ChangeWorkShop(WorkContext.ID, WorkShopState.Busy);
            m.msg = "接单完成";
            return Content(JsonConvert.SerializeObject(m));
        }
        /// <summary>
        /// 开始洗车
        /// </summary>
        public ActionResult StartWashCart(int oid = -1)
        {
            MessageModel m = new Models.MessageModel();

            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WorkContext.ID)
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
            string result = AliceDAL.Common.GetFormString("images");
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
                RealName = "洗车技师(" + WorkContext.Title + ")",
                ActionType = "洗车开始",
                ActionDes = "开始洗车"
            });
            DAL.Orders.UpdateStartImg(oid, sb.ToString().TrimEnd(','));
            m.msg = "开始洗车";
            return Content(JsonConvert.SerializeObject(m));
        }
        /// <summary>
        /// 洗车完成
        /// </summary>
        public ActionResult Finished(int oid = -1)
        {
            MessageModel m = new Models.MessageModel();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null || orderInfo.WorkID != WorkContext.ID)
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
            string result = AliceDAL.Common.GetFormString("images");
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
                RealName = "洗车技师(" + WorkContext.Title + ")",
                ActionType = "洗车完成",
                ActionDes = "洗车完成"
            });
            DAL.Orders.UpdateEndImg(oid, sb.ToString().TrimEnd(','));
            //洗车完成，变为空闲
            DAL.WorkShop.ChangeWorkShop(WorkContext.ID, WorkShopState.Open);
            m.msg = "洗车完成";
            return Content(JsonConvert.SerializeObject(m));
        }
    }
}
