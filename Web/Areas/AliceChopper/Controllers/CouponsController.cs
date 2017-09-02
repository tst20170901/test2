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
using Newtonsoft.Json;
using AliceDAL;
using System.IO;

namespace Web.Areas.AliceChopper.Controllers
{
    public class CouponsController : AdminBaseController
    {
        public ActionResult CouponsList(string txtMobile, int BranchID = -1, int CouState = 0, int TypeID = 0, int page = 1)
        {
            InitCouponsList();
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
            if (CouState > 0) AliceDAL.Common.SqlString(strsql, String.Format("CouponState={0}", CouState));
            if (!String.IsNullOrEmpty(txtMobile)) AliceDAL.Common.SqlString(strsql, String.Format("LoginID like '%{0}%'", AliceDAL.Common.cutBadStr(txtMobile)));
            if (TypeID > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", TypeID));
            ListModel.CreatePage(model, "[uv_Coupons]", strsql, page, WorkContext.PageSize, "CDate desc,ID");
            return View(model);
        }
        public ActionResult InitCouponsTypes(int businessID)
        {
            DataTable dt = DAL.PageModel.DataKeysList("[CouponsType]", "[ID],[Title]", String.Format("[BusinessID]={0} and [IsSpecial]=10", businessID), "ID", 0);
            string result = JsonConvert.SerializeObject(dt);
            return Content(result);
        }
        public ActionResult CouponsAdd(Models.CouponsSingleModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.SMSContent = "【小熊洗车】xxx赠送您x元优惠券，请关注小熊洗车公众号或者下载APP，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
                model.Count = 1;
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "手机号码不能为空");
                return View(model);
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "手机号码不正确");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Mobile", "请选择商家");
                return View(model);
            }
            else if (model.Count <= 0)
            {
                ModelState.AddModelError("Mobile", "数量不正确");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("Mobile", "请选择优惠券");
                return View(model);
            }
            BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(model.Mobile);
            if (bu == null)
            {
                ModelState.AddModelError("Mobile", "会员不存在");
                return View(model);
            }
            CouponsType ct = DAL.CouponsType.GetModel(model.TypeID);
            if (ct == null)
            {
                ModelState.AddModelError("Mobile", "优惠券不存在");
                return View(model);
            }
            Coupons cou = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = ct.OriginalPrice,
                            Price = ct.Price,
                            TypeID = model.TypeID,
                            Uid = bu.ID,
                            TDate = DateTime.Now.AddDays(ct.Period)
                        };
            bool result = false;
            for (int i = 0; i < model.Count; i++)
            {
                result = DAL.Coupons.CreateCoupons(cou);
            }
            if (result)
            {
                if (!String.IsNullOrEmpty(model.SMSContent) && model.SMSContent.Length > 10)
                {
                    MessageManger.PostMsg(bu.Mobile, model.SMSContent);
                }
                AddAdminLog("发放优惠券", "手机号码:" + model.Mobile + ";优惠券ID为:" + ct.ID + ";名称为:" + ct.Title + ";数量是:" + model.Count);
                return PromptView(Url.Action("CouponsList"), "优惠券发放成功");
            }
            else
            {
                ModelState.AddModelError("Mobile", "优惠券发放失败，请稍候再试");
                return View(model);
            }
        }
        public ActionResult CouponsBatchAdd(Models.CouponsBatchModel model, HttpPostedFileBase uploadFile)
        {
            Load();
            if (AliceDAL.Common.IsGet())
            {
                model.SMSContent = "【小熊洗车】xxx赠送您x元优惠券，请关注小熊洗车公众号或者下载APP，使用此手机号码即刻使用。如需帮助请拨打：0318-8888235，祝您洗车愉快。";
                model.Count = 1;
                return View(model);
            }

            #region 收集手机号码
            List<string> list = new List<string>();
            if (uploadFile != null)
            {
                if (uploadFile.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/uploadfiles"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            String line;
                            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (!String.IsNullOrWhiteSpace(line) && AliceDAL.ValidateHelper.IsMobile(line))
                                    {
                                        list.Add(line);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Mobile", "请选择正确导入源");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Mobile", "请选择正确导入源");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Mobile", "请选择正确导入源");
                return View(model);
            }
            #endregion

            #region 条件判断
            if (list == null || list.Count <= 0)
            {
                ModelState.AddModelError("Mobile", "手机号码不能为空");
                return View(model);
            }
            else if (model.BusinessID == 0)
            {
                ModelState.AddModelError("Mobile", "请选择商家");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("Mobile", "请选择优惠券");
                return View(model);
            }
            else if (model.Count <= 0)
            {
                ModelState.AddModelError("Mobile", "数量不正确");
                return View(model);
            }
            CouponsType ct = DAL.CouponsType.GetModel(model.TypeID);
            if (ct == null)
            {
                ModelState.AddModelError("Mobile", "优惠券不存在");
                return View(model);
            }
            #endregion

            StringBuilder sb = new StringBuilder();
            foreach (string item in list)
            {
                BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(item);
                if (bu != null)
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        Coupons cou = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = ct.OriginalPrice,
                            Price = ct.Price,
                            TypeID = model.TypeID,
                            Uid = bu.ID,
                            TDate = DateTime.Now.AddDays(ct.Period)
                        };
                        DAL.Coupons.CreateCoupons(cou);
                    }
                    sb.Append(item + ",");
                }
            }
            if (!String.IsNullOrEmpty(model.SMSContent) && model.SMSContent.Length > 10)
            {
                MessageManger.PostMsg(sb.ToString().TrimEnd(','), model.SMSContent);
            }
            AddAdminLog("批量发放优惠券", "手机号码:" + sb.ToString() + ";手机号码数量:" + list.Count + ";优惠券ID为:" + ct.ID + ";名称为:" + ct.Title + ";数量是:" + model.Count);
            return PromptView(Url.Action("CouponsList"), "批量发放优惠券成功");
        }
        public ActionResult CouponsChangeState(int cid = -1, int page = 1)
        {
            Coupons c = DAL.Coupons.GetCouponsByID(cid);
            if (c == null) return PromptView("此优惠券不存在");
            if (c.CouponState == 10)
            {
                DAL.Coupons.ChangeState(cid, 50);
            }
            else
            {
                return PromptView("此优惠券不允许作废");
            }
            AddAdminLog("优惠券更改状态", "作废优惠券,优惠券ID为:" + cid);
            return PromptView(Url.Action("CouponsList", new { page = page.ToString() }), "优惠券作废成功");
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

                    DataTable dttype = DAL.PageModel.DataKeysList("[CouponsType]", "[ID],[Title]", "[BusinessID]=" + cttbm.BusinessID, "ID", 0);
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
