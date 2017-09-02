using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Models;
using Web.Areas.app.Models;
using AliceDAL;
using Newtonsoft.Json;
using System.Data;
using System.Collections.Specialized;

namespace Web.Areas.app.Controllers
{
    public class AccountController : AppAreaBaseController
    {
        public ActionResult Login()
        {
            MessageModel m = new MessageModel();
            m.code = "0";
            string accountName = AliceDAL.Common.GetFormString("aliceusername").Trim();
            string password = AliceDAL.Common.GetFormString("alicecode").Trim();
            if (string.IsNullOrWhiteSpace(accountName))
            {
                m.msg = "手机号码不能为空";
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                m.msg = "验证码不能为空";
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(accountName))
            {
                m.msg = "手机号码格式不正确";
            }
            else
            {
                string result;
                BD_Users admin = new DAL.BD_Users().LoginCode(accountName, password, out result);
                if (admin != null)
                {
                    admin.UserPwd = SecureHelper.MD5(admin.UserPwd + admin.ID.ToString() + admin.Data10);
                    m.msg = "success";
                    m.code = "1";
                    Models.UserModel um = new UserModel();
                    um.CDate = admin.CDate;
                    um.EncryptPassword = admin.UserPwd;
                    um.LoginID = admin.LoginID;
                    um.Mobile = admin.Mobile;
                    um.Uid = admin.ID;
                    um.UserName = admin.Data1;//作为用户姓名
                    um.CouponCount = DAL.BD_Users.GetUserCouponCountByUserID(admin.ID);
                    BD_Wallet bw = DAL.BD_Users.GetUserWalletByUserID(admin.ID);
                    um.WalletMoney = bw.Money1 + bw.Money2;
                    m.data = um;
                }
                else
                {
                    m.msg = result;
                }
            }
            return Content(JsonConvert.SerializeObject(m));
        }
        public ActionResult SendMsg()
        {
            MessageModel m = new MessageModel();
            m.code = "0";
            string accountName = AliceDAL.Common.GetFormString("aliceusername").Trim();
            if (String.IsNullOrWhiteSpace(accountName))
            {
                m.msg = "手机号码不能为空！";
                return Content(JsonConvert.SerializeObject(m));
            }
            else if (!AliceDAL.ValidateHelper.IsMobile(accountName))
            {
                m.msg = "手机号码格式不正确！";
                return Content(JsonConvert.SerializeObject(m));
            }

            try
            {
                BD_Users user = new BD_Users();
                user.Data1 = user.Data2 = user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = "";

                user.LoginID = accountName;
                user.Mobile = accountName;
                user.UserPwd = AliceDAL.SecureHelper.MD5("123456");


                string code = AliceDAL.Common.CreateRandomValue(4, true);
                user.Data10 = code;
                string con = "【小熊洗车】您的手机验证码为：" + code + "。本信息请勿回复，如需帮助请拨打客服电话：0318-8888235，祝您洗车愉快。";
                MessageManger.PostMsg(accountName, con);
                DAL.BD_Users.EditCode(user);
                m.code = "1";
                m.msg = "验证码发送成功";
                return Content(JsonConvert.SerializeObject(m));
            }
            catch (Exception ex)
            {
                m.msg = "系统错误，请稍候再试！";
                return Content(JsonConvert.SerializeObject(m));
            }
        }
        /// <summary>
        /// 普通字符串
        /// </summary>
        /// <returns></returns>
        public ActionResult WashMsg()
        {
            MessageModel m = new MessageModel();
            m.data = "1.车辆、车窗都已关闭\n2.在预约洗完时间前请勿移动车辆，如果移动需向服务人员说明情况\n3.车辆四周需预留足够的洗车空间";
            return Content(JsonConvert.SerializeObject(m));
        }
        /// <summary>
        /// 常见问题
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowArticle(int id)
        {
            DataTable dt = DAL.PageModel.Table_Model("AB_SinglePage", "ID=" + id.ToString());
            if (dt != null)
            {
                AB_SinglePage model = new AB_SinglePage();
                model.Body = dt.Rows[0]["Body"].ToString();
                model.Description = dt.Rows[0]["Description"].ToString();
                model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                model.Img = dt.Rows[0]["Img"].ToString();
                model.KeyWords = dt.Rows[0]["KeyWords"].ToString();
                model.Title = dt.Rows[0]["Title"].ToString();
                model.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
                SeoInit.InitHead(model.TitleWeb, model.KeyWords, model.Description, this);
                return View(model);
            }
            else
            {
                return View("404");
            }
        }
        public ActionResult PreOrder()
        {
            MessageModel mm = new MessageModel();
            string lng = AliceDAL.Common.GetFormString("alicelng");
            string lat = AliceDAL.Common.GetFormString("alicelat");
            string msg = "附近没有可用服务";
            mm.code = "0";
            AddressModel m = AMap.Common.GetAddress(lng, lat);
            if (m != null && m.CityCode != "" && m.AdCode != "")
            {
                List<WorkShop> list = DAL.WorkShop.WorkShopForCode(m.CityCode, m.AdCode, (int)WorkShopState.Open);
                if (list != null)
                {
                    AMap.Common.InitDistance(list, lng, lat);
                    WorkShop ws = list.OrderBy(x => x.Distan).First();
                    if (ws != null)
                    {
                        if (ws.Distan <= ws.WorkRadius)
                        {
                            int t = TimeHelper.IsInTime(ws.StartTime, ws.EndTime);
                            if (t == 1)
                            {
                                msg = "预计今日" + DateTime.Now.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            else if (t == 2)
                            {
                                msg = "预计明日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            else
                            {
                                msg = "预计今日" + ws.StartTime.AddMinutes(ws.CostTime).AddSeconds(ws.Dur).ToString("HH时mm分") + "完成洗车";
                            }
                            mm.code = "1";
                        }
                        else
                        {
                            msg = "超出服务范围";
                        }
                    }
                }
            }
            mm.msg = msg;
            return Content(JsonConvert.SerializeObject(mm));
        }
    }
}
