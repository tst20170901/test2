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

namespace Web.Areas.AliceChopper.Controllers
{
    public class ChargeTypeController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[ChargeType]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult AdminOrdersList(string txtTitle, string txtMobile, string btnSearch, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
                if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
                ListModel.CreatePage(model, "[uv_AdminOrders]", strsql, page, WorkContext.PageSize, "ID");
            }
            else
            {
                if (!String.IsNullOrEmpty(txtMobile))
                {
                    if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn ='{0}'", AliceDAL.Common.cutBadStr(txtTitle)));
                    AliceDAL.Common.SqlString(strsql, String.Format("Mobile='{0}'", AliceDAL.Common.cutBadStr(txtMobile)));
                    ListModel.CreatePage(model, "[uv_AdminOrders]", strsql, page, WorkContext.PageSize, "ID");
                }
            }

            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "订单编号,手机号,订单金额,状态,交易方式,交易码,时间";
                    string strCol = "Osn,Mobile,OrderAmount,OrderState,PayName,PaySn,CDate";
                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[uv_AdminOrders]", strsql.ToString(), "ID", 0);
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
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("充值流水" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
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
        public ActionResult OrdersActionList(string txtTitle, string txtMobile, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
                if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("LoginID like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
                ListModel.CreatePage(model, "[uv_OrderActions]", strsql, page, WorkContext.PageSize, "ID");
            }
            else
            {
                if (!String.IsNullOrEmpty(txtMobile))
                {
                    if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn ='{0}'", AliceDAL.Common.cutBadStr(txtTitle)));
                    AliceDAL.Common.SqlString(strsql, String.Format("LoginID='{0}'", AliceDAL.Common.cutBadStr(txtMobile)));
                    ListModel.CreatePage(model, "[uv_OrderActions]", strsql, page, WorkContext.PageSize, "ID");
                }
            }
            return View(model);
        }
        public ActionResult ProOrdersActionList(string txtTitle, string txtMobile, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("ActionType = '商品支付'");
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
                if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("LoginID like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
                ListModel.CreatePage(model, "[uv_ProOrderActions]", strsql, page, WorkContext.PageSize, "ID");
            }
            else
            {
                if (!String.IsNullOrEmpty(txtMobile))
                {
                    if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn ='{0}'", AliceDAL.Common.cutBadStr(txtTitle)));
                    AliceDAL.Common.SqlString(strsql, String.Format("LoginID='{0}'", AliceDAL.Common.cutBadStr(txtMobile)));
                    ListModel.CreatePage(model, "[uv_ProOrderActions]", strsql, page, WorkContext.PageSize, "ID");
                }
            }
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult ChargeForOther(Models.ChargeForOtherModel model)
        {
            if (AliceDAL.Common.IsGet())
            {
                model.Content = "【小熊洗车】赠送您洗车费xxx元，请关注小熊洗车公众号，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "手机号码不能为空");
                return View(model);
            }
            if (model.Money <= 0)
            {
                ModelState.AddModelError("Money", "金额不正确");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Content))
            {
                ModelState.AddModelError("Content", "内容不能为空");
                return View(model);
            }
            string[] mob = model.Mobile.Split(',');
            bool i = false;
            foreach (string m in mob)
            {
                if (!AliceDAL.ValidateHelper.IsMobile(m))
                {
                    i = true;
                }
            }
            if (i)
            {
                ModelState.AddModelError("Mobile", "手机号码中存在错误号码");
                return View(model);
            }
            StringBuilder sb = new StringBuilder();
            foreach (string m in mob)
            {
                sb.Append(m + ",");
                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";

                user.LoginID = m;
                user.Mobile = m;
                user.UserPwd = AliceDAL.SecureHelper.MD5("123456");
                int uid = DAL.BD_Users.GiveMoney(user, model.Money);

                AdminOrders order = new AdminOrders();
                order.OrderAmount = model.Money;
                order.OrderState = (int)OrderState.Confirming;
                order.Osn = DAL.AdminOrders.GenerateOSN(uid);
                order.TypeID = 0;
                order.Uid = uid;
                order.Count = 1;
                order.PayName = "赠送充值";
                order.PayTime = DateTime.Now;
                DAL.AdminOrders.CreateOrder(order);
            }
            MessageManger.PostMsg(sb.ToString().TrimEnd(','), model.Content);
            AddAdminLog("为他人充值", "手机号码:" + model.Mobile + ";金额为:" + model.Money + ";内容为:" + model.Content);
            return PromptView("/AliceChopper/ChargeType/ChargeForOther", "赠送添加成功");
        }

        [ValidateInput(false)]
        public ActionResult ChargeForUser(Models.ChargeForUserModel model)
        {
            if (AliceDAL.Common.IsGet())
            {
                model.Content = "【小熊洗车】为您充值洗车费xxx元，请关注小熊洗车公众号，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "手机号码不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Code))
            {
                ModelState.AddModelError("Mobile", "验证码不能为空");
                return View(model);
            }
            if (model.Money <= 0)
            {
                ModelState.AddModelError("Money", "金额不正确");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Content))
            {
                ModelState.AddModelError("Content", "内容不能为空");
                return View(model);
            }

