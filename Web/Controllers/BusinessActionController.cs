using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
using System.Data;
using Web.Models;
namespace Web.Areas.AliceChopper.Controllers
{
    public class BusinessActionController : Controller
    {
        private static object _locker = new object();//锁对象
        public ActionResult ActionDetails(int id)
        {
            BD_BusiAction bb = DAL.BD_BusiAction.GetModel(id);

            if (bb != null && bb.ActionState == 10)
            {
                if (bb.DataType != 0) return Content("活动不存在");
                List<BD_BusiAction_Item> list = DAL.BD_BusiAction.GetItemListByActionID(id);
                if (list != null && list.Count > 0)
                {
                    BusinessActionModel bam = new BusinessActionModel();
                    bam.Body = bb.Body;
                    bam.ID = id;
                    bam.AllMoney = (int)list.Sum(x => x.ItemPrice * x.Count);
                    SeoInit.InitHead(bb.ActionName, "", "", this);
                    return View(bam);
                }
                else
                {
                    return Content("活动不存在");
                }
            }
            else
            {
                return Content("活动不存在");
            }
        }
        public ActionResult ActionDetailsWithCarInfo(int id)
        {
            BD_BusiAction bb = DAL.BD_BusiAction.GetModel(id);

            if (bb != null && bb.ActionState == 10)
            {
                if (bb.DataType != 10) return Content("活动不存在");
                List<BD_BusiAction_Item> list = DAL.BD_BusiAction.GetItemListByActionID(id);
                if (list != null && list.Count > 0)
                {
                    BusinessActionModel bam = new BusinessActionModel();
                    bam.Body = bb.Body;
                    bam.ID = id;
                    bam.AllMoney = (int)list.Sum(x => x.ItemPrice * x.Count);
                    SeoInit.InitHead(bb.ActionName, "", "", this);
                    return View(bam);
                }
                else
                {
                    return Content("活动不存在");
                }
            }
            else
            {
                return Content("活动不存在");
            }
        }
        public ActionResult PhoneSendMsg(int id)
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
            BD_BusiAction bb = DAL.BD_BusiAction.GetModel(id);
            if (bb == null && bb.ActionState == 30)
            {
                url = "";
                result = "活动不存在";
                return AjaxResult(url, result);
            }
            try
            {
                if (bb.IsNewUser == 10)
                {
                    BD_Users b = DAL.BD_Users.GetUserInfoByMobile(LoginID);
                    //判断新用户的标准就是没注册过或者没下过单，下过单取消了也不行。
                    if (b != null && !String.IsNullOrEmpty(b.Mobile) && DAL.Orders.ExistOrder(b.ID) != null)
                    {
                        url = "";
                        result = "本活动仅限新用户参与！";
                        return AjaxResult(url, result);
                    }
                }
                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = "";
                user.LoginID = LoginID;
                user.Mobile = LoginID;
                user.UserPwd = AliceDAL.Common.CreateRandomValue(6, true);
                string code = AliceDAL.Common.CreateRandomValue(4, true);
                user.Data10 = code;
                string con = "【小熊洗车】您的手机验证码为：" + code + "。本信息请勿回复，如需帮助请拨打客服电话：0318-8888235，祝您洗车愉快。";
                MessageManger.SendMsg(LoginID, con);
                DAL.BD_Users.EditCode(user);
                url = "success";
                result = "验证码发送成功";
                return AjaxResult(url, result);
            }
            catch
            {
                url = "error";
                result = "系统错误，请稍候再试！";
                return AjaxResult(url, result);
            }
        }
        public ActionResult ActionDetailsResult(int id)
        {
            lock (_locker)
            {
                string LoginID = AliceDAL.Common.GetFormString("loginID");
                string password = AliceDAL.Common.GetFormString("password");
                string result, url = "";
                if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
                {
                    url = "";
                    result = "手机号码格式不正确！";
                    return AjaxResult(url, result);
                }
                else if (String.IsNullOrWhiteSpace(password))
                {
                    url = "";
                    result = "请输入验证码！";
                    return AjaxResult(url, result);
                }
                BD_Users admin = new DAL.BD_Users().LoginCode(LoginID, password, out result);
                if (null != admin)
                {
                    BD_BusiAction bb = DAL.BD_BusiAction.GetModel(id);
                    if (bb == null || bb.ActionState == 30)
                    {
                        url = "";
                        result = "活动不存在";
                        return AjaxResult(url, result);
                    }
                    if (bb.IsNewUser == 10 && (DAL.Orders.ExistOrder(admin.ID) != null))
                    {
                        url = "";
                        result = "本活动仅限新用户参与！";
                        return AjaxResult(url, result);
                    }
                    List<BD_BusiAction_Item> list = DAL.BD_BusiAction.GetItemListByActionID(id);
                    bool r = false;
                    if (list != null && list.Count > 0)
                    {
                        foreach (BD_BusiAction_Item item in list)
                        {
                            int count = DAL.PageModel.DataCount("[Coupons]", String.Format("[Uid]={0} and [TypeID]={1}", admin.ID, item.ItemID));
                            if (count <= 0)
                            {
                                Coupons cou = new Coupons()
                                {
                                    CouponState = (int)CouponState.Submitted,
                                    OriginalPrice = item.ItemPrice,
                                    Price = item.ItemPrice,
                                    TypeID = item.ItemID,
                                    Uid = admin.ID,
                                    Count = item.Count,
                                    TDate = DateTime.Now.AddDays(item.Period)
                                };
                                for (int i = 0; i < cou.Count; i++)
                                {
                                    DAL.Coupons.CreateCoupons(cou);
                                }
                                r = true;
                            }
                        }
                        if (admin.Data9 == "1")
                        {
                            DAL.BD_Users.EditPreUser(admin.ID, "");//更新预备用户为普通用户
                        }
                        if (r)
                        {
                            url = "";
                            result = "领取成功！";
                            if (!String.IsNullOrEmpty(bb.SMSContent) && bb.SMSContent.Length > 20)
                            {
                                MessageManger.PostMsg(LoginID, bb.SMSContent);
                            }
                        }
                        else
                        {
                            url = "";
                            result = "您已经成功领取，请进入小熊公众号使用！";
                        }
                        return AjaxResult(url, result);
                    }
                    else
                    {
                        url = "";
                        result = "活动不存在";
                        return AjaxResult(url, result);
                    }
                }
                return AjaxResult(url, result);
            }
        }
        public ActionResult ActionDetailsResultCarInfo(int id)
        {
            lock (_locker)
            {
                string LoginID = AliceDAL.Common.GetFormString("loginID");
                string password = AliceDAL.Common.GetFormString("password");
                string plate = AliceDAL.Common.GetFormString("plate");
                string brand = AliceDAL.Common.GetFormString("brand");
                string username = AliceDAL.Common.GetFormString("username");
                string result, url = "";
                if (String.IsNullOrWhiteSpace(username))
                {
                    url = "";
                    result = "请输入称呼！";
                    return AjaxResult(url, result);
                }
                else if (!AliceDAL.ValidateHelper.IsMobile(LoginID))
                {
                    url = "";
                    result = "手机号码格式不正确！";
                    return AjaxResult(url, result);
                }
                else if (String.IsNullOrWhiteSpace(password))
                {
                    url = "";
                    result = "请输入验证码！";
                    return AjaxResult(url, result);
                }
                else if (String.IsNullOrWhiteSpace(plate))
                {
                    url = "";
                    result = "请输入车牌号！";
                    return AjaxResult(url, result);
                }
                else if (String.IsNullOrWhiteSpace(brand))
                {
                    url = "";
                    result = "请输入车型！";
                    return AjaxResult(url, result);
                }
                else if (!AliceDAL.ValidateHelper.IsCarNo(plate))
                {
                    url = "";
                    result = "车牌号不正确！";
                    return AjaxResult(url, result);
                }
                BD_Users admin = new DAL.BD_Users().LoginCode(LoginID, password, out result);
                if (null != admin)
                {
                    UserCar uc = new UserCar()
                    {
                        BrandID = -1,
                        BrandShow = brand,
                        Color = "",
                        ModelID = -1,
                        Plate = plate,
                        UserID = admin.ID,
                        UserName = username
                    };
                    uc.Data1 = uc.Data2 = uc.Data3 = uc.Data4 = uc.Data5 = "";
                    DAL.UserCar.Add(uc);
                    BD_BusiAction bb = DAL.BD_BusiAction.GetModel(id);
                    if (bb == null || bb.ActionState == 30)
                    {
                        url = "";
                        result = "活动不存在";
                        return AjaxResult(url, result);
                    }
                    if (bb.IsNewUser == 10 && (DAL.Orders.ExistOrder(admin.ID) != null))
                    {
                        url = "";
                        result = "本活动仅限新用户参与！";
                        return AjaxResult(url, result);
                    }
                    List<BD_BusiAction_Item> list = DAL.BD_BusiAction.GetItemListByActionID(id);
                    bool r = false;
                    if (list != null && list.Count > 0)
                    {
                        foreach (BD_BusiAction_Item item in list)
                        {
                            int count = DAL.PageModel.DataCount("[Coupons]", String.Format("[Uid]={0} and [TypeID]={1}", admin.ID, item.ItemID));
                            if (count <= 0)
                            {
                                Coupons cou = new Coupons()
                                {
                                    CouponState = (int)CouponState.Submitted,
                                    OriginalPrice = item.ItemPrice,
                                    Price = item.ItemPrice,
                                    TypeID = item.ItemID,
                                    Uid = admin.ID,
                                    Count = item.Count,
                                    TDate = DateTime.Now.AddDays(item.Period)
                                };
                                for (int i = 0; i < cou.Count; i++)
                                {
                                    DAL.Coupons.CreateCoupons(cou);
                                }
                                r = true;
                            }
                        }
                        if (admin.Data9 == "1")
                        {
                            DAL.BD_Users.EditPreUser(admin.ID, "");//更新预备用户为普通用户
                        }
                        if (r)
                        {
                            url = "";
                            result = "领取成功！";
                            if (!String.IsNullOrEmpty(bb.SMSContent) && bb.SMSContent.Length > 20)
                            {
                                MessageManger.PostMsg(LoginID, bb.SMSContent);
                            }
                        }
                        else
                        {
                            url = "";
                            result = "您已经成功领取，请进入小熊公众号使用！";
                        }
                        return AjaxResult(url, result);
                    }
                    else
                    {
                        url = "";
                        result = "活动不存在";
                        return AjaxResult(url, result);
                    }
                }
                return AjaxResult(url, result);
            }
        }
        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { url = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
