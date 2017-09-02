using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Text;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class ToolsController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            string loginid = AliceDAL.UrlParam.GetStringValue("loginid");
            string password = AliceDAL.UrlParam.GetStringValue("password");

            BD_Admin admin = new DAL.BD_Admin().Login(loginid, password);

            if (admin == null) admin = GetUser();

            if (admin == null || admin.ID <= 0)
            {
                filterContext.Result = HttpNotFound();
                return;
            }
        }
        public ActionResult Upload()
        {
            string opeartion = AliceDAL.UrlParam.GetStringValue("operation");
            switch (opeartion)
            {
                case "img":
                    HttpPostedFileBase file = Request.Files[0];
                    string result = AliceDAL.Common.SaveImg(file);
                    return Content(result);

                case "ueconfig":
                    StringBuilder imageAllowFiles = new StringBuilder("[");
                    foreach (string imgType in new string[] { ".jpeg", ".jpg", ".png", ".bmp", ".gif", ".flash", "JPG" })
                    {
                        imageAllowFiles.AppendFormat("\"{0}\",", imgType);
                    }
                    if (imageAllowFiles.Length > 1)
                        imageAllowFiles.Remove(imageAllowFiles.Length - 1, 1);
                    imageAllowFiles.Append("]");

                    string imageUrlPrefix = "/uploadfiles/";

                    return Content(string.Format("{0}\"imageActionName\": \"uploadimage\", \"imageFieldName\": \"upfile\", \"imageMaxSize\": {1},\"imageAllowFiles\": {2}, \"imageCompressEnable\": true, \"imageCompressBorder\": 1600, \"imageInsertAlign\": \"none\", \"imageUrlPrefix\": \"{3}\", \"imagePathFormat\": \"\", \"imageManagerActionName\": \"listimage\",\"imageManagerListPath\": \"upload/image\",\"imageManagerListSize\": 20, \"imageManagerUrlPrefix\": \"/ueditor/net/\",\"imageManagerInsertAlign\": \"none\", \"imageManagerAllowFiles\": [\".png\", \".jpg\", \".jpeg\", \".gif\", \".bmp\"]{4}", "{", 10240000, imageAllowFiles, imageUrlPrefix, "}"));

                case "uploadproducteditorimage":
                    HttpPostedFileBase file1 = Request.Files[0];
                    string result1 = AliceDAL.Common.SaveImg(file1);
                    return Content(string.Format("{2}'url':'uploadfiles/','state':'{1}'{3}", result1, GetUEState(result1), "{", "}"));
                default:
                    return HttpNotFound();
            }
        }
        private BD_Admin GetUser()
        {
            BD_Admin admin = null;
            if (System.Web.HttpContext.Current.Request.Cookies["LoginInfo"] == null) return null;
            admin = new BD_Admin();
            admin.LoginID = AliceDAL.Common.GetCookie("LoginInfo", "LoginID");
            admin.Password = AliceDAL.Common.GetCookie("LoginInfo", "Password");
            admin.ID = int.Parse(AliceDAL.Common.GetCookie("LoginInfo", "ID"));
            admin.NickName = AliceDAL.Common.GetCookie("LoginInfo", "NickName");
            admin.Data1 = AliceDAL.Common.GetCookie("LoginInfo", "Data1");
            admin.Data2 = AliceDAL.Common.GetCookie("LoginInfo", "Data2");
            admin.Data3 = AliceDAL.Common.GetCookie("LoginInfo", "Data3");
            admin.Data4 = AliceDAL.Common.GetCookie("LoginInfo", "Data4");
            admin.Data5 = AliceDAL.Common.GetCookie("LoginInfo", "Data5");
            admin.Data6 = AliceDAL.Common.GetCookie("LoginInfo", "Data6");
            admin.Data7 = AliceDAL.Common.GetCookie("LoginInfo", "Data7");
            admin.Data8 = AliceDAL.Common.GetCookie("LoginInfo", "Data8");
            admin.Data9 = AliceDAL.Common.GetCookie("LoginInfo", "Data9");
            admin.Data10 = AliceDAL.Common.GetCookie("LoginInfo", "Data10");
            return admin;
        }
        private string GetUEState(string result)
        {
            if (result == "-1")
            {
                return "上传图片不能为空";
            }
            else if (result == "-2")
            {
                return "不允许的图片类型";
            }
            else if (result == "-3")
            {
                return "图片大小超出网站限制";
            }
            else
            {
                return "SUCCESS";
            }
        }
    }
}
