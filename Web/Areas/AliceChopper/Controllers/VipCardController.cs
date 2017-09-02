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
    public class VipCardController : AdminBaseController
    {
        public ActionResult VipCardList(string txtCardNo, string txtTitle, string txtPrefix, string btnSearch, int BranchID = -1, int BusID = 0, int page = 1)
        {
            Load();
            ListModel model = new ListModel();
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
            if (BusID > 0) AliceDAL.Common.SqlString(strsql, String.Format("BusinessID={0}", BusID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (!String.IsNullOrEmpty(txtCardNo)) AliceDAL.Common.SqlString(strsql, String.Format("CardNo like '%{0}%'", AliceDAL.Common.cutBadStr(txtCardNo)));
            if (!String.IsNullOrEmpty(txtPrefix)) AliceDAL.Common.SqlString(strsql, String.Format("CardNo like '{0}%'", AliceDAL.Common.cutBadStr(txtPrefix)));
            ListModel.CreatePage(model, "[uv_VipCard]", strsql, page, WorkContext.PageSize, "ID");

            switch (btnSearch)
            {
                case "导出表格":
                    AddAdminLog("导出会员卡", "导出会员卡,卡号为:" + txtCardNo + ";卡名称为:" + txtTitle + ";卡号前缀为:" + txtPrefix + ";商家ID为:" + BusID);
                    string strHeader = "卡号,卡密,状态";
                    string strCol = "CardNo,CardPwd,CardState";
                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DataKeysList("[uv_VipCard]", strCol, strsql.ToString(), "ID asc,CDate", 0);
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
                                if (i == 2)
                                {
                                    string result = "";
                                    int state = AliceDAL.DataType.ConvertToInt(dgvr[ColArr[i]].ToString());
                                    switch (state)
                                    {
                                        case 10:
                                            result = "正常";
                                            break;
                                        case 30:
                                            result = "已使用";
                                            break;
                                        case 50:
                                            result = "作废";
                                            break;
                                        default:
                                            break;
                                    }
                                    dr[HeaderArr[i]] = result;
                                }
                                else dr[HeaderArr[i]] = dgvr[ColArr[i]];
                            }
                            dtt.Rows.Add(dr);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("会员卡导出" + txtPrefix + ".xls"));
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
        public ActionResult VipCardUsedList(string txtCardNo, string txtMobile, string txtTitle, string btnSearch, int BranchID = 0, int BusID = 0, int page = 1)
        {
            Load();
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[CardState]=30");
            if (WorkContext.RoleID == 1)
            {
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            if (BusID > 0) AliceDAL.Common.SqlString(strsql, String.Format("BusinessID={0}", BusID));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (!String.IsNullOrEmpty(txtCardNo)) AliceDAL.Common.SqlString(strsql, String.Format("CardNo like '%{0}%'", AliceDAL.Common.cutBadStr(txtCardNo)));
            ListModel.CreatePage(model, "[uv_VipCard]", strsql, page, WorkContext.PageSize, "UseDate desc,ID");

            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "卡号,名称,手机号,车牌号,商家,项目,次数,已使用,兑换日期";
                    string strCol = "CardNo,Title,Mobile,Plate,BusinessName,Project,CardCount,UserCount,UseDate";

                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[uv_VipCard]", strsql.ToString(), "UseDate desc,ID", 0);
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
                                        string d = dgvr["ItemTitle"].ToString() + "(" + dgvr["ItemPrice"].ToString() + ")";
                                        dr[HeaderArr[i]] = d;
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
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("会员卡使用记录" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
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
        public ActionResult VipCardAdd(Models.VipCardSingleModel model)
        {
            InitCouponsList();
            if (AliceDAL.Common.IsGet())
            {
                model.TDate = DateTime.Now.AddMonths(1);
                return View(model);
            }
            if (String.IsNullOrEmpty(model.CardNo))
            {
                ModelState.AddModelError("CardNo", "卡号不能为空");
                return View(model);
            }
            else if (String.IsNullOrWhiteSpace(model.CardPwd))
            {
                ModelState.AddModelError("CardNo", "密码不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("CardNo", "请选择卡系列");
                return View(model);
            }
            else if (model.TDate <= DateTime.Now)
            {
                ModelState.AddModelError("CardNo", "到期日期不正确");
                return View(model);
            }
            DataTable dt = DAL.PageModel.Table_Model("uv_VipType", "[ID]=" + model.TypeID);
            if (dt == null || dt.Rows.Count <= 0)
            {
                ModelState.AddModelError("CardNo", "卡系列不存在");
                return View(model);
            }

            VipCard vc = new VipCard()
            {
                BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString()),
                CardNo = model.CardNo,
                CardPwd = model.CardPwd,
                CardState = (int)VipCardState.Normal,
                Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString()),
                SMSContent = model.SMSContent,
                Title = model.Title,
                TDate = model.TDate,
                CardCount = DataType.ConvertToInt(dt.Rows[0]["Count"].ToString()),
                TypeID = model.TypeID
            };
            if (String.IsNullOrEmpty(model.Title))
            {
                vc.Title = dt.Rows[0]["Title"].ToString();
            }
            if (String.IsNullOrEmpty(model.SMSContent))
            {
                vc.SMSContent = dt.Rows[0]["SMSContent"].ToString();
            }
            string result = DAL.VipCard.Add(vc);
            if (result == "exists no")
            {
                ModelState.AddModelError("CardNo", "卡号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加会员卡", "添加会员卡,卡号为:" + model.CardNo + ";卡名称为:" + vc.Title + ";卡项目为:" + vc.TypeID + ";商家ID为:" + vc.BusinessID + ";使用次数为:" + vc.CardCount);
                return PromptView(Url.Action("VipCardList"), "会员卡卡添加成功");
            }
        }
        public ActionResult VipCardBatchAdd(Models.VipCardBatchModel model)
        {
            InitCouponsList();
            if (AliceDAL.Common.IsGet())
            {
                model.TDate = DateTime.Now.AddMonths(1);
                model.CardNoPrefix = DateTime.Now.ToString("yyMMdd");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.CardNoPrefix))
            {
                ModelState.AddModelError("CardNoPrefix", "卡号前缀不能为空");
                return View(model);
            }
            else if (model.CardNoStart <= 0)
            {
                ModelState.AddModelError("CardNoPrefix", "卡号起始不正确");
                return View(model);
            }
            else if (model.CardNoEnd <= 0)
            {
                ModelState.AddModelError("CardNoPrefix", "卡号结尾不正确");
                return View(model);
            }
            else if (model.CardNoEnd < model.CardNoStart)
            {
                ModelState.AddModelError("CardNoPrefix", "结尾不能小于开始");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("CardNo", "请选择卡系列");
                return View(model);
            }
            else if (model.TDate <= DateTime.Now)
            {
                ModelState.AddModelError("CardNo", "到期日期不正确");
                return View(model);
            }
            DataTable dt = DAL.PageModel.Table_Model("uv_VipType", "[ID]=" + model.TypeID);
            if (dt == null || dt.Rows.Count <= 0)
            {
                ModelState.AddModelError("CardNo", "卡系列不存在");
                return View(model);
            }
            List<VipCard> list = new List<VipCard>();
            for (int i = model.CardNoStart; i <= model.CardNoEnd; i++)
            {
                string pwd = AliceDAL.Common.CreatePwd(6, i);
                VipCard vc = new VipCard()
                {
                    BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString()),
                    CardNo = model.CardNoPrefix + i.ToString().PadLeft(4, '0'),
                    CardPwd = pwd,
                    CardState = (int)VipCardState.Normal,
                    Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString()),
                    SMSContent = model.SMSContent,
                    Title = model.Title,
                    TDate = model.TDate,
                    CardCount = DataType.ConvertToInt(dt.Rows[0]["Count"].ToString()),
                    TypeID = model.TypeID
                };
                if (String.IsNullOrEmpty(model.Title))
                {
                    vc.Title = dt.Rows[0]["Title"].ToString();
                }
                if (String.IsNullOrEmpty(model.SMSContent))
                {
                    vc.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                }
                list.Add(vc);
            }

            bool result = DAL.VipCard.AddList(list);
            if (!result)
            {
                ModelState.AddModelError("CardNoPrefix", "添加失败，请检查");
                return View(model);
            }
            else
            {
                AddAdminLog("批量添加会员卡", "批量添加会员卡,卡号前缀为:" + model.CardNoPrefix + ";卡名称为:" + model.Title + ";卡号起始为:" + model.CardNoStart + ";卡号结尾为:" + model.CardNoEnd + ";卡号到期为:" + model.TDate + ";卡系列为:" + model.TypeID + ";商家ID为:" + dt.Rows[0]["BusinessID"].ToString());
                return PromptView(Url.Action("VipCardList"), "会员卡批量添加成功");
            }
        }
        public ActionResult CardChangeState(int sid = -1, int page = 1)
        {
            VipCard sc = DAL.VipCard.GetModel(sid);
            if (sc == null) return PromptView("此会员卡不存在");

            DAL.VipCard.ChangeState(sid, 50);
            AddAdminLog("作废会员卡", "作废会员卡,卡号为:" + sc.CardNo);
            return PromptView(Url.Action("VipCardList", new { page = page.ToString() }), "会员卡作废成功");
        }
        public ActionResult VipCardEdit(Models.VipCardSingleModel model, int sid = -1, int page = 1)
        {
            InitCouponsList();
            VipCard vc = DAL.VipCard.GetModel(sid);
            if (vc == null) return PromptView("此会员卡不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.CardNo = vc.CardNo;
                model.CardPwd = vc.CardPwd;
                model.Title = vc.Title;
                model.SMSContent = vc.SMSContent;
                model.TypeID = vc.TypeID;
                model.TDate = vc.TDate;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.CardNo))
            {
                ModelState.AddModelError("CardNo", "卡号不能为空");
                return View(model);
            }
            else if (String.IsNullOrWhiteSpace(model.CardPwd))
            {
                ModelState.AddModelError("CardNo", "密码不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("CardNo", "请选择卡系列");
                return View(model);
            }
            else if (model.TDate <= DateTime.Now)
            {
                ModelState.AddModelError("CardNo", "到期日期不正确");
                return View(model);
            }

            DataTable dt = DAL.PageModel.Table_Model("uv_VipType", "[ID]=" + model.TypeID);
            if (dt == null || dt.Rows.Count <= 0)
            {
                ModelState.AddModelError("CardNo", "卡系列不存在");
                return View(model);
            }

            VipCard vv = new VipCard()
            {
                BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString()),
                CardNo = model.CardNo,
                CardPwd = model.CardPwd,
                CardState = (int)VipCardState.Normal,
                Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString()),
                SMSContent = model.SMSContent,
                Title = model.Title,
                TDate = model.TDate,
                CardCount = DataType.ConvertToInt(dt.Rows[0]["Count"].ToString()),
                TypeID = model.TypeID
            };
            vv.ID = vc.ID;
            if (String.IsNullOrEmpty(model.Title))
            {
                vc.Title = dt.Rows[0]["Title"].ToString();
            }
            if (String.IsNullOrEmpty(model.SMSContent))
            {
                vc.SMSContent = dt.Rows[0]["SMSContent"].ToString();
            }

            string result = DAL.VipCard.Edit(vv);
            if (result == "exists no")
            {
                ModelState.AddModelError("CardNo", "卡号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("修改会员卡", "修改会员卡,原卡号为:" + vc.CardNo + ";原卡名称为:" + vc.Title + ";原卡项目为:" + vc.TypeID + ";原商家ID为:" + vc.BusinessID + "现卡号为:" + vv.CardNo + ";现卡名称为:" + vv.Title + ";现卡项目为:" + vv.TypeID + ";现商家ID为:" + vv.BusinessID);
                return PromptView(Url.Action("VipCardList") + "?page=" + page.ToString(), "会员卡修改成功");
            }
        }
        private void Load()
        {
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID > 1)
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            }
            ViewData["businessList"] = DAL.BD_Business.GetList(strsql.ToString());
        }
        private void InitCouponsList()
        {
            List<Models.VipTypeToBusi> cttb = new List<Models.VipTypeToBusi>();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID > 1)
            {
                AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            }
            DataTable dt = DAL.PageModel.DataKeysList("[BD_Business]", "[ID],[BusinessName]", strsql.ToString(), "SortID", 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Models.VipTypeToBusi cttbm = new Models.VipTypeToBusi()
                    {
                        BusinessID = DataType.ConvertToInt(item["ID"].ToString()),
                        BusinessName = item["BusinessName"].ToString(),
                    };

                    DataTable dttype = DAL.PageModel.DataKeysList("[VipType]", "[ID],[Title]", "[BusinessID]=" + cttbm.BusinessID, "ID", 0);
                    List<Models.VipTypeSimple> lst = new List<Models.VipTypeSimple>();
                    if (dttype != null && dttype.Rows.Count > 0)
                    {
                        foreach (DataRow row in dttype.Rows)
                        {
                            Models.VipTypeSimple cts = new Models.VipTypeSimple()
                            {
                                Title = row["Title"].ToString(),
                                TypeID = DataType.ConvertToInt(row["ID"].ToString())
                            };
                            lst.Add(cts);
                        }
                    }
                    cttbm.TypeList = lst;
                    cttb.Add(cttbm);
                }

            }
            ViewData["businessVipTypeList"] = cttb;
        }
        /// <summary>
        /// 充值测试
        /// </summary>
        /// <returns></returns>
        //public ActionResult ChargeTemp()
        //{
        //    if (AliceDAL.Common.IsGet()) return View();
        //    string mobile = AliceDAL.Common.GetFormString("mobile");
        //    string cardno = AliceDAL.Common.GetFormString("cardno");
        //    string cardpwd = AliceDAL.Common.GetFormString("cardpwd");

        //    string result = "";
        //    StoreCard sc = DAL.StoreCard.Exchange(cardno, cardpwd, out result);
        //    if (sc != null && sc.CardNo == cardno)
        //    {
        //        BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
        //        if (bu != null)
        //        {
        //            DAL.BD_Users.EditUserWalletByUserID(bu.ID, sc.Price, 0);
        //            DAL.StoreCard.ChangeState(sc.ID, 70);
        //            //增加兑换记录
        //            AdminOrders order = new AdminOrders();
        //            order.OrderAmount = sc.Price;
        //            order.OrderState = (int)OrderState.Confirming;
        //            order.Osn = DAL.AdminOrders.GenerateOSN(bu.ID);
        //            order.TypeID = 0;
        //            order.Uid = bu.ID;
        //            order.Count = 1;
        //            order.PaySn = sc.CardNo;
        //            order.PayName = "储值卡充值";
        //            order.PayTime = DateTime.Now;
        //            DAL.AdminOrders.CreateOrder(order);
        //            return Content("{\"msg\":\"兑换成功\"}");
        //        }
        //        else
        //        {
        //            return Content("{\"msg\":\"用户不存在\"}");
        //        }
        //    }
        //    else
        //    {
        //        return Content("{\"msg\":\"" + result + "\"}");
        //    }
        //}
    }
}
