using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class AppBaseController : AppAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            WorkContext.IP = AliceDAL.Common.GetIP();
            BD_Users admin = GetUser();
            if (admin != null)
            {
                WorkContext.ID = admin.ID;
                WorkContext.CDate = admin.CDate;
                WorkContext.Data1 = admin.Data1;
                WorkContext.Data2 = admin.Data2;
                WorkContext.Data3 = admin.Data3;
                WorkContext.Data4 = admin.Data4;
                WorkContext.Data5 = admin.Data5;
                WorkContext.Data6 = admin.Data6;
                WorkContext.Data7 = admin.Data7;
                WorkContext.Data8 = admin.Data8;
                WorkContext.Data9 = admin.Data9;
                WorkContext.Data10 = admin.Data10;
                WorkContext.LoginID = admin.LoginID;
                WorkContext.Mobile = admin.Mobile;
                WorkContext.UserPwd = AliceDAL.SecureHelper.MD5(admin.UserPwd + admin.ID.ToString() + admin.Data10);
                BD_Wallet bw = DAL.BD_Users.GetUserWalletByUserID(WorkContext.ID);
                WorkContext.Wallet = bw;
                WorkContext.WalletMoney = bw.Money1 + bw.Money2;
                WorkContext.CouponCount = DAL.BD_Users.GetUserCouponCountByUserID(WorkContext.ID);
                WorkContext.PageSize = 10;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            if (WorkContext.ID <= 0 && String.IsNullOrEmpty(WorkContext.Mobile))
            {
                filterContext.Result = MessageModel("2", "您还未登陆");
                return;
            }
        }
        protected ActionResult MessageModel(string code, string content)
        {
            return Content(string.Format("{0}\"code\":\"{1}\",\"msg\":\"{2}\",\"data\":null{3}", "{", code, content, "}"));
        }
        private BD_Users GetUser()
        {
            BD_Users model = null;
            int uid = AliceDAL.Common.GetFormInt("uid");
            if (uid < 1)
            {
                return model;
            }
            else
            {
                string encryptPwd = AliceDAL.Common.GetFormString("encryptpassword");
                if (String.IsNullOrWhiteSpace(encryptPwd))
                {
                    return model;
                }
                else
                {
                    model = DAL.BD_Users.GetUserByApp(uid, encryptPwd);
                }
            }
            return model;
        }
    }
}