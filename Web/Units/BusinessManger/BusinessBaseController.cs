using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Web.Units
{
    public class BusinessBaseController : BusinessAreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            BD_Business admin = GetUser();
            if (admin != null)
            {
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
                WorkContext.CDate = admin.CDate;
                WorkContext.BusinessName = admin.BusinessName;
                WorkContext.Password = admin.Password;
                WorkContext.ID = admin.ID;
                WorkContext.Wallet = admin.Wallet;
                WorkContext.BranchID = admin.BranchID;
                WorkContext.SortID = admin.SortID;
                WorkContext.TypeID = admin.TypeID;
                WorkContext.BusinessState = admin.BusinessState;
                WorkContext.PageSize = 10;
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.IsChildAction) return;
            if (WorkContext.ID <= 0 && String.IsNullOrEmpty(WorkContext.LoginID))
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/BusinessManger/FrameSet/Login/");
                return;
            }
        }
        private BD_Business GetUser()
        {
            BD_Business admin = null;
            admin = DAL.BD_Business.GetBusinessByApp(AliceDAL.DataType.ConvertToInt(HttpUtility.UrlDecode(AliceDAL.Common.GetCookie("BusinessUserInfo", "ID"))), AliceDAL.Common.GetCookie("BusinessUserInfo", "Password"));
            return admin;
        }
    }
}