            if (!AliceDAL.ValidateHelper.IsMobile(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "手机号码不正确");
                return View(model);
            }
            string result;
            BD_Users admin = new DAL.BD_Users().LoginCode(model.Mobile, model.Code, out result);
            if (null != admin)
            {

                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";

                user.LoginID = model.Mobile;
                user.Mobile = model.Mobile;
                user.UserPwd = AliceDAL.SecureHelper.MD5("123456");
                int uid = DAL.BD_Users.GiveMoneyUser(user, model.Money, model.Money1);

                AdminOrders order = new AdminOrders();
                order.OrderAmount = model.Money + model.Money1;
                order.OrderState = (int)OrderState.Confirming;
                order.Osn = DAL.AdminOrders.GenerateOSN(uid);
                order.TypeID = 0;
                order.Uid = uid;
                order.Count = 1;
                order.PayName = "手动充值";
                order.PayTime = DateTime.Now;
                DAL.AdminOrders.CreateOrder(order);
                MessageManger.PostMsg(model.Mobile, model.Content);
                AddAdminLog("手动充值", "手机号码:" + model.Mobile + ";金额为:" + model.Money + ";赠送金额为:" + model.Money1 + ";内容为:" + model.Content);
                return PromptView("/AliceChopper/ChargeType/ChargeForUser", "手动充值添加成功");
            }
            else
            {
                ModelState.AddModelError("Mobile", result);
                return View(model);
            }
        }
        public ActionResult Add(ChargeType model)
        {
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "套餐名称不能为空");
                return View(model);
            }
            if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "价格不正确");
                return View(model);
            }
            ChargeType wi = new ChargeType()
            {
                GivePrice = model.GivePrice,
                Price = model.Price,
                State = 10,
                Title = model.Title,
                Remark = model.Remark
            };
            DAL.ChargeType.Add(wi);
            AddAdminLog("套餐添加", "套餐名称为:" + wi.Title + ";价格为:" + wi.Price + ";赠送金额为:" + wi.GivePrice + ";赠送礼品:" + wi.Remark);
            return PromptView("/AliceChopper/ChargeType/List", "套餐添加成功");
        }
        public ActionResult Edit(ChargeType model, int fid = -1)
        {
            ChargeType wi = DAL.ChargeType.GetModel(fid);
            if (wi == null) return PromptView("此套餐不存在");
            if (AliceDAL.Common.IsGet()) return View(wi);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "价格不正确");
                return View(model);
            }
            ChargeType item = new ChargeType()
            {
                GivePrice = model.GivePrice,
                Price = model.Price,
                Title = model.Title,
                Remark = model.Remark,
                ID = wi.ID
            };
            DAL.ChargeType.Edit(item);
            AddAdminLog("套餐修改", "套餐ID为:" + fid + ";修改前名称为:" + wi.Title + ";价格为:" + wi.Price + ";赠送金额为:" + wi.GivePrice + ";赠送礼品:" + wi.Remark + ";修改后名称为:" + item.Title + ";价格为:" + item.Price + ";赠送金额为:" + item.GivePrice);
            return PromptView("/AliceChopper/ChargeType/List", "套餐修改成功");
        }
        public ActionResult ChangeState(int fid = -1)
        {
            ChargeType wi = DAL.ChargeType.GetModel(fid);
            if (wi == null) return PromptView("此套餐不存在");
            DAL.ChargeType.ChangeState(fid, wi.State == 0 ? 10 : 0);
            AddAdminLog("套餐更改状态", (wi.State == 0 ? "启用" : "禁用") + "套餐,套餐ID为:" + fid + ";名称为:" + wi.Title + ";价格为:" + wi.Price);
            return RedirectToAction("List");
        }
        public ActionResult SendMsg()
        {
            string LoginID = AliceDAL.UrlParam.GetStringValue("loginID");
            string result, url = "";
            if (String.IsNullOrWhiteSpace(LoginID))
            {
                url = "";
                result = "手机号码不能为空！";
                return AjaxResult(url, result);
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
            {
                url = "";
                result = "手机号码格式不正确！";
                return AjaxResult(url, result);
            }

            try
            {
                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = "";

                user.LoginID = LoginID;
                user.Mobile = LoginID;
                user.UserPwd = AliceDAL.SecureHelper.MD5("123456");
                string code = AliceDAL.Common.CreateRandomValue(4, true);
                user.Data10 = code;
                string con = "【小熊洗车】您的手机验证码为：" + code + "。本信息请勿回复，如需帮助请拨打客服电话：0318-8888235，祝您洗车愉快。";
                MessageManger.PostMsg(LoginID, con);
                DAL.BD_Users.EditCode(user);
                url = "success";
                result = "验证码发送成功";
                return AjaxResult(url, result);
            }
            catch (Exception ex)
            {
                url = "error";
                result = "系统错误，请稍候再试！";
                return AjaxResult(url, result);
            }
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
