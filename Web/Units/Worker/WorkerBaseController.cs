using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class WorkerBaseController : WorkerAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            WorkContext.IP = AliceDAL.Common.GetIP();

            WorkShop admin = GetUser();
            if (admin != null)
            {
                WorkContext.ID = admin.ID;
                WorkContext.Lat = admin.Lat1;
                WorkContext.Lng = admin.Lng1;
                WorkContext.Mobile = admin.Mobile;
                WorkContext.ReDate = admin.ReDate;
                WorkContext.Title = admin.Title;
                WorkContext.UserPwd = AliceDAL.SecureHelper.MD5(admin.UserPwd + admin.ID.ToString());
                WorkContext.WorkState = admin.WorkState;
                WorkContext.PageSize = 10;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            //if (WorkContext.ID <= 0 && String.IsNullOrEmpty(WorkContext.Mobile))
            //{
            //    filterContext.Result = MessageModel("2", "您还未登陆");
            //    return;
            //}
            filterContext.Result = MessageModel("2", "工人端APP已暂停使用，请使用微信工人端");
            return;
        }
        protected ActionResult MessageModel(string code, string content)
        {
            return Content(string.Format("{0}\"code\":\"{1}\",\"msg\":\"{2}\",\"data\":null{3}", "{", code, content, "}"));
        }
        private WorkShop GetUser()
        {
            WorkShop model = null;
            int uid = AliceDAL.UrlParam.GetIntValue("uid");
            if (uid < 1)
            {
                return model;
            }
            else
            {
                string encryptPwd = AliceDAL.UrlParam.GetStringValue("encryptpassword");
                if (String.IsNullOrWhiteSpace(encryptPwd))
                {
                    return model;
                }
                else
                {
                    model = DAL.WorkShop.GetUserByApp(uid, encryptPwd);
                }
            }
            return model;
        }
    }
}