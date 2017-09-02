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
using Web.Areas.app.WeiXinPay;
using System.Xml;

namespace Web.Areas.app.Controllers
{
    public class WeiXinPayController : AppAreaBaseController
    {
        public ActionResult Pay()
        {
            int oid = AliceDAL.UrlParam.GetIntValue("oid");
            AdminOrders orderInfo = DAL.AdminOrders.GetOrderInfoByID(oid);
            if (orderInfo == null) return Content("");

            #region 支付操作============================

            #region 基本参数===========================

            //商户订单号
            string outTradeNo = oid.ToString() + AliceDAL.Common.CreateRandomValue(10, false);
            //订单名称
            string subject = "小熊洗车";
            //付款金额
            string totalFee = (orderInfo.OrderAmount * 100).ToString("0").Replace(".", "");

            //时间戳 
            string timeStamp = TenpayUtil.getTimestamp();
            //随机字符串 
            string nonceStr = TenpayUtil.getNoncestr();
            //服务器异步通知页面路径
            string notifyUrl = string.Format("http://{0}/app/weixinpay/notify", AliceDAL.IniHelper.GetValue("WebInfo", "Url"));

            //LogUtil.WriteLog("totalFee" + totalFee);

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(System.Web.HttpContext.Current);
            //初始化
            packageReqHandler.init();

            //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置

            packageReqHandler.setParameter("appid", PayConfig.AppId);
            packageReqHandler.setParameter("body", subject); //商品信息 127字符
            packageReqHandler.setParameter("mch_id", PayConfig.MchId);
            packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
            packageReqHandler.setParameter("notify_url", notifyUrl);
            packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
            packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            packageReqHandler.setParameter("total_fee", totalFee); //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.setParameter("trade_type", "APP");
            //if (!string.IsNullOrEmpty(this.Attach))
            //    packageReqHandler.setParameter("attach", this.Attach);//自定义参数 127字符

            #endregion

            #region sign===============================

            string sign = packageReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
            //LogUtil.WriteLog("WeiPay 页面  sign：" + sign);

            #endregion

            #region 获取package包======================

            packageReqHandler.setParameter("sign", sign);

            string data = packageReqHandler.parseXML();
            //LogUtil.WriteLog("WeiPay 页面  package（XML）：" + data);

            string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
            //LogUtil.WriteLog("WeiPay 页面  package（Back_XML）：" + prepayXml);

            //获取预支付ID
            string prepayId = "";
            string package = "";
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(prepayXml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 7)
            {
                prepayId = xnl[7].InnerText;
                package = string.Format("prepay_id={0}", prepayId);
                //LogUtil.WriteLog("WeiPay 页面  package：" + package);
            }
            #endregion

            #region 设置支付参数 输出页面  该部分参数请勿随意修改 ==============

            RequestHandler paySignReqHandler = new RequestHandler(System.Web.HttpContext.Current);
            paySignReqHandler.setParameter("appid", PayConfig.AppId);
            paySignReqHandler.setParameter("noncestr", nonceStr.ToLower());
            paySignReqHandler.setParameter("package", "Sign=WXPay");
            paySignReqHandler.setParameter("partnerid", PayConfig.MchId);
            paySignReqHandler.setParameter("prepayid", prepayId);
            paySignReqHandler.setParameter("timestamp", timeStamp);
            string paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);


            //LogUtil.WriteLog("WeiPay 页面  paySign：" + paySign);

            #endregion

            #endregion

