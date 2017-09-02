using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Text;
using Web.Areas.Models;
using System.Data;
using Models;
using AliceDAL;

namespace Web.Areas.AliceChopper.Controllers
{
    public class StatisticsController : AdminBaseController
    {
        public ActionResult DailyOrders(string txtSDate, string ddlState, string btnSearch, int WorkerID = -1, int BranchID = -1, int page = 1)
        {
            ViewData["WorkerListchild"] = DAL.PageModel.DateList("[uv_WorkShop]", "[BranchID]=" + WorkContext.BranchID + " and (WorkState=10 or WorkState=30)", "Title asc,ID", 0);
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

            if (!String.IsNullOrEmpty(ddlState))
            {
                //150为全部订单
                if (ddlState == "150")
                {
                    AliceDAL.Common.SqlString(strsql, string.Format("([OrderState]={0} or [OrderState]={1} or [OrderState]={2} or [OrderState]={3} or [OrderState]={4})", 10, 30, 50, 70, 90));
                }
                else
                {
                    AliceDAL.Common.SqlString(strsql, string.Format("[OrderState]={0}", ddlState));
                }
            }
            else AliceDAL.Common.SqlString(strsql, "[OrderState]=70");

            if (WorkerID > 0) AliceDAL.Common.SqlString(strsql, String.Format("[WorkID]={0}", WorkerID));
            if (!String.IsNullOrWhiteSpace(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[FinishDate] between '{0}' and '{1}'", txtSDate, AliceDAL.DataType.ConvertToDateTime(txtSDate).AddDays(1).ToString("yyyy/MM/dd")));
            }
            else
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[FinishDate] between '{0}' and '{1}'", DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.AddDays(1).ToString("yyyy/MM/dd")));
            }
            ListModel model = new ListModel();
            ListModel.CreatePage(model, "[Orders]", strsql, page, WorkContext.PageSize, "[PayTime] desc,[ID]");
            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "订单号,车主姓名,电话,车牌号,车型,服务项目,所在区域,实收金额,下单方式,洗车工,完成情况,备注,开始时间,结束时间,客户反馈,回访时间";
                    string strCol = "Osn,UserName,Mobile,Plate,BrandShow,OrderItem,Address,Amount,OrderStyle,Worker,FinishState,Remark,StartTime,FinishTime,Reback,BackTime";
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
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dr = dtt.NewRow();
                            for (int i = 0; i < HeaderArr.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        dr[HeaderArr[i]] = (j + 1).ToString();
                                        break;
                                    case 5:
                                        string r = Web.Units.Comm.GetItemNoPrice(dt.Rows[j]["ID"].ToString(), 1);
                                        dr[HeaderArr[i]] = r;
                                        break;
                                    case 8:
                                        string r1 = dt.Rows[j]["Data3"].ToString() == "" ? "线上" : dt.Rows[j]["Data3"].ToString();
                                        dr[HeaderArr[i]] = r1;
                                        break;
                                    case 9:
                                        string w = Web.Units.Comm.GetWorkName(dt.Rows[j]["WorkID"].ToString(), 1);
                                        dr[HeaderArr[i]] = w;
                                        break;
                                    case 10:
                                        string s = "";
                                        UserOrderState uos = (UserOrderState)AliceDAL.DataType.ConvertToInt(dt.Rows[j]["OrderState"].ToString());
                                        switch (uos)
                                        {
                                            case UserOrderState.WaitPaying:
                                                s = "等待支付";
                                                break;
                                            case UserOrderState.Payed:
                                                s = "支付成功";
                                                break;
                                            case UserOrderState.Assigned:
                                                s = "派单";
                                                break;
                                            case UserOrderState.Started:
                                                s = "开始";
                                                break;
                                            case UserOrderState.Finished:
                                                s = "完成";
                                                break;
                                            case UserOrderState.Canceled:
                                                s = "取消";
                                                break;
                                            case UserOrderState.Void:
                                                s = "作废";
                                                break;
                                            default:
                                                break;
                                        }
                                        dr[HeaderArr[i]] = s;
                                        break;
                                    case 11:
                                        string b = dt.Rows[j]["DiscountDes"].ToString() != "" ? dt.Rows[j]["DiscountDes"].ToString() : "";
                                        dr[HeaderArr[i]] = b;
                                        break;
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                        dr[HeaderArr[i]] = "";
                                        break;
                                    default:
                                        dr[HeaderArr[i]] = dt.Rows[j][ColArr[i]];
                                        break;
                                }

                            }
                            dtt.Rows.Add(dr);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("小熊洗车日记录表" + DateTime.Now.ToString("yyyy.MM.dd")) + ".xls");
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
        public ActionResult AreaOrders(string ddlState, string txtSDate, string txtEDate, string ddlPayName, int BranchID = -1, int page = 1)
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
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }
            else
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
            }
            if (!String.IsNullOrEmpty(ddlState)) AliceDAL.Common.SqlString(strsql, String.Format("[OrderState]={0}", AliceDAL.Common.cutBadStr(ddlState)));
            if (!String.IsNullOrEmpty(ddlPayName)) AliceDAL.Common.SqlString(strsql, String.Format("[PayName]='{0}'", AliceDAL.Common.cutBadStr(ddlPayName)));
            DataTable dt = DAL.PageModel.DataKeysList("[Orders]", "[ID],[Lng],[Lat]", strsql.ToString(), "CDate asc,ID", 0);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    sb.Append("{\"ID\":" + item["ID"].ToString() + ",");
                    if (!String.IsNullOrEmpty(item["Lng"].ToString())) sb.Append("\"Pos\":[" + item["Lng"].ToString() + "," + item["Lat"].ToString() + "],");
                    else sb.Append("\"Pos\":[" + WorkContext.Branch.CenterLng + "," + WorkContext.Branch.CenterLat + "],");
                    sb.Append("\"Icon\":\"/Content/images/7.png\"},");
                }
            }
            if (sb.Length > 5) sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            ViewBag.OrderPoints = sb.ToString();
            return View();
        }
        public ActionResult NoticeLogsList(string txtSDate, string txtEDate, int WorkerID = -1, int BranchID = -1, int page = 1)
        {
            ViewData["WorkerListchild"] = DAL.PageModel.DateList("[uv_WorkShop]", "[BranchID]=" + WorkContext.BranchID + " and (WorkState=10 or WorkState=30)", "Title asc,ID", 0);
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));



            if (WorkerID > 0) AliceDAL.Common.SqlString(strsql, String.Format("[WorkID]={0}", WorkerID));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }
            else
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
            }
            ListModel model = new ListModel();
            ListModel.CreatePage(model, "[NoticeLogs]", strsql, page, WorkContext.PageSize, "[ID]");
            return View(model);
        }
        public ActionResult CheckLogsList(string txtSDate, string txtEDate, string plate, string mobile, string usermobile, int page = 1)
        {
            StringBuilder strsql = new StringBuilder();
            if (!String.IsNullOrEmpty(plate)) AliceDAL.Common.SqlString(strsql, String.Format("[Plate] like '%{0}%'", plate));
            if (!String.IsNullOrEmpty(mobile)) AliceDAL.Common.SqlString(strsql, String.Format("[Mobile] like '%{0}%'", mobile));
            if (!String.IsNullOrEmpty(usermobile)) AliceDAL.Common.SqlString(strsql, String.Format("[UserMobile] like '%{0}%'", usermobile));
            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", txtSDate, DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy-MM-dd")));
            }
            else
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[CDate] between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
            }
            ListModel model = new ListModel();
            ListModel.CreatePage(model, "[Edo_CheckLogs]", strsql, page, WorkContext.PageSize, "[ID]");
            return View(model);
        }
    }
}
