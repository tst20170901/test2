using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;
using AliceDAL;
using System.Data;

namespace Web.Units
{
    public class AdminBaseController : AreaBaseController
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            BD_Admin admin = GetUser();
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
                WorkContext.NickName = admin.NickName;
                WorkContext.Password = admin.Password;
                WorkContext.ID = admin.ID;
                WorkContext.RoleID = admin.RoleID;
                WorkContext.ActionsAllot = DAL.BD_BearActionsAllot.Model(admin.RoleID);
                WorkContext.BranchID = admin.BranchID;
                WorkContext.Branch = DAL.BD_Branch.GetModel(admin.BranchID);
                WorkContext.PageSize = 10;
                //设置当前控制器类名
                WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
                //设置当前动作方法名
                WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
                WorkContext.PageKey = String.Format("{0}{1}", WorkContext.Controller, WorkContext.Action);
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
                    filterContext.Result = new RedirectResult("/AliceChopper/AliceChopper/Login/");
                return;
            }
            //如果当前用户不是管理员
            if (WorkContext.RoleID < 1)
            {
                if (!WorkContext.IsHttpAjax)
                {
                    filterContext.Result = new RedirectResult("/");
                    return;
                }
            }

            //如果当前用户不属于公司
            if (WorkContext.BranchID < 1 || WorkContext.Branch == null)
            {
                if (WorkContext.IsHttpAjax)
                    filterContext.Result = AjaxResult(new { state = "404", msg = "您访问的网址不存在" });
                else
                    filterContext.Result = new RedirectResult("/");
                return;
            }

            //判断当前用户是否有访问当前页面的权限
            if (!DAL.BD_BearActionsAllot.CheckAuthority(WorkContext.RoleID, WorkContext.PageKey))
            {
                if (!WorkContext.IsHttpAjax)
                {
                    filterContext.Result = PromptView("您没有当前操作的权限！");
                    return;
                }
            }
        }
        private BD_Admin GetUser()
        {
            BD_Admin admin = null;
            if (System.Web.HttpContext.Current.Request.Cookies["LoginInfo"] == null) return null;
            admin = new BD_Admin();
            HttpCookie c = Request.Cookies["LoginInfo"];
            admin.LoginID = c.Values["LoginID"];
            admin.Password = c.Values["Password"];
            admin.ID = int.Parse(c.Values["ID"]);
            admin.RoleID = int.Parse(c.Values["RoleID"]);
            admin.BranchID = int.Parse(c.Values["BranchID"]);
            admin.NickName = c.Values["NickName"];
            ////admin.Data1 = c.Values["Data1"];//mg del
            ////admin.Data2 = c.Values["Data2"];
            ////admin.Data3 = c.Values["Data3"];
            ////admin.Data4 = c.Values["Data4"];
            ////admin.Data5 = c.Values["Data5"];
            ////admin.Data6 = c.Values["Data6"];
            ////admin.Data7 = c.Values["Data7"];
            ////admin.Data8 = c.Values["Data8"];
            ////admin.Data9 = c.Values["Data9"];
            ////admin.Data10 = c.Values["Data10"];

            admin.Data1 = "";
            admin.Data2 = "";
            admin.Data3 = "";
            admin.Data4 = "";
            admin.Data5 = "";
            admin.Data6 = "";
            admin.Data7 = "";
            admin.Data8 = "";
            admin.Data9 = "";
            admin.Data10 = "";

            return admin;
        }
        public void LoadBranch()
        {
            List<BD_Branch> list = CacheManager.Get(CacheKeys.BRANCHLIST) as List<BD_Branch>;
            if (list == null)
            {
                list = new List<BD_Branch>();
                DataTable dt = DAL.PageModel.DataKeysList("[BD_Branch]", "[ID],[Title]", "[BranchState]=10", "ID", 0);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        BD_Branch bb = new BD_Branch()
                        {
                            ID = DataType.ConvertToInt(item["ID"].ToString()),
                            Title = item["Title"].ToString()
                        };
                        list.Add(bb);
                    }

                }
                CacheManager.Insert(CacheKeys.BRANCHLIST, list);
            }
            ViewData["branchlist"] = list;
            ViewData["businessTypeList"] = DAL.Business_Types.List();
        }
    }
}