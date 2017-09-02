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
    public class BusinessController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int BranchID = -1, int page = 1)
        {
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
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("BusinessName like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_BD_Business]", strsql, page, WorkContext.PageSize, "SortID desc,ID");
            return View(model);
        }
        public ActionResult StoreCardList(string txtCardNo, string txtTitle, int BusID = 0, int page = 1)
        {
            Load();
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (BusID > 0) AliceDAL.Common.SqlString(strsql, String.Format("BusinessID={0}", BusID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (!String.IsNullOrEmpty(txtCardNo)) AliceDAL.Common.SqlString(strsql, String.Format("CardNo like '%{0}%'", AliceDAL.Common.cutBadStr(txtCardNo)));
            ListModel.CreatePage(model, "[uv_StoreCard]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult StoreCardAdd(Models.StoreCardSingleModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.SMSContent = "【小熊洗车】储值卡为您充值洗车费xxx元，请关注小熊洗车公众号或者下载APP，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
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
            else if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("CardNo", "卡名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("CardNo", "储值金额不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("CardNo", "请选择商家");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.SMSContent))
            {
                ModelState.AddModelError("CardNo", "短信内容不能为空");
                return View(model);
            }

            StoreCard sc = new StoreCard()
            {
                BusinessID = model.BusinessID,
                CardNo = model.CardNo,
                CardPwd = model.CardPwd,
                CardState = (int)StoreCardState.Normal,
                Price = model.Price,
                SMSContent = model.SMSContent,
                Title = model.Title
            };
            sc.Data1 = sc.Data2 = sc.Data3 = sc.Data4 = sc.Data5 = sc.Data6 = sc.Data7 = sc.Data8 = sc.Data9 = sc.Data10 = "";
            string result = DAL.StoreCard.Add(sc);
            if (result == "exists no")
            {
                ModelState.AddModelError("CardNo", "卡号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加储值卡", "添加储值卡,储值卡名称为:" + model.Title + ";卡号是:" + model.CardNo + ";金额是:" + model.Price + ";商家ID是:" + model.BusinessID);
                return PromptView(Url.Action("StoreCardList"), "储值卡添加成功");
            }
        }
        public ActionResult StoreCardBatchAdd(Models.StoreCardBatchModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.SMSContent = "【小熊洗车】储值卡为您充值洗车费xxx元，请关注小熊洗车公众号或者下载APP，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
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
            else if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("CardNoPrefix", "卡名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("CardNoPrefix", "储值金额不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("CardNoPrefix", "请选择商家");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.SMSContent))
            {
                ModelState.AddModelError("CardNoPrefix", "短信内容不能为空");
                return View(model);
            }
            List<StoreCard> list = new List<StoreCard>();
            for (int i = model.CardNoStart; i <= model.CardNoEnd; i++)
            {
                string pwd = AliceDAL.Common.CreatePwd(6, i);
                StoreCard sc = new StoreCard()
                {
                    BusinessID = model.BusinessID,
                    CardNo = model.CardNoPrefix + i.ToString().PadLeft(4, '0'),
                    CardPwd = pwd,
                    CardState = (int)StoreCardState.Normal,
                    Price = model.Price,
                    SMSContent = model.SMSContent,
                    Title = model.Title
                };
                sc.Data1 = sc.Data2 = sc.Data3 = sc.Data4 = sc.Data5 = sc.Data6 = sc.Data7 = sc.Data8 = sc.Data9 = sc.Data10 = "";
                list.Add(sc);
            }

            bool result = DAL.StoreCard.AddList(list);
            if (!result)
            {
                ModelState.AddModelError("CardNoPrefix", "添加失败，请检查");
                return View(model);
            }
            else
            {
                AddAdminLog("批量添加储值卡", "批量添加储值卡,储值卡名称为:" + model.Title + ";卡前缀是:" + model.CardNoPrefix + ";卡起始是:" + model.CardNoStart + ";卡结尾是:" + model.CardNoEnd + ";卡名称是:" + model.Title + ";金额是:" + model.Price + ";商家ID是:" + model.BusinessID);
                return PromptView(Url.Action("StoreCardList"), "储值卡批量添加成功");
            }
        }
        public ActionResult CardChangeState(int sid = -1, int page = 1)
        {
            StoreCard sc = DAL.StoreCard.GetModel(sid);
            if (sc == null) return PromptView("此储值卡不存在");
            if (sc.CardState == 10)
            {
                DAL.StoreCard.ChangeState(sid, 50);
            }
            else
            {
                return PromptView("此储值卡不允许作废");
            }
            AddAdminLog("储值卡更改状态", "作废储值卡,储值卡ID为:" + sid + ";名称为:" + sc.Title + ";卡号为:" + sc.CardNo);
            return PromptView(Url.Action("StoreCardList") + "?page=" + page.ToString(), "储值卡作废成功");
        }
        public ActionResult StoreCardEdit(Models.StoreCardSingleModel model, int sid = -1, int page = 1)
        {
            StoreCard sc = DAL.StoreCard.GetModel(sid);
            if (sc == null) return PromptView("此储值卡不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.BusinessID = sc.BusinessID;
                model.CardNo = sc.CardNo;
                model.CardPwd = sc.CardPwd;
                model.Price = sc.Price;
                model.SMSContent = sc.SMSContent;
                model.Title = sc.Title;
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
            else if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("CardNo", "卡名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("CardNo", "储值金额不正确");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.SMSContent))
            {
                ModelState.AddModelError("CardNo", "短信内容不能为空");
                return View(model);
            }
            StoreCard mm = new StoreCard()
            {
                CardNo = model.CardNo,
                CardPwd = model.CardPwd,
                CardState = (int)StoreCardState.Normal,
                Price = model.Price,
                SMSContent = model.SMSContent,
                Title = model.Title
            };
            mm.ID = sc.ID;
            mm.Data1 = mm.Data2 = mm.Data3 = mm.Data4 = mm.Data5 = mm.Data6 = mm.Data7 = mm.Data8 = mm.Data9 = mm.Data10 = "";
            string result = DAL.StoreCard.Edit(mm);
            if (result == "exists no")
            {
                ModelState.AddModelError("CardNo", "卡号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("储值卡修改", "储值卡修改,储值卡ID为:" + sc.ID + ";原名称为:" + sc.Title + ";原卡号为:" + sc.CardNo + ";原金额为:" + sc.Price + ";现名称为:" + model.Title + ";现卡号为:" + model.CardNo + ";现金额为:" + model.Price);
                return PromptView(Url.Action("StoreCardList") + "?page=" + page.ToString(), "储值卡修改成功");
            }
        }
        public ActionResult Add(BD_Business model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrEmpty(model.LoginID))
            {
                ModelState.AddModelError("LoginID", "登录名不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Data2))
            {
                ModelState.AddModelError("LoginID", "手机号不能为空");
                return View(model);
            }
            if (String.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "密码不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.BusinessName))
            {
                ModelState.AddModelError("BusinessName", "商家名称不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("BusinessName", "请选择分公司");
                return View(model);
            }
            BD_Business bb = new BD_Business()
            {
                LoginID = model.LoginID,
                Password = AliceDAL.SecureHelper.MD5(model.Password),
                BusinessName = model.BusinessName,
                SortID = model.SortID,
                TypeID = model.TypeID,
                BusinessState = 10,
                BranchID = model.BranchID,
                Data1 = model.Data1 ?? "",
                Data2 = model.Data2
            };
            bb.Data3 = bb.Data4 = bb.Data5 = bb.Data6 = bb.Data7 = bb.Data8 = bb.Data9 = bb.Data10 = "";
            string result = DAL.BD_Business.Add(bb);
            if (result == "-2")
            {
                ModelState.AddModelError("BusinessName", "登录名或者商家名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加商家", "添加商家,商家名称为:" + model.BusinessName + ";商家登录名:" + model.LoginID + ";分公司ID为:" + model.BranchID);
                return PromptView(Url.Action("List"), "商家添加成功");
            }
        }
        public ActionResult Edit(BD_Business model, int bid = -1)
        {
            LoadBranch();
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            if (bb == null) return PromptView("此商家不存在");
            if (AliceDAL.Common.IsGet()) return View(bb);

            if (String.IsNullOrEmpty(model.BusinessName))
            {
                ModelState.AddModelError("BusinessName", "商家名称不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Data2))
            {
                ModelState.AddModelError("BusinessName", "手机号不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("BusinessName", "请选择分公司");
                return View(model);
            }
            BD_Business mm = new BD_Business()
            {
                BusinessName = model.BusinessName,
                SortID = model.SortID,
                BranchID = model.BranchID,
                ID = bb.ID,
                Data1 = model.Data1 ?? "",
                Data2 = model.Data2,
                TypeID = model.TypeID
            };
            if (!String.IsNullOrWhiteSpace(model.Password)) mm.Password = AliceDAL.SecureHelper.MD5(model.Password);
            else mm.Password = bb.Password;
            mm.Data3 = mm.Data4 = mm.Data5 = mm.Data6 = mm.Data7 = mm.Data8 = mm.Data9 = mm.Data10 = "";
            string result = DAL.BD_Business.Edit(mm);
            if (result == "-2")
            {
                ModelState.AddModelError("BusinessName", "商家名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("商家更改", "商家ID为:" + bid + ";名称为:" + model.BusinessName + ";原名称为:" + bb.BusinessName);
                return PromptView(Url.Action("List"), "商家修改成功");
            }
        }
        public ActionResult ChangeState(int bid = -1)
        {
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            if (bb == null) return PromptView("此商家不存在");
            DAL.BD_Business.ChangeState(bid, bb.BusinessState == 0 ? 10 : 0);
            AddAdminLog("商家更改状态", (bb.BusinessState == 0 ? "启用" : "禁用") + "商家,商家ID为:" + bid + ";名称为:" + bb.BusinessName);
            return RedirectToAction("List");
        }
        public ActionResult EditPwd(int bid)
        {
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            if (bb == null) return PromptView("此商家不存在");
            DAL.BD_Business.EditPwd(bb.LoginID, AliceDAL.SecureHelper.MD5("123456"));
            AddAdminLog("重置商家密码", "重置商家密码,商家ID为:" + bid + ";名称为:" + bb.BusinessName);
            return PromptView("重置商家密码成功，密码为123456");
        }
        public ActionResult EditMoney(Models.BD_BusinessModel model, int bid = -1, int page = 1)
        {
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            if (bb == null) return PromptView("此商家不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.ID = bb.ID;
                model.LoginID = bb.LoginID;
                model.BusinessName = bb.BusinessName;
                model.Money = 0;
                model.Remark = "";
                model.Wallet = bb.Wallet;
                return View(model);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(model.Remark))
                {
                    ModelState.AddModelError("Remark", "修改原因必须填写");
                    return View(model);
                }
                if (DAL.BD_Business.EditWallet(bid, model.Money) > 0)
                {
                    AddAdminLog("商家余额变更", String.Format("商家余额变更,商家ID为:{0},商家帐号{1},原余额{2},当前余额{3},变更原因:{4}", bb.ID, bb.LoginID, bb.Wallet, bb.Wallet + model.Money, model.Remark));
                    return PromptView(Url.Action("List", new { page = page.ToString() }), "商家余额变更成功");
                }
                else
                {
                    return PromptView("余额变更失败");
                }
            }
        }
        public ActionResult ExChargeRecord(string txtTitle, string txtMobile, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("PayName='储值卡充值'");
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Osn like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            ListModel.CreatePage(model, "[uv_AdminOrders]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult CouponsTypeList(string txtTitle, string txtBusinessName, int BranchID = -1, int page = 1)
        {
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
            if (!String.IsNullOrEmpty(txtBusinessName)) AliceDAL.Common.SqlString(strsql, String.Format("BusinessName like '%{0}%'", AliceDAL.Common.cutBadStr(txtBusinessName)));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_CouponsType]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult CouponsTypeAdd(Models.CouponsTypeModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.Count = 1;
                model.Period = 7;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "卡名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "储值金额不正确");
                return View(model);
            }
            else if (model.Period <= 0)
            {
                ModelState.AddModelError("Title", "有效期不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Title", "请选择商家");
                return View(model);
            }
            CouponsType ct = new CouponsType()
            {
                BusinessID = model.BusinessID,
                Count = 1,
                IsSpecial = 10,
                OriginalPrice = model.Price,
                Period = model.Period,
                Price = model.Price,
                SMSContent = "",
                Title = model.Title
            };
            ct.Data1 = ct.Data2 = ct.Data3 = ct.Data4 = ct.Data5 = ct.Data6 = ct.Data7 = ct.Data8 = ct.Data9 = ct.Data10 = "";
            string result = DAL.CouponsType.Add(ct);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加优惠券", "添加优惠券,优惠券名称为:" + model.Title + ";金额为:" + model.Price + ";商家ID是:" + model.BusinessID + ";有效期:" + model.Period);
                return PromptView(Url.Action("CouponsTypeList"), "优惠券添加成功");
            }
        }
        public ActionResult CouponsTypeEdit(Models.CouponsTypeModel model, int cid = -1, int page = 1)
        {
            Load();
            CouponsType ct = DAL.CouponsType.GetModel(cid);
            if (ct == null) return PromptView("此优惠券不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.BusinessID = ct.BusinessID;
                model.Price = ct.Price;
                model.SMSContent = "";
                model.Title = ct.Title;
                model.Count = ct.Count;
                model.Period = ct.Period;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "卡名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "储值金额不正确");
                return View(model);
            }
            else if (model.Period <= 0)
            {
                ModelState.AddModelError("Title", "有效期不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Title", "请选择商家");
                return View(model);
            }
            CouponsType mo = new CouponsType()
            {
                BusinessID = model.BusinessID,
                Count = model.Count,
                IsSpecial = ct.IsSpecial,
                OriginalPrice = model.Price,
                Period = model.Period,
                Price = model.Price,
                SMSContent = "",
                Title = model.Title,
                ID = ct.ID
            };
            mo.Data1 = mo.Data2 = mo.Data3 = mo.Data4 = mo.Data5 = mo.Data6 = mo.Data7 = mo.Data8 = mo.Data9 = mo.Data10 = "";
            string result = DAL.CouponsType.Edit(mo);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else if (result == "1")
            {
                AddAdminLog("修改优惠券", "修改优惠券,优惠券现名称为:" + model.Title + ";金额为:" + model.Price + ";商家ID是:" + model.BusinessID + ";有效期是:" + model.Period + ";原名称:" + ct.Title + ";原金额:" + ct.Price + ";原商家ID是:" + ct.BusinessID + ";原有效期:" + ct.Period);
                return PromptView(Url.Action("CouponsTypeList", new { page = page.ToString() }), "优惠券修改成功");
            }
            else { return PromptView("系统错误，稍后再试"); }
        }
        public ActionResult CouponsTypeChangeState(int cid = -1, int page = 1)
        {
            CouponsType ct = DAL.CouponsType.GetModel(cid);
            if (ct == null) return PromptView("此优惠券不存在");
            DAL.CouponsType.ChangeState(cid, ct.IsSpecial == 30 ? 10 : 30);
            AddAdminLog("优惠券更改状态", (ct.IsSpecial == 30 ? "启用" : "禁用") + "优惠券,优惠券ID为:" + cid + ";名称为:" + ct.Title);
            return Redirect(Url.Action("CouponsTypeList", new { page = page.ToString() }));
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
        /// <summary>
        /// 充值测试
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeTemp()
        {
            //if (AliceDAL.Common.IsGet()) 
            return View();
            //string mobile = AliceDAL.Common.GetFormString("mobile");
            //string cardno = AliceDAL.Common.GetFormString("cardno");
            //string cardpwd = AliceDAL.Common.GetFormString("cardpwd");

            //string result = "";
            //StoreCard sc = DAL.StoreCard.Exchange(cardno, cardpwd, out result);
            //if (sc != null && sc.CardNo == cardno)
            //{
            //    BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
            //    if (bu != null)
            //    {
            //        DAL.BD_Users.EditUserWalletByUserID(bu.ID, sc.Price, 0);
            //        DAL.StoreCard.ChangeState(sc.ID, 70);
            //        //增加兑换记录
            //        AdminOrders order = new AdminOrders();
            //        order.OrderAmount = sc.Price;
            //        order.OrderState = (int)OrderState.Confirming;
            //        order.Osn = DAL.AdminOrders.GenerateOSN(bu.ID);
            //        order.TypeID = 0;
            //        order.Uid = bu.ID;
            //        order.Count = 1;
            //        order.PaySn = sc.CardNo;
            //        order.PayName = "储值卡充值";
            //        order.PayTime = DateTime.Now;
            //        DAL.AdminOrders.CreateOrder(order);
            //        return Content("{\"msg\":\"兑换成功\"}");
            //    }
            //    else
            //    {
            //        return Content("{\"msg\":\"用户不存在\"}");
            //    }
            //}
            //else
            //{
            //    return Content("{\"msg\":\"" + result + "\"}");
            //}
        }
    }
}
