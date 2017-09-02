using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
using Web.Areas.Worker.Models;
using AliceDAL;
using Newtonsoft.Json;

namespace Web.Areas.Worker.Controllers
{
    public class AccountController : WorkerAreaBaseController
    {
        public ActionResult Login()
        {
            MessageModel m = new MessageModel();
            m.code = "0";
            string accountName = AliceDAL.Common.GetFormString("aliceusername");
            string password = AliceDAL.Common.GetFormString("alicepassword");
            if (string.IsNullOrWhiteSpace(accountName))
            {
                m.msg = "帐号不能为空";
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                m.msg = "密码不能为空";
            }
            else
            {
                string result;
                WorkShop user = new DAL.WorkShop().Login(accountName, AliceDAL.SecureHelper.MD5(password), out result);
                if (user != null)
                {

                    user.UserPwd = SecureHelper.MD5(user.UserPwd + user.ID.ToString());
                    m.msg = "success";
                    m.code = "1";
                    Models.WorkerModel mm = new WorkerModel();
                    mm.Lat = user.Lat1;
                    mm.Lng = user.Lng1;
                    mm.Mobile = user.Mobile;
                    mm.ReDate = user.ReDate;
                    mm.EncryptPassword = user.UserPwd;
                    mm.Uid = user.ID;
                    mm.UserName = user.Title;
                    mm.WorkState = user.WorkState;
                    m.data = mm;
                }
                else
                {
                    m.msg = result;
                }
            }
            return Content(JsonConvert.SerializeObject(m));
        }
    }
}
