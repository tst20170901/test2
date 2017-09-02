using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using Models;
using Web.Areas.app.Models;
using AliceDAL;
using Newtonsoft.Json;
using System.Data;

namespace Web.Areas.app.Controllers
{
    public class UsersController : AppBaseController
    {
        public ActionResult UserInfo()
        {
            MessageModel mm = new MessageModel();
            UserModel um = new UserModel();
            um.CDate = WorkContext.CDate;
            um.EncryptPassword = WorkContext.UserPwd;
            um.LoginID = WorkContext.LoginID;
            um.Mobile = WorkContext.Mobile;
            um.Uid = WorkContext.ID;
            um.UserName = WorkContext.Data1;//作为用户姓名
            um.WalletMoney = WorkContext.WalletMoney;
            um.CouponCount = WorkContext.CouponCount;
            mm.data = um;
            return Content(JsonConvert.SerializeObject(mm));
        }
        [ValidateInput(false)]
        public ActionResult EditUserInfo()
        {
            MessageModel mm = new MessageModel();
            string username = AliceDAL.Common.cutBadStr(AliceDAL.Common.GetFormString("aliceusername")).Trim();
            mm.code = "0";
            if (String.IsNullOrWhiteSpace(username))
            {
                mm.msg = "用户姓名不能为空";
            }
            int result = DAL.BD_Users.EditUserName(WorkContext.ID, username);
            if (result > 0)
            {
                mm.code = "1";
                mm.msg = "success";
            }
            else
            {
                mm.msg = "更新失败，请稍候再试";
            };
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult CouponsList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "fail";

            List<string> business = new List<string>() { /*"平安保险", "民生银行", "工商银行", "中国人寿"*/ };
            List<CouponsModel> list = new List<CouponsModel>();
            //list.Add(new CouponsModel() { Business = "平安保险", EndDate = DateTime.Now.AddYears(1), ID = 1, Title = "10元抵用券", Price = 10 });
            //list.Add(new CouponsModel() { Business = "平安保险", EndDate = DateTime.Now.AddYears(1), ID = 2, Title = "20元抵用券", Price = 20 });
            //list.Add(new CouponsModel() { Business = "平安保险", EndDate = DateTime.Now.AddYears(1), ID = 3, Title = "30元抵用券", Price = 30 });
            //list.Add(new CouponsModel() { Business = "平安保险", EndDate = DateTime.Now.AddYears(1), ID = 4, Title = "40元抵用券", Price = 40 });

            //list.Add(new CouponsModel() { Business = "民生银行", EndDate = DateTime.Now.AddYears(1), ID = 5, Title = "5元抵用券", Price = 5 });
            //list.Add(new CouponsModel() { Business = "民生银行", EndDate = DateTime.Now.AddYears(1), ID = 6, Title = "10元抵用券", Price = 10 });

            //list.Add(new CouponsModel() { Business = "中国人寿", EndDate = DateTime.Now.AddYears(1), ID = 7, Title = "10元抵用券", Price = 10 });

            //list.Add(new CouponsModel() { Business = "中国人寿", EndDate = DateTime.Now.AddYears(1), ID = 8, Title = "10元抵用券", Price = 10 });

            CouponsListModel clm = new CouponsListModel();
            clm.Business = business;
            clm.CouponsList = list;
            if (list != null && business != null)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = clm;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult ConsumerList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无消费记录";
            List<BD_Consumer> list = DAL.BD_Consumer.GetListByUserID(WorkContext.ID);
            if (list != null && list.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult AdminOrdersList()
        {
            MessageModel mm = new MessageModel();
            mm.code = "0";
            mm.msg = "暂无充值记录";
            List<AdminOrders> list = DAL.AdminOrders.GetListByUserID(WorkContext.ID);
            if (list != null && list.Count > 0)
            {
                mm.code = "1";
                mm.msg = "success";
                mm.data = list;
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
    }
}
