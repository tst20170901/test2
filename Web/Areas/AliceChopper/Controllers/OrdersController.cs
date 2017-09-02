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
using System.Data;
using Newtonsoft.Json;

namespace Web.Areas.AliceChopper.Controllers
{
    public class OrdersController : AdminBaseController
    {
        public ActionResult List(string txtUserName, string txtMobile, string ddlState, string txtSDate, string btnSearch, string txtEDate, int BranchID = -1, int page = 1)
        {
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }

            if (!String.IsNullOrEmpty(txtUserName)) AliceDAL.Common.SqlString(strsql, String.Format("UserName like '%{0}%'", AliceDAL.Common.cutBadStr(txtUserName)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (!String.IsNullOrEmpty(ddlState)) AliceDAL.Common.SqlString(strsql, String.Format("OrderState={0}", AliceDAL.Common.cutBadStr(ddlState)));
            ListModel model = new ListModel();
            ListModel.CreatePage(model, "[Orders]", strsql, page, WorkContext.PageSize, "[PayTime] desc,[ID]");
            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "订单编号,用户姓名,联系电话,订单总额,优惠金额,优惠方式,支付方式,线上线下,洗车工,下单时间,订单状态,订单明细";
                    string strCol = "Osn,UserName,Mobile,Amount,DiscountAmount,DiscountDes,PayName,Online,WorkName,CDate,OrderState,OrderDetail";
                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[Orders]", strsql.ToString(), "ID asc,CDate", 0);
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
                                    case 7:
                                        string d = dgvr["Data3"].ToString() == "" ? "线上" : dgvr["Data3"].ToString();
                                        dr[HeaderArr[i]] = d;
                                        break;
                                    case 8:
                                        string w = Web.Units.Comm.GetWorkName(dgvr["WorkID"].ToString(), 1);
                                        dr[HeaderArr[i]] = w;
                                        break;
                                    case 10:
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
                                                s = "派单完成";
                                                break;
                                            case UserOrderState.Started:
                                                s = "开始洗车";
                                                break;
                                            case UserOrderState.Finished:
                                                s = "洗车完成";
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
                                    case 11:
                                        string r = Web.Units.Comm.GetItem(dgvr["ID"].ToString(), 1) + dgvr["Address"].ToString();
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
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("订单" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
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
        public ActionResult ProOrderList(string txtUserName, string txtMobile, string ddlState, string txtSDate, string btnSearch, string txtEDate, int BusinessID = -1, int page = 1)
        {
            ViewData["businessList"] = DAL.BD_Business.GetList(String.Format("[BranchID]={0} and [BusinessState]=10 and [TypeID]>0", WorkContext.BranchID));

            StringBuilder strsql = new StringBuilder();
            AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("CDate between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }

            if (!String.IsNullOrEmpty(txtUserName)) AliceDAL.Common.SqlString(strsql, String.Format("UserName like '%{0}%'", AliceDAL.Common.cutBadStr(txtUserName)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (!String.IsNullOrEmpty(ddlState)) AliceDAL.Common.SqlString(strsql, String.Format("OrderState={0}", AliceDAL.Common.cutBadStr(ddlState)));
            if (BusinessID > 0) AliceDAL.Common.SqlString(strsql, String.Format("[BusinessID]={0}", BusinessID.ToString()));
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
        public ActionResult PointList(string txtUserName, string txtMobile, string ddlState, string txtSDate, string btnSearch, string txtEDate, int BranchID = -1, int page = 1)
        {
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", WorkContext.BranchID));
            AliceDAL.Common.SqlString(strsql, "[OrderState]=10");
            DataTable dt = DAL.PageModel.DataKeysList("[Orders]", "[ID],[Osn],[UserName],[Mobile],[Plate],[Amount],[Lng],[Lat],[Address],[CDate]", strsql.ToString(), "CDate asc,ID", 100);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    sb.Append("{\"ID\":" + item["ID"].ToString() + ",\"Osn\":\"" + item["Osn"].ToString() + "\",\"UserName\":\"" + item["UserName"].ToString() + "\"," +
                               "\"Mobile\":\"" + item["Mobile"].ToString() + "\",\"Plate\":\"" + item["Plate"] + "\",\"Amount\":" + item["Amount"].ToString() + ",");
                    if (!String.IsNullOrEmpty(item["Lng"].ToString())) sb.Append("\"Pos\":[" + item["Lng"].ToString() + "," + item["Lat"].ToString() + "],");
                    else sb.Append("\"Pos\":[" + WorkContext.Branch.CenterLng + "," + WorkContext.Branch.CenterLat + "],");
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
                    sb.Append("\"Icon\":\"/Content/images/" + ico + "\",\"Address\":\"" + item["Address"].ToString() + "\",\"Details\":\"" + Web.Units.Comm.GetItem(item["ID"].ToString()) + "\"},");
                }
            }
            if (sb.Length > 5) sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            ViewBag.OrderPoints = sb.ToString();

            DataTable work = DAL.PageModel.DataKeysList("[uv_WorkShop]", "[ID],[Title],[Phone],[Lng1],[Lat1],[OrderCount]", "[BranchID]=" + WorkContext.BranchID + " and (WorkState=10 or WorkState=30)", "ID", 0);
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("[");
            if (work != null && work.Rows.Count > 0)
            {
                foreach (DataRow item in work.Rows)
                {
                    sb1.Append("{\"ID\":" + item["ID"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\"," +
                               "\"OrderCount\":\"" + item["OrderCount"].ToString() + "\",\"Phone\":\"" + item["Phone"].ToString() + "\",\"Pos\":[" + item["Lng1"].ToString() + "," + item["Lat1"].ToString() + "],},");
                }
            }
            if (sb1.Length > 5) sb1.Remove(sb1.Length - 1, 1);
            sb1.Append("]");
            ViewBag.WorkList = sb1.ToString();
            return View();
        }
        public ActionResult ReviewsList(string txtUserName, string txtMobile, int BranchID = -1, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[StoreID]={0}", WorkContext.BranchID));

            if (!String.IsNullOrEmpty(txtUserName)) AliceDAL.Common.SqlString(strsql, String.Format("UserName like '%{0}%'", AliceDAL.Common.cutBadStr(txtUserName)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("LoginID like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            ListModel.CreatePage(model, "[uv_Reviews]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        public ActionResult DeleteReview(int fid = -1)
        {
            DAL.PageModel.Delete("[Reviews]", String.Format("ID={0}", fid.ToString()));
            AddAdminLog("删除评价", "删除评价,评价ID为:" + fid);
            return PromptView("评价删除成功");
        }
        public ActionResult AjaxList(int oid = 0)
        {
            int maxid = DAL.Orders.GetMaxOid(oid, WorkContext.BranchID);
            if (maxid > 0)
            {
                return Content(maxid.ToString());
            }
            else
            {
                return Content("0");
            }
        }
        public ActionResult AjaxList1(int oid = 0)
        {
            int maxid = DAL.Pro_Orders.GetMaxOid(oid, WorkContext.BranchID);
            if (maxid > 0)
            {
                return Content(maxid.ToString());
            }
            else
            {
                return Content("0");
            }
        }
        public ActionResult OrderInfo()
        {
            Load();
            int oid = UrlParam.GetIntValue("oid");
            Models.OrderInfoModel oim = new Models.OrderInfoModel();
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            oim.OrderInfo = orderInfo;
            List<UserOrderActions> list = DAL.UserOrderActions.GetListByOrderID(oid, 0);
            oim.UserActionsList = list;
            oim.OrderItems = DAL.Orders.GetItemListByOrderID(oid);
            UserCar uu = DAL.UserCar.UserExist(oim.OrderInfo.Uid, oim.OrderInfo.Plate);
            oim.UserCar = uu;
            if (AliceDAL.Common.IsPost())
            {
                string btn = AliceDAL.Common.GetFormString("btnSure");
                switch (btn)
                {
                    case "修改":
                        string brandshow = AliceDAL.Common.GetFormString("oBrandShow");
                        string color = AliceDAL.Common.GetFormString("oColor");
                        string plate = AliceDAL.Common.GetFormString("oPlate");
                        string company = AliceDAL.Common.GetFormString("oCompany");
                        string duetime = AliceDAL.Common.GetFormString("oDueTime");
                        string oRemark = AliceDAL.Common.GetFormString("oRemark");
                        string oYY = AliceDAL.Common.GetFormString("oYY");

                        DAL.Orders.EditOrderCar(oid, brandshow, color, plate, oRemark, oYY);


                        UserCar uc = new UserCar();
                        uc.BrandID = uu == null ? -1 : uu.BrandID;
                        uc.BrandShow = brandshow;
                        uc.Color = color;
                        uc.Data1 = uu == null ? "" : uu.Data1;
                        uc.Data2 = company;//保险公司
                        uc.Data3 = duetime;//到期时间
                        uc.Data4 = uu == null ? "" : uu.Data4;
                        uc.Data5 = "";
                        uc.Plate = plate;
                        uc.ModelID = uu == null ? -1 : uu.ModelID;
                        uc.UserID = orderInfo.Uid;
                        uc.UserName = orderInfo.UserName;
                        int i = DAL.UserCar.EditSafeCompany(uc);
                        AddAdminLog("修改订单车辆信息", "修改订单车辆信息,订单编号为:" + oim.OrderInfo.Osn + ";原车型为:" + oim.OrderInfo.BrandShow + ";原颜色为:" + oim.OrderInfo.Color + ";原车牌号为:" + oim.OrderInfo.Plate + ";原车保险公司为:" + (uu == null ? "" : uu.Data2) + ";原车保险到期为:" + (uu == null ? "" : uu.Data3) + ";现车型为:" + brandshow + ";现颜色为:" + color + ";现车牌号为:" + plate + ";现车保险公司为:" + company + ";现车保险到期为:" + duetime);
                        oim.OrderInfo.BrandShow = brandshow;
                        oim.OrderInfo.Color = color;
                        oim.OrderInfo.Plate = plate;
                        oim.OrderInfo.Data4 = oRemark;
                        oim.OrderInfo.Remark = oYY;
                        if (oim.UserCar != null)
                        {
                            oim.UserCar.Data2 = company;
                            oim.UserCar.Data3 = duetime;
                        }
                        else
                        {
                            oim.UserCar = uc;
                        }

                        break;
                    case "确定":
                        if (oim.OrderInfo == null)
                        {
                            return PromptView(Url.Action("List"), "订单不存在");
                        }
                        else if (oim.OrderInfo.OrderState != (int)UserOrderState.Assigned && oim.OrderInfo.OrderState != (int)UserOrderState.Payed)
                        {
                            return PromptView(Url.Action("OrderInfo", new { oid = oid }), "订单已开始无法派送");
                        }
                        int workerID = AliceDAL.Common.GetFormInt("WorkerID");
                        if (workerID > 0)
                        {
                            WorkShop ws = DAL.WorkShop.WorkShopByID(workerID);
                            MessageManger.OrderAssign(oim.OrderInfo.Mobile, ws.Title, ws.Phone);
                        }
                        DAL.Orders.GetWorker(oid, workerID);
                        AddAdminLog("修改洗车工", "修改洗车工,订单编号为:" + oim.OrderInfo.Osn + ";原洗车工ID为:" + oim.OrderInfo.WorkID + ";现洗车工ID为:" + workerID);
                        oim.OrderInfo.WorkID = workerID;
                        break;
                    default:
                        break;
                }

            }
            return View(oim);
        }
        public ActionResult ProOrderInfo()
        {
            int oid = UrlParam.GetIntValue("oid");
            Models.ProOrderInfoModel oim = new Models.ProOrderInfoModel();
            Pro_Orders orderInfo = DAL.Pro_Orders.GetOrderInfoByID(oid);
            oim.OrderInfo = orderInfo;
            List<UserOrderActions> list = DAL.UserOrderActions.GetListByOrderID(0, oid);
            oim.UserActionsList = list;
            oim.OrderItems = DAL.Pro_Orders.GetItemListByOrderID(oid);
            return View(oim);
        }
        private void Load()
        {
            ViewData["WorkerListchild"] = DAL.PageModel.DateList("[uv_WorkShop]", "[BranchID]=" + WorkContext.BranchID + " and (WorkState=10 or WorkState=30)", "Title asc,ID", 0);
        }
        public ActionResult CancelOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            if (order.OrderState != (int)UserOrderState.Payed && order.OrderState != (int)UserOrderState.Assigned && order.OrderState != (int)UserOrderState.Started)
            {
                return PromptView("订单不可取消");
            }
            DAL.Orders.ChangeOrder(oid, UserOrderState.Canceled);
            //订单动作
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
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
            DAL.Orders.CancelUseVip(order.VipCardID, order.CouponsID, order.ID);
            DAL.BD_Users.EditUserWalletByUserID(order.Uid, 0, order.Amount);

            AddAdminLog("订单取消", "订单取消,订单ID为:" + oid + ";订单号为:" + order.Osn);
            return RedirectToAction("OrderInfo", new { oid = oid });
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
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
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

            AddAdminLog("订单取消", "订单取消,订单ID为:" + oid + ";订单号为:" + order.Osn);
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
        public ActionResult VoidOrder()
        {
            int oid = UrlParam.GetIntValue("oid");
            Orders order = DAL.Orders.GetOrderInfoByID(oid);
            //if (order.OrderState != (int)UserOrderState.Payed && order.OrderState != (int)UserOrderState.Assigned && order.OrderState != (int)UserOrderState.Started)
            //{
            //    return PromptView("订单不可取消");
            //}
            DAL.Orders.ChangeOrder(oid, UserOrderState.Void);
            AddAdminLog("订单作废", "订单作废,订单ID为:" + oid + ";订单号为:" + order.Osn);
            return RedirectToAction("OrderInfo", new { oid = oid });
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
            AddAdminLog("订单作废", "订单作废,订单ID为:" + oid + ";订单号为:" + order.Osn);
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
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "备货",
                ActionDes = "系统备货"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
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
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "发货中",
                ActionDes = "发货中"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
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
            Web.Units.MessageManger.OrderFinish(order.Mobile);
            return RedirectToAction("OrderInfo", new { oid = oid });
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
                RealName = WorkContext.LoginID + "(" + WorkContext.NickName + ")",
                ActionType = "已签收",
                ActionDes = "已签收"
            });
            return RedirectToAction("ProOrderInfo", new { oid = oid });
        }
        public ActionResult OrdersAdd()
        {
            if (AliceDAL.Common.IsGet())
            {
                List<Wash_Item> list = new List<Wash_Item>();
                list = DAL.Wash_Item.GetList("[BranID]=" + WorkContext.BranchID + " and [ItemState]=10");
                Models.OrderAddModel oam = new Models.OrderAddModel();
                oam.Items = list;
                //oam.Cars = DAL.UserCar.GetList(WorkContext.ID);
                //oam.Money = DAL.BD_Users.GetUserWalletByUserID();
                return View(oam);
            }
            bool isFir = false;
            Models.MessageModel mm = new Models.MessageModel();
            string mobile = Common.GetFormString("mobile");
            string username = Common.GetFormString("username");
            string plate = AliceDAL.Common.GetFormString("plate");
            string remark = AliceDAL.Common.GetFormString("remark");
            string address = AliceDAL.Common.GetFormString("address");
            string[] itemid = AliceDAL.Common.GetFormString("itemIds").Split(',');
            decimal money = AliceDAL.DataType.ConvertToDecimal(AliceDAL.Common.GetFormString("allmoney"));
            string pay = Common.GetFormString("paymode");
            string lat = Common.GetFormString("lat");
            string lng = Common.GetFormString("lng");
            AddressModel am = AMap.Common.GetAddress(lng, lat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                mm.code = "0";
                mm.msg = "选取坐标有误";
                return Content(JsonConvert.SerializeObject(mm));
            }
            bool b = Web.AMap.Common.IsPtInPoly(AliceDAL.DataType.ConvertToDoubile(lng), AliceDAL.DataType.ConvertToDoubile(lat), WorkContext.Branch.Points);
            if (!b)
            {
                mm.code = "0";
                mm.msg = "超出服务范围";
                return Content(JsonConvert.SerializeObject(mm));
            }

            if (!ValidateHelper.IsCarNo(plate))
            {
                mm.code = "0";
                mm.msg = "车牌号不正确";
                return Content(JsonConvert.SerializeObject(mm));
            }
            Orders order = new Orders();

            int uid = 0;
            BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
            BD_Wallet bw = null;
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
                order.Osn = DAL.Orders.GenerateOSN(AliceDAL.DataType.ConvertToInt(result));
                bw = DAL.BD_Users.GetUserWalletByUserID(AliceDAL.DataType.ConvertToInt(result));
                order.Uid = AliceDAL.DataType.ConvertToInt(result);
                uid = order.Uid;
            }
            else
            {
                order.Data3 = "手动线上";
                order.Osn = DAL.Orders.GenerateOSN(bu.ID);
                bw = DAL.BD_Users.GetUserWalletByUserID(bu.ID);
                order.Uid = bu.ID;
                uid = bu.ID;
            }


            order.BrandID = -1;
            order.BrandShow = "";
            order.CDate = DateTime.Now;
            order.StoreID = WorkContext.BranchID;
            order.Color = "";
            order.WorkID = 0;
            order.IP = AliceDAL.Common.GetIP();
            order.Lat = lat;
            order.Lng = lng;
            order.Mobile = mobile;//WorkContext.Mobile;
            order.ModelID = -1;
            order.OrderState = (int)UserOrderState.WaitPaying;
            order.Plate = plate;

            order.UserName = username;
            order.Remark = remark;
            order.FinishDate = DateTime.Now.AddHours(1);
            order.Address = address;

            order.PayName = "钱包";
            order.PaySn = "";
            order.PayTime = new DateTime(1900, 1, 1);

            int youhuitype = 0;
            List<Orders_Item> listitem = new List<Orders_Item>();
            foreach (var item in itemid)
            {
                Wash_Item wi = DAL.Wash_Item.GetModel(AliceDAL.DataType.ConvertToInt(item));
                if (wi != null)
                {
                    order.Amount += wi.Price;
                    if (wi.SortID == 999)
                    {
                        Orders o = DAL.Orders.ExistOrder(uid);
                        if (o != null)
                        {
                            isFir = false;
                            mm.code = "0";
                            mm.msg = "对不起，此选项仅限首单用户";
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                        else isFir = true;
                    }

                    if (wi.SortID == 888)//代表首单外观清洗
                    {
                        youhuitype += 111;
                        Orders o = DAL.Orders.ExistOrder(WorkContext.ID);
                        if (o != null)
                        {
                            isFir = false;
                            mm.code = "0";
                            mm.msg = "对不起，此选项仅限首单用户";
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                        else isFir = true;
                    }
                    else if (wi.SortID == 866)//代表首单整车清洗
                    {
                        youhuitype += 222;
                        Orders o = DAL.Orders.ExistOrder(WorkContext.ID);
                        if (o != null)
                        {
                            isFir = false;
                            mm.code = "0";
                            mm.msg = "对不起，此选项仅限首单用户";
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                        else isFir = true;
                    }
                    Orders_Item oi = new Orders_Item()
                    {
                        CDate = DateTime.Now,
                        ItemID = wi.ID,
                        ItemName = wi.Title,
                        Money = wi.Price,
                        StoreID = WorkContext.BranchID,
                        UserID = order.Uid,
                        WorkPrice = wi.WorkPrice
                    };
                    listitem.Add(oi);
                }
            }
            order.Amount = order.Amount == money ? money : order.Amount;
            if (pay == "10")
            {
                if (bw != null && (bw.Money1 + bw.Money2 >= order.Amount))
                {
                    if (bw.Money1 >= order.Amount)//优先使用余额
                    {
                        DAL.BD_Users.SetUserWalletByUserID(bu.ID, bw.Money1 - order.Amount, bw.Money2);
                    }
                    else if (bw.Money2 >= order.Amount)
                    {
                        DAL.BD_Users.SetUserWalletByUserID(bu.ID, bw.Money1, bw.Money2 - order.Amount);
                    }
                    else
                    {
                        DAL.BD_Users.SetUserWalletByUserID(bu.ID, 0, bw.Money2 - (order.Amount - bw.Money1));
                    }
                    order.Data3 = "手动线上";
                }
                else
                {
                    order.Data3 = "手动线下";
                }
            }
            else
            {
                order.Data3 = "手动线下";
            }


            int oid = DAL.Orders.CreateNewOrder(order, listitem);

            DAL.Orders.PayOrder(oid, UserOrderState.Payed, "", DateTime.Now);
            //MessageManger.OrderSuccess(mobile, uid);
            //if (isFir)
            //{
            //    MessageManger.FirstOrder(mobile, uid);
            //}
            DAL.BD_Consumer.Add(new BD_Consumer()
            {
                Money = -order.Amount,
                OrderID = oid,//Orders ID
                Remark = order.Data3,
                UserID = order.Uid
            });
            DAL.UserOrderActions.Add(new UserOrderActions()
            {
                Oid = oid,
                Uid = order.Uid,
                RealName = "网站客服",
                ActionType = "支付",
                ActionDes = order.Data3
            });
            mm.code = "1";
            mm.msg = "下单成功";
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult AjaxUserInfo()
        {
            Models.MessageModel mm = new Models.MessageModel();
            string mobile = AliceDAL.Common.GetFormString("mobile");
            if (!AliceDAL.ValidateHelper.IsMobile(mobile))
            {
                mm.code = "0";
                mm.msg = "手机号码格式不正确！";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
            BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
            if (bu == null)
            {
                mm.code = "0";
                mm.msg = "新用户";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
            BD_Wallet bw = DAL.BD_Users.GetUserWalletByUserID(bu.ID);

            Models.BD_OrderAdd boa = new Models.BD_OrderAdd();
            Orders order = DAL.Orders.GetLastestOrder(bu.ID);
            if (order != null)
            {
                if (order.Plate.Length > 7) boa.Plate = order.Plate.Substring(0, 7);
                else boa.Plate = order.Plate;
                boa.Address = order.Address;
                boa.UserName = order.UserName;
                boa.Remark = order.Remark;
            }
            boa.Money = bw.Money1 + bw.Money2;
            mm.code = "1";
            mm.msg = "老用户";
            mm.data = boa;
            string res1 = JsonConvert.SerializeObject(mm);
            return Content(res1);
        }
    }
}
