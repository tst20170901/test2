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
    public class VipTypeController : AdminBaseController
    {
        public ActionResult VipTypeList(string txtTitle, string txtBusinessName, int Online = -1, int BranchID = -1, int page = 1)
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
            if (!String.IsNullOrEmpty(txtBusinessName)) AliceDAL.Common.SqlString(strsql, String.Format("BusinessName like '%{0}%'", AliceDAL.Common.cutBadStr(txtBusinessName)));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (Online > 0) AliceDAL.Common.SqlString(strsql, String.Format("[Online]={0}", Online));
            ListModel.CreatePage(model, "[uv_VipType]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult VipTypeAdd(Models.VipTypeModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.SMSContent = "【小熊洗车】xxx赠送您x元优惠券，请关注小熊洗车公众号或者下载APP，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
                model.Count = 1;
                model.Online = 1;
                model.LockPlate = 1;
                model.CardCount = 1000;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "类型名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "金额不正确");
                return View(model);
            }
            else if (model.Count <= 0)
            {
                ModelState.AddModelError("Title", "使用次数不正确");
                return View(model);
            }
            else if (model.CardCount <= 0)
            {
                ModelState.AddModelError("Title", "限购数量不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Title", "请选择商家");
                return View(model);
            }
            else if (model.FreeID == 0)
            {
                ModelState.AddModelError("Title", "请选择使用项目");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.SMSContent))
            {
                ModelState.AddModelError("Title", "短信内容不能为空");
                return View(model);
            }
            VipType vt = new VipType()
            {
                BusinessID = model.BusinessID,
                Count = model.Count,
                FreeID = model.FreeID,
                VipDes = model.VipDes,
                VipTypeState = 10,
                Period = model.Period,
                Price = model.Price,
                SMSContent = model.SMSContent,
                Title = model.Title,
                Online = model.Online,
                LockPlate = model.LockPlate,
                CardCount = model.CardCount
            };
            vt.Data1 = vt.Data2 = vt.Data3 = vt.Data4 = vt.Data5 = vt.Data6 = vt.Data7 = vt.Data8 = vt.Data9 = vt.Data10 = "";
            string result = DAL.VipType.Add(vt);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加会员卡类型", "添加会员卡类型,名称为:" + model.Title + ";金额为:" + model.Price + ";使用次数为:" + model.Count + ";商家ID为:" + model.BusinessID + ";服务项目ID为:" + model.FreeID + ";限购数量:" + model.CardCount);
                return PromptView(Url.Action("VipTypeList"), "会员卡类型添加成功");
            }
        }
        public ActionResult VipTypeEdit(Models.VipTypeModel model, int cid = -1, int page = 1)
        {
            Load();
            VipType ct = DAL.VipType.GetModel(cid);
            if (ct == null) return PromptView("此会员卡类型不存在");
            if (AliceDAL.Common.IsGet())
            {
                model.BusinessID = ct.BusinessID;
                model.Price = ct.Price;
                model.SMSContent = ct.SMSContent;
                model.Title = ct.Title;
                model.Count = ct.Count;
                model.FreeID = ct.FreeID;
                model.VipDes = ct.VipDes;
                model.CardCount = ct.CardCount;
                model.Online = ct.Online;
                model.LockPlate = ct.LockPlate;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "类型名称不能为空");
                return View(model);
            }
            else if (model.Price <= 0)
            {
                ModelState.AddModelError("Title", "金额不正确");
                return View(model);
            }
            else if (model.Count <= 0)
            {
                ModelState.AddModelError("Title", "使用次数不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Title", "请选择商家");
                return View(model);
            }
            else if (model.CardCount <= 0)
            {
                ModelState.AddModelError("Title", "限购数量不正确");
                return View(model);
            }
            else if (model.FreeID == 0)
            {
                ModelState.AddModelError("Title", "请选择使用项目");
                return View(model);
            }
            else if (String.IsNullOrEmpty(model.SMSContent))
            {
                ModelState.AddModelError("Title", "短信内容不能为空");
                return View(model);
            }

            VipType mo = new VipType()
            {
                BusinessID = model.BusinessID,
                Count = model.Count,
                VipTypeState = ct.VipTypeState,
                VipDes = model.VipDes,
                FreeID = model.FreeID,
                Period = 0,
                Price = model.Price,
                SMSContent = model.SMSContent,
                Title = model.Title,
                CardCount = model.CardCount,
                Online = model.Online,
                LockPlate = model.LockPlate,
                ID = ct.ID
            };
            mo.Data1 = mo.Data2 = mo.Data3 = mo.Data4 = mo.Data5 = mo.Data6 = mo.Data7 = mo.Data8 = mo.Data9 = mo.Data10 = "";
            string result = DAL.VipType.Edit(mo);
            if (result == "exists title")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else if (result == "1")
            {
                AddAdminLog("修改会员卡类型", "修改会员卡类型,现名称为:" + model.Title + ";现金额为:" + model.Price + ";现使用次数为:" + model.Count + ";现商家ID为:" + model.BusinessID + ";现服务项目ID为:" + model.FreeID + ";现限购数量为:" + model.CardCount + ";原名称为:" + ct.Title + ";原金额为:" + ct.Price + ";原使用次数为:" + ct.Count + ";原商家ID为:" + ct.BusinessID + ";原服务项目ID为:" + ct.FreeID + ";原限购数量为:" + ct.CardCount);
                return PromptView(Url.Action("VipTypeList", new { page = page.ToString() }), "会员卡修改成功");
            }
            else { return PromptView("系统错误，稍后再试"); }
        }
        public ActionResult VipTypeChangeState(int cid = -1, int page = 1)
        {
            VipType vt = DAL.VipType.GetModel(cid);
            if (vt == null) return PromptView("此会员卡类型不存在");
            DAL.VipType.ChangeState(cid, vt.VipTypeState == 30 ? 10 : 30);
            AddAdminLog("会员卡类型更改状态", (vt.VipTypeState == 30 ? "启用" : "禁用") + "会员卡类型,会员卡类型ID为:" + cid + ";名称为:" + vt.Title);
            return Redirect(Url.Action("VipTypeList", new { page = page.ToString() }));
        }
        private void Load()
        {
            StringBuilder strsql = new StringBuilder();
            AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            ViewData["businessList"] = DAL.BD_Business.GetList(strsql.ToString());
            ViewData["washitemList"] = DAL.Wash_Item.GetList("ItemState=10 and BranID=" + WorkContext.BranchID);
        }
    }
}