            return Content(string.Format("{0}\"appid\":\"{1}\",\"partnerid\":\"{2}\",\"prepayid\":\"{3}\",\"package\":\"{4}\",\"noncestr\":\"{5}\",\"timestamp\":\"{6}\",\"sign\":\"{7}\"{8}",
                                             "{", PayConfig.AppId,
                                             PayConfig.MchId,
                                             prepayId,
                                             "Sign=WXPay",
                                             nonceStr.ToLower(),
                                             timeStamp,
                                             paySign,
                                             "}"));
        }
        /// <summary>
        /// 返回调用
        /// </summary>
        public ActionResult Notify()
        {
            //创建ResponseHandler实例
            ResponseHandler resHandler = new ResponseHandler(System.Web.HttpContext.Current);
            resHandler.setKey(PayConfig.AppKey);
            //判断签名
            try
            {
                string error = "";
                if (resHandler.isWXsign(out error))
                {
                    #region 协议参数=====================================

                    //--------------协议参数--------------------------------------------------------
                    //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查
                    string return_code = resHandler.getParameter("return_code");
                    //返回信息，如非空，为错误原因签名失败参数格式校验错误
                    string return_msg = resHandler.getParameter("return_msg");
                    //微信分配的公众账号 ID
                    string appid = resHandler.getParameter("appid");

                    //以下字段在 return_code 为 SUCCESS 的时候有返回--------------------------------
                    //微信支付分配的商户号
                    string mch_id = resHandler.getParameter("mch_id");
                    //微信支付分配的终端设备号
                    string device_info = resHandler.getParameter("device_info");
                    //微信分配的公众账号 ID
                    string nonce_str = resHandler.getParameter("nonce_str");
                    //业务结果 SUCCESS/FAIL
                    string result_code = resHandler.getParameter("result_code");
                    //错误代码 
                    string err_code = resHandler.getParameter("err_code");
                    //结果信息描述
                    string err_code_des = resHandler.getParameter("err_code_des");

                    //以下字段在 return_code 和 result_code 都为 SUCCESS 的时候有返回---------------
                    //-------------业务参数---------------------------------------------------------
                    //用户在商户 appid 下的唯一标识
                    string openid = resHandler.getParameter("openid");
                    //用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
                    string is_subscribe = resHandler.getParameter("is_subscribe");
                    //JSAPI、NATIVE、MICROPAY、APP
                    string trade_type = resHandler.getParameter("trade_type");
                    //银行类型，采用字符串类型的银行标识
                    string bank_type = resHandler.getParameter("bank_type");
                    //订单总金额，单位为分
                    string total_fee = resHandler.getParameter("total_fee");
                    //货币类型，符合 ISO 4217 标准的三位字母代码，默认人民币：CNY
                    string fee_type = resHandler.getParameter("fee_type");
                    //微信支付订单号
                    string transaction_id = resHandler.getParameter("transaction_id");
                    //商户系统的订单号，与请求一致。
                    string out_trade_no = resHandler.getParameter("out_trade_no");
                    //商家数据包，原样返回
                    string attach = resHandler.getParameter("attach");
                    //支 付 完 成 时 间 ， 格 式 为yyyyMMddhhmmss，如 2009 年12 月27日 9点 10分 10 秒表示为 20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器
                    string time_end = resHandler.getParameter("time_end");

                    #endregion

                    //支付成功
                    if (!out_trade_no.Equals("") && return_code.Equals("SUCCESS") && result_code.Equals("SUCCESS"))
                    {
                        //LogUtil.WriteLog("Notify 页面  支付成功，支付信息：商家订单号：" + out_trade_no + "、支付金额(分)：" + total_fee + "、自定义参数：" + attach);

                        /**
                         *  这里输入用户逻辑操作，比如更新订单的支付状态
                         * 
                         * **/

                        string oidList = AliceDAL.Common.SubString(out_trade_no, out_trade_no.Length - 10);//订单id列表

                        string realOidList = oidList;
                        AdminOrders orderInfo = DAL.AdminOrders.GetOrderInfoByID(AliceDAL.DataType.ConvertToInt(realOidList));
                        decimal allSurplusMoney = 0M;

                        allSurplusMoney += orderInfo.OrderAmount;

                        if (orderInfo != null)
                        {
                            if (orderInfo.OrderAmount > 0 && orderInfo.OrderState == (int)OrderState.WaitPaying)
                            {
                                DAL.AdminOrders.PayOrder(orderInfo.ID, OrderState.Confirming, transaction_id, DateTime.Now);

                                if (orderInfo.TypeID == 999)
                                {
                                    DAL.BD_Users.EditUserWalletByUserID(orderInfo.Uid, orderInfo.OrderAmount, 0);
                                }
                                else
                                {
                                    ChargeType ct = DAL.ChargeType.GetModel(orderInfo.TypeID);
                                    DAL.BD_Users.EditUserWalletByUserID(orderInfo.Uid, ct.Price, ct.GivePrice);
                                }
                                DAL.BD_Consumer.Add(new BD_Consumer()
                                {
                                    CDate = DateTime.Now,
                                    Money = orderInfo.OrderAmount,
                                    OrderID = orderInfo.ID,//AdminOrders
                                    Remark = "微信充值",
                                    UserID = orderInfo.Uid
                                });
                                DAL.OrderActions.Add(new OrderActions()
                                {
                                    Oid = orderInfo.ID,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = "支付",
                                    ActionDes = "你使用微信支付充值成功，微信交易号为:" + transaction_id
                                });
                            }

                        }
                        //LogUtil.WriteLog("============ 单次支付结束 ===============");
                        return Content("success");
                    }
                    else
                    {
                        //LogUtil.WriteLog("Notify 页面  支付失败，支付信息   total_fee= " + total_fee + "、err_code_des=" + err_code_des + "、result_code=" + result_code);
                        return new EmptyResult();
                    }
                }
                else
                {
                    //LogUtil.WriteLog("Notify 页面  isWXsign= false ，错误信息：" + error);
                    return new EmptyResult();
                }
            }
            catch (Exception ee)
            {
                //LogUtil.WriteLog("Notify 页面  发送异常错误：" + ee.Message);
                return new EmptyResult();
            }
        }
    }
}