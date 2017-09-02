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
    public class BusiActionController : AdminBaseController
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
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("ActionName like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_BD_BusiAction]", strsql, page, WorkContext.PageSize, "SortID desc,ID");
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult Add(BD_BusiAction model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet())
            {
                model.IsNewUser = 10;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.ActionName))
            {
                ModelState.AddModelError("ActionName", "活动名称不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("ActionName", "请选择分公司");
                return View(model);
            }
            BD_BusiAction bb = new BD_BusiAction()
            {
                ActionName = model.ActionName,
                Body = model.Body ?? "",
                IsNewUser = model.IsNewUser,
                DataType = model.DataType,
                SortID = model.SortID,
                BranchID = model.BranchID,
                SMSContent = model.SMSContent
            };
            string result = DAL.BD_BusiAction.Add(bb);
            if (result == "-2")
            {
                ModelState.AddModelError("ActionName", "活动名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加商家活动", "添加商家活动,商家活动名称为:" + model.ActionName + ";人群为:" + (model.IsNewUser == 0 ? "所有用户" : "新用户") + ";车辆信息为:" + (model.DataType == 0 ? "不需要" : "需要"));
                return PromptView(Url.Action("List"), "商家活动添加成功");
            }
        }
        [ValidateInput(false)]
        public ActionResult Edit(BD_BusiAction model, int bid = -1)
        {
            LoadBranch();
            BD_BusiAction bb = DAL.BD_BusiAction.GetModel(bid);
            if (bb == null) return PromptView("此商家活动不存在");
            if (AliceDAL.Common.IsGet()) return View(bb);

            if (String.IsNullOrEmpty(model.ActionName))
            {
                ModelState.AddModelError("ActionName", "活动名称不能为空");
                return View(model);
            }
            BD_BusiAction mm = new BD_BusiAction()
            {
                ActionName = model.ActionName,
                Body = model.Body ?? "",
                IsNewUser = model.IsNewUser,
                DataType = model.DataType,
                SortID = model.SortID,
                BranchID = bb.BranchID,
                ID = bb.ID,
                SMSContent = model.SMSContent
            };
            string result = DAL.BD_BusiAction.Edit(mm);
            if (result == "-2")
            {
                ModelState.AddModelError("BusinessName", "活动名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("商家活动更改", "商家活动ID为:" + bid + ";名称为:" + model.ActionName + ";原名称为:" + bb.ActionName + ";人群为:" + (model.IsNewUser == 0 ? "所有用户" : "新用户") + ";原人群为:" + (bb.IsNewUser == 0 ? "所有用户" : "新用户") + ";车辆信息为:" + (model.DataType == 0 ? "不需要" : "需要") + ";原车辆信息为:" + (bb.DataType == 0 ? "不需要" : "需要"));
                return PromptView(Url.Action("List"), "商家活动修改成功");
            }
        }
        public ActionResult ChangeState(int bid = -1)
        {
            BD_BusiAction bb = DAL.BD_BusiAction.GetModel(bid);
            if (bb == null) return PromptView("此商家活动不存在");
            DAL.BD_BusiAction.ChangeState(bid, bb.ActionState == 30 ? 10 : 30);
            AddAdminLog("商家活动更改状态", (bb.ActionState == 30 ? "启用" : "禁用") + "商家活动,商家活动ID为:" + bid + ";名称为:" + bb.ActionName);
            return RedirectToAction("List");
        }
        [ValidateInput(false)]
        public ActionResult ItemList(BD_BusiAction_Item model, int bid, int id = -1, string a = "list")
        {
            InitCouponsList();
            switch (a)
            {
                case "list":
                    if (bid <= 0)
                    {
                        return PromptView(Url.Action("List"), "参数不正确！");
                    }
                    ViewData["list"] = DAL.PageModel.DateList("[uv_BD_BusiAction_Item]", "[ActionID]=" + bid, "ID", 0);
                    if (AliceDAL.Common.IsGet()) return View(model);
                    if (model.ItemID < 0)
                    {
                        ModelState.AddModelError("Msg", "请选择优惠券");
                        return View(model);
                    }
                    else if (model.Count <= 0)
                    {
                        ModelState.AddModelError("Msg", "数量不正确");
                        return View(model);
                    }

                    CouponsType ct = DAL.CouponsType.GetModel(model.ItemID);
                    if (ct == null)
                    {
                        ModelState.AddModelError("Msg", "优惠券不存在");
                        return View(model);
                    }
                    model.ActionID = bid;
                    model.Count = model.Count;
                    model.ItemPrice = ct.Price;
                    model.ItemTitle = ct.Title;
                    model.Period = ct.Period;
                    string result = DAL.BD_BusiAction.AddItem(model);
                    if ("1" == result)
                    {
                        ModelState.AddModelError("Msg", "添加成功");
                        ViewData["list"] = DAL.PageModel.DateList("[uv_BD_BusiAction_Item]", "[ActionID]=" + bid, "ID", 0);
                        return View(model);
                    }
                    else if ("-2" == result)
                    {
                        ModelState.AddModelError("Msg", "此优惠券已经存在");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "添加失败，请稍候再试");
                        return View(model);
                    }
                case "d":
                    if (DAL.PageModel.Delete("[BD_BusiAction_Item]", "[ID]=" + id) > 0)
                    {
                        return RedirectToAction("ItemList", "BusiAction", new { bid = bid });
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "删除失败，请稍候再试");
                        return View();
                    }
                default:
                    return View();
            }
        }
        private void InitCouponsList()
        {
            List<Models.CouponsTypeToBusi> cttb = new List<Models.CouponsTypeToBusi>();
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
                    Models.CouponsTypeToBusi cttbm = new Models.CouponsTypeToBusi()
                    {
                        BusinessID = DataType.ConvertToInt(item["ID"].ToString()),
                        BusinessName = item["BusinessName"].ToString(),
                    };

                    DataTable dttype = DAL.PageModel.DataKeysList("[CouponsType]", "[ID],[Title]", "[BusinessID]=" + cttbm.BusinessID + " and [IsSpecial]=10", "ID", 0);
                    List<Models.CouponsTypeSimple> lst = new List<Models.CouponsTypeSimple>();
                    if (dttype != null && dttype.Rows.Count > 0)
                    {
                        foreach (DataRow row in dttype.Rows)
                        {
                            Models.CouponsTypeSimple cts = new Models.CouponsTypeSimple()
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
            ViewData["businessCouponsTypeList"] = cttb;
        }
    }
}
