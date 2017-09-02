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
using System.Data;

namespace Web.Areas.BusinessManger.Controllers
{
    public class OrdersController : BusinessBaseController
    {
        public ActionResult ProOrderList(string txtUserName, string txtMobile, string ddlState, string txtSDate, string btnSearch, string txtEDate, int page = 1)
        {
            StringBuilder strsql = new StringBuilder(String.Format("[BusinessID]={0}", WorkContext.ID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }
            if (!String.IsNullOrEmpty(txtUserName)) AliceDAL.Common.SqlString(strsql, String.Format("UserName like '%{0}%'", AliceDAL.Common.cutBadStr(txtUserName)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (!String.IsNullOrEmpty(ddlState)) AliceDAL.Common.SqlString(strsql, String.Format("OrderState={0}", AliceDAL.Common.cutBadStr(ddlState)));
            ListModel model = new ListModel();
            ListModel.CreatePage(model, "[Pro_Orders]", strsql, page, WorkContext.PageSize, "[PayTime] desc,[ID]");
            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "订单编号,用户姓名,联系电话,订单总额,支付方式,商家,下单时间,订单状态,订单明细";
                    string strCol = "Osn,UserName,Mobile,Amount,PayName,BusinessName,CDate,OrderState,OrderDetail";
                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[Pro_Orders]", strsql.ToString(), "ID asc,CDate", 0);
                    string[] HeaderArr = strHeader.Split(new char[] { ',' });
                    string[] ColArr = strCol.Split(new char[] { ',' });
                    foreach (string col in HeaderArr)
                    {
                        dtt.Columns.Add(col);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dgvr in dt.Rows)
                        {
                            DataRow dr = dtt.NewRow();
                            for (int i = 0; i < HeaderArr.Length; i++)
                            {
                                switch (i)
                                {
                                    case 5:
                                        string bn = Web.Units.Comm.GetBusinessName(dgvr["BusinessID"].ToString(), 1);
                                        dr[HeaderArr[i]] = bn;
                                        break;
                                    case 7:
                                        string s = "";
                                        UserOrderState uos = (UserOrderState)AliceDAL.DataType.ConvertToInt(dgvr["OrderState"].ToString());
                                        switch (uos)
                                        {
                                            case UserOrderState.WaitPaying:
                                                s = "等待支付";
                                                break;
                                            case UserOrderState.Payed:
                                                s = "支付成功";
                                                break;
                                            case UserOrderState.Assigned:
                                                s = "备货完成";
                                                break;
                                            case UserOrderState.Started:
                                                s = "开始配送";
                                                break;
                                            case UserOrderState.Finished:
                                                s = "配送完成";
                                                break;
                                            case UserOrderState.Canceled:
                                                s = "已取消";
                                                break;
                                            case UserOrderState.Void:
                                                s = "已作废";
                                                break;
                                            default:
                                                break;
                                        }
                                        dr[HeaderArr[i]] = s;
                                        break;
                                    case 8:
                                        string r = Web.Units.Comm.GetProItem(dgvr["ID"].ToString(), 1) + dgvr["Address"].ToString();
                                        dr[HeaderArr[i]] = r;
                                        break;
                                    default:
                                        dr[HeaderArr[i]] = dgvr[ColArr[i]];
                                        break;
                                }

                            }
                            dtt.Rows.Add(dr);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("商品订单" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(ExcelRender.RenderToExcel(dtt).GetBuffer());
                    }
                    break;
                default:
                    break;
            }
            return View(model);
        }
        public ActionResult AjaxList1(int oid = 0)
        {
            int maxid = DAL.Pro_Orders.GetMaxOid1(oid, WorkContext.ID);
            if (maxid > 0)
            {
                return Content(maxid.ToString());
            }
            else
            {
                return Content("0");
            }
        }
        public ActionResult ProOrderInfo()
        {
            int oid = UrlParam.GetIntValue("oid");
            Models.ProOrderInfoModel oim = new Models.ProOrderInfoModel();
            Pro_Orders orderInfo = DAL.Pro_Orders.GetOrderInfoByID(oid);
            oim.OrderInfo = orderInfo;
            List<UserOrderActions> list = DAL.UserOrderActions.GetListByOrderID(0,oid);
            oim.UserActionsList = list;
            oim.OrderItems = DAL.Pro_Orders.GetItemListByOrderID(oid);
            return View(oim);
        }
        public ActionResult CancelOrder1()
        {
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders order = DAL.Pro_Orders.GetOrderInfoByID(oid);
            if (order.OrderState != (int)UserProOrderState.Payed && order.OrderState != (int)UserProOrderState.Assigned && order.OrderState != (int)UserProOrderState.Started)
            {
                return PromptView("订单不可取消");
            }
            DAL.Pro_Orders.ChangeOrder(oid, UserProOrderState.Canceled);
            //订单动作
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = 0,
                POid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.BusinessName + ")",
                ActionType = "取消",
                ActionDes = "取消订单"
            });
            //消费记录
            DAL.BD_Consumer.Add(new BD_Consumer()
            {
                CDate = DateTime.Now,
                Money = order.Amount,
                OrderID = order.ID,
                Remark = "退款",
                UserID = order.Uid
            });
            //返钱
            DAL.BD_Users.EditUserWalletByUserID(order.Uid, 0, order.Amount);

            //AddAdminLog("订单取消", "订单取消,订单ID为:" + oid + ";订单号为:" + order.Osn);
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
        public ActionResult VoidOrder1()
        {
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders order = DAL.Pro_Orders.GetOrderInfoByID(oid);
            //if (order.OrderState != (int)UserOrderState.Payed && order.OrderState != (int)UserOrderState.Assigned && order.OrderState != (int)UserOrderState.Started)
            //{
            //    return PromptView("订单不可取消");
            //}
            DAL.Pro_Orders.ChangeOrder(oid, UserProOrderState.Void);
            //AddAdminLog("订单作废", "订单作废,订单ID为:" + oid + ";订单号为:" + order.Osn);
            return RedirectToAction("OrderInfo", new { oid = oid });
        }
        public ActionResult AssignOrder1()
        {
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders order = DAL.Pro_Orders.GetOrderInfoByID(oid);
            DAL.Pro_Orders.ChangeOrder(oid, UserProOrderState.Assigned);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = 0,
                POid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.BusinessName + ")",
                ActionType = "备货",
                ActionDes = "系统备货"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
        public ActionResult StartOrder1()
        {
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders order = DAL.Pro_Orders.GetOrderInfoByID(oid);
            DAL.Pro_Orders.ChangeOrder(oid, UserProOrderState.Started);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = 0,
                POid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.BusinessName + ")",
                ActionType = "发货中",
                ActionDes = "发货中"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
        public ActionResult FinishOrder1()
        {
            int oid = UrlParam.GetIntValue("oid");
            Pro_Orders order = DAL.Pro_Orders.GetOrderInfoByID(oid);
            DAL.Pro_Orders.ChangeOrder(oid, UserProOrderState.Finished);
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = 0,
                POid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.BusinessName + ")",
                ActionType = "已签收",
                ActionDes = "已签收"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
    }
}
