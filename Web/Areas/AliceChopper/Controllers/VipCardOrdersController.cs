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

namespace Web.Areas.AliceChopper.Controllers
{
    public class VipCardOrdersController : AdminBaseController
    {
        public ActionResult OrdersList(string txtTitle, string txtMobile, string btnSearch, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
                if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
                ListModel.CreatePage(model, "[uv_VipCardOrders]", strsql, page, WorkContext.PageSize, "ID");
            }
            else
            {
                if (!String.IsNullOrEmpty(txtMobile))
                {
                    if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn ='{0}'", AliceDAL.Common.cutBadStr(txtTitle)));
                    AliceDAL.Common.SqlString(strsql, String.Format("Mobile='{0}'", AliceDAL.Common.cutBadStr(txtMobile)));
                    ListModel.CreatePage(model, "[uv_VipCardOrders]", strsql, page, WorkContext.PageSize, "ID");
                }
            }
            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "订单编号,手机号,订单金额,状态,推荐人,交易方式,交易码,时间";
                    string strCol = "Osn,Mobile,OrderAmount,OrderState,WorkNo,PayName,PaySn,CDate";
                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[uv_VipCardOrders]", strsql.ToString(), "ID", 0);
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
                                    case 3:
                                        string s = "";
                                        OrderState os = (OrderState)AliceDAL.DataType.ConvertToInt(dgvr["OrderState"].ToString());
                                        switch (os)
                                        {
                                            case OrderState.Submitted:
                                                s = "已提交";
                                                break;
                                            case OrderState.WaitPaying:
                                                s = "等待付款";
                                                break;
                                            case OrderState.Confirming:
                                                s = "支付完成";
                                                break;
                                            default:
                                                break;
                                        }
                                        dr[HeaderArr[i]] = s;
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
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("充卡流水" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
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
    }
}
