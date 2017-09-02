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
using Web.Areas.app.Alipay;

namespace Web.Areas.app.Controllers
{
    public class AlipayController : AppAreaBaseController
    {
        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            MessageModel mm = new MessageModel();
            int oid = AliceDAL.UrlParam.GetIntValue("oid");
            AdminOrders orderInfo = DAL.AdminOrders.GetOrderInfoByID(oid);
            if (orderInfo == null)
            {
                mm.code = "0";
                mm.msg = "订单不存在";
                mm.data = "";
            };
            //支付类型，必填，不能修改
            string paymentType = "1";
            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notifyUrl = string.Format("http://{0}/app/alipay/notify_url", AliceDAL.IniHelper.GetValue("WebInfo", "Url"));

            //收款支付宝帐户
            string sellerEmail = Config.Seller_id;
            //合作者身份ID
            string partner = Config.Partner;
            //交易安全检验码
            string key = Config.Private_key;

            //商户订单号
            string outTradeNo = oid + AliceDAL.Common.CreateRandomValue(10, false);
            //订单名称
            string subject = "小熊洗车";
            //付款金额
            string totalFee = orderInfo.OrderAmount.ToString();
            //订单描述
            string body = "";

            Encoding e = Encoding.GetEncoding(Config.Input_charset);

            //把请求参数打包成数组
            SortedDictionary<string, string> parms = new SortedDictionary<string, string>();
            parms.Add("partner", partner);
            parms.Add("seller_id", sellerEmail);
            parms.Add("out_trade_no", outTradeNo);
            parms.Add("subject", subject);
            parms.Add("body", body);
            parms.Add("total_fee", totalFee);
            parms.Add("notify_url", notifyUrl);
            parms.Add("service", "mobile.securitypay.pay");
            parms.Add("payment_type", paymentType);
            parms.Add("_input_charset", Config.Input_charset);
            parms.Add("it_b_pay", "30m");
            parms.Add("show_url", "m.alipay.com");

            string sign = RSAFromPkcs8.sign(Core.CreateLinkString(Core.FilterPara(parms)), Config.Private_key, Config.Input_charset);
            parms.Add("sign", HttpUtility.UrlEncode(sign, e));
            parms.Add("sign_type", Config.Sign_type);

            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in parms)
            {
                dicArray.Add(temp.Key, temp.Value);
            }

            string content = Core.CreateLinkString(dicArray); 
            mm.data = content;
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult notify_url()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult && (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = Request.Form["out_trade_no"];//商户订单号
                    string tradeSN = Request.Form["trade_no"];//支付宝交易号
                    decimal tradeMoney = AliceDAL.DataType.ConvertToDecimal(Request.Form["total_fee"]);//交易金额
                    DateTime tradeTime = AliceDAL.DataType.ConvertToDateTime(Request.Form["gmt_payment"]);//交易时间
                    string oidList = AliceDAL.Common.SubString(out_trade_no, out_trade_no.Length - 10);//订单id列表
                    string realOidList = oidList;


                    AdminOrders orderInfo = DAL.AdminOrders.GetOrderInfoByID(AliceDAL.DataType.ConvertToInt(realOidList));
                    decimal allSurplusMoney = 0M;
                    allSurplusMoney += orderInfo.OrderAmount;

                    if (orderInfo != null && allSurplusMoney <= tradeMoney)
                    {
                        if (orderInfo.OrderAmount > 0 && orderInfo.OrderState == (int)UserOrderState.WaitPaying)
                        {
                            DAL.AdminOrders.PayOrder(orderInfo.ID, OrderState.Confirming, tradeSN, DateTime.Now);
                            ChargeType ct = DAL.ChargeType.GetModel(orderInfo.TypeID);
                            DAL.BD_Users.EditUserWalletByUserID(orderInfo.Uid, ct.Price, ct.GivePrice);
                            DAL.OrderActions.Add(new OrderActions()
                            {
                                Oid = orderInfo.ID,
                                Uid = orderInfo.Uid,
                                RealName = "本人",
                                ActionType = "支付",
                                ActionDes = "你使用支付宝支付订单成功，支付宝交易号为:" + tradeSN
                            });
                        }

                    }
                    return Content("success");
                }
                else//验证失败
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
