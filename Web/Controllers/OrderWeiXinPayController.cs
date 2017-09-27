using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using Com.Alipay;
using Models;
using System.Collections.Specialized;
using WebChatConfig;
using Newtonsoft.Json;
using System.Xml;
using swiftpass.utils;
using System.Collections;

namespace Web.Controllers
{
    public class OrderWeiXinPayController : Controller
    {
        private ClientResponseHandler resHandler = new ClientResponseHandler();
        private PayHttpClient pay = new PayHttpClient();
        private swiftpass.utils.RequestHandler reqHandler = null;
        private Dictionary<string, string> cfg = new Dictionary<string, string>(1);

        ////public ActionResult PayOld()//mg update
        ////{
        ////    int oid = AliceDAL.UrlParam.GetIntValue("oid");
        ////    Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
        ////    if (orderInfo == null) Redirect("/BearClient");
        ////    string code = Request.QueryString["code"];
        ////    if (string.IsNullOrEmpty(code))
        ////    {
        ////        string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect", PayConfig.AppId, string.Format("http://{0}/OrderWeiXinPay/pay?oid=" + oid.ToString(), AliceDAL.IniHelper.GetValue("WebInfo", "Url")));
        ////        //LogUtil.WriteLog("code_url:" + code_url);
        ////        return Redirect(code_url);
        ////    }

        ////    //LogUtil.WriteLog(" ============ 开始 获取微信用户相关信息 =====================");

        ////    #region 获取支付用户 OpenID================

        ////    string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AliceDAL.IniHelper.GetValue("WeiXinInfo", "AppId"), PayConfig.AppSecret, code);
        ////    string returnStr = HttpUtil.Send("", url);
        ////    //LogUtil.WriteLog("Send 页面  returnStr 第一个：" + returnStr);

        ////    OpenModel openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

        ////    url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", PayConfig.AppId, openModel.refresh_token);
        ////    returnStr = HttpUtil.Send("", url);
        ////    openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

        ////    #endregion

        ////    #region 支付操作============================

        ////    #region 基本参数===========================

        ////    //商户订单号
        ////    string outTradeNo = oid.ToString() + AliceDAL.Common.CreateRandomValue(10, false);
        ////    //订单名称
        ////    string subject = "小熊洗车";
        ////    //付款金额
        ////    string totalFee = (orderInfo.Amount * 100).ToString("0").Replace(".", "");
        ////    //openId
        ////    string openId = openModel.openid;
        ////    //时间戳 
        ////    string timeStamp = TenpayUtil.getTimestamp();
        ////    //随机字符串 
        ////    string nonceStr = TenpayUtil.getNoncestr();
        ////    //服务器异步通知页面路径
        ////    string notifyUrl = string.Format("http://{0}/OrderWeiXinPay/notify", AliceDAL.IniHelper.GetValue("WebInfo", "Url"));

        ////    //LogUtil.WriteLog("totalFee" + totalFee);

        ////    //创建支付应答对象
        ////    RequestHandler packageReqHandler = new RequestHandler(System.Web.HttpContext.Current);
        ////    //初始化
        ////    packageReqHandler.init();

        ////    //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
        ////    packageReqHandler.setParameter("body", subject); //商品信息 127字符
        ////    packageReqHandler.setParameter("appid", PayConfig.AppId);
        ////    packageReqHandler.setParameter("mch_id", PayConfig.MchId);
        ////    packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
        ////    packageReqHandler.setParameter("notify_url", notifyUrl);
        ////    packageReqHandler.setParameter("openid", openId);
        ////    packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
        ////    packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
        ////    packageReqHandler.setParameter("total_fee", totalFee); //商品金额,以分为单位(money * 100).ToString()
        ////    packageReqHandler.setParameter("trade_type", "JSAPI");
        ////    //if (!string.IsNullOrEmpty(this.Attach))
        ////    //    packageReqHandler.setParameter("attach", this.Attach);//自定义参数 127字符

        ////    #endregion

        ////    #region sign===============================

        ////    string sign = packageReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
        ////    //LogUtil.WriteLog("WeiPay 页面  sign：" + sign);

        ////    #endregion

        ////    #region 获取package包======================

        ////    packageReqHandler.setParameter("sign", sign);

        ////    string data = packageReqHandler.parseXML();
        ////    //LogUtil.WriteLog("WeiPay 页面  package（XML）：" + data);

        ////    string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
        ////    //LogUtil.WriteLog("WeiPay 页面  package（Back_XML）：" + prepayXml);

        ////    //获取预支付ID
        ////    string prepayId = "";
        ////    string package = "";
        ////    XmlDocument xdoc = new XmlDocument();
        ////    xdoc.LoadXml(prepayXml);
        ////    XmlNode xn = xdoc.SelectSingleNode("xml");
        ////    XmlNodeList xnl = xn.ChildNodes;
        ////    if (xnl.Count > 7)
        ////    {
        ////        prepayId = xnl[7].InnerText;
        ////        package = string.Format("prepay_id={0}", prepayId);
        ////        //LogUtil.WriteLog("WeiPay 页面  package：" + package);
        ////    }
        ////    #endregion

        ////    #region 设置支付参数 输出页面  该部分参数请勿随意修改 ==============

        ////    RequestHandler paySignReqHandler = new RequestHandler(System.Web.HttpContext.Current);
        ////    paySignReqHandler.setParameter("appId", PayConfig.AppId);
        ////    paySignReqHandler.setParameter("timeStamp", timeStamp);
        ////    paySignReqHandler.setParameter("nonceStr", nonceStr);
        ////    paySignReqHandler.setParameter("package", package);
        ////    paySignReqHandler.setParameter("signType", "MD5");
        ////    string paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);

        ////    //LogUtil.WriteLog("WeiPay 页面  paySign：" + paySign);

        ////    #endregion

        ////    #endregion

        ////    Dictionary<string, string> model = new Dictionary<string, string>();
        ////    model.Add("oidList", oid.ToString());
        ////    model.Add("timeStamp", timeStamp);
        ////    model.Add("nonceStr", nonceStr);
        ////    model.Add("package", package);
        ////    model.Add("paySign", paySign);
        ////    return PartialView("~/views/orderweixinpay/pay.cshtml", model);
        ////}

        public ActionResult Pay()
        {
            int oid = AliceDAL.UrlParam.GetIntValue("oid");
            Orders orderInfo = DAL.Orders.GetOrderInfoByID(oid);
            if (orderInfo == null) Redirect("/BearClient");
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect", PayConfig.AppId, string.Format("http://{0}/OrderWeiXinPay/pay?oid=" + oid.ToString(), AliceDAL.IniHelper.GetValue("WebInfo", "Url")));
                //LogUtil.WriteLog("code_url:" + code_url);
                return Redirect(code_url);
            }

            LogUtil.WriteLog(" ============ 开始 获取微信用户相关信息 =====================");

            #region 获取支付用户 OpenID================

            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AliceDAL.IniHelper.GetValue("WeiXinInfo", "AppId"), PayConfig.AppSecret, code);
            string returnStr = HttpUtil.Send("", url);
            LogUtil.WriteLog("Send 页面  returnStr 第一个：" + returnStr);

            OpenModel openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", PayConfig.AppId, openModel.refresh_token);
            returnStr = HttpUtil.Send("", url);
            openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            #endregion

            #region 支付操作============================

            #region 基本参数===========================

            //商户订单号
            string outTradeNo = oid.ToString() + AliceDAL.Common.CreateRandomValue(10, false);
            //订单名称
            string subject = "小熊洗车";
            //付款金额
            string totalFee = (orderInfo.Amount * 100).ToString("0").Replace(".", "");
            //openId
            string openId = openModel.openid;
            
            LogUtil.WriteLog(" openid: " + openId);
            //时间戳 
            string timeStamp = TenpayUtil.getTimestamp();
            //随机字符串 
            string nonceStr = TenpayUtil.getNoncestr();
            //服务器异步通知页面路径
            string notifyUrl = string.Format("http://{0}/OrderWeiXinPay/notify", AliceDAL.IniHelper.GetValue("WebInfo", "Url"));

            LogUtil.WriteLog("测试：totalFee" + totalFee);

            //////////////创建支付应答对象
            ////////////WebChatConfig.RequestHandler packageReqHandler = new WebChatConfig.RequestHandler(System.Web.HttpContext.Current);
            //////////////初始化
            ////////////packageReqHandler.init();

            //////////////设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
            ////////////packageReqHandler.setParameter("body", subject); //商品信息 127字符
            ////////////packageReqHandler.setParameter("appid", PayConfig.AppId);
            ////////////packageReqHandler.setParameter("mch_id", PayConfig.MchId);
            ////////////packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
            ////////////packageReqHandler.setParameter("notify_url", notifyUrl);
            ////////////packageReqHandler.setParameter("openid", openId);
            ////////////packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
            ////////////packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            ////////////packageReqHandler.setParameter("total_fee", totalFee); //商品金额,以分为单位(money * 100).ToString()
            ////////////packageReqHandler.setParameter("trade_type", "JSAPI");
            //////////////if (!string.IsNullOrEmpty(this.Attach))
            //////////////    packageReqHandler.setParameter("attach", this.Attach);//自定义参数 127字符

            #endregion

            #region mg add swfitpass支付
            // 参考：https://open.swiftpass.cn/openapi/wiki?index=3&chapter=1

            this.reqHandler = new swiftpass.utils.RequestHandler(null);
            //加载配置数据
            this.cfg = Utils.loadCfg();

            int nIsXiaoXiongPay = -1;
            string strMerchantPaymentAccount;
            string strMerchantPaymentKey;
            if(-1 == GetMechantInfo(orderInfo.StoreID, out nIsXiaoXiongPay, out strMerchantPaymentAccount, out strMerchantPaymentKey))
                return View("此分公司不存在");

            if (0 == nIsXiaoXiongPay)//不使用小熊支付账号，则修改商户号、密钥
            {
                this.cfg["mch_id"] = strMerchantPaymentAccount;
                this.cfg["key"] = strMerchantPaymentKey;
            }
            //使用小熊支付账号，默认小熊的账号信息，记得在config.properties文件配置好



            //初始化数据  
            this.reqHandler.setGateUrl(this.cfg["req_url"].ToString());
            this.reqHandler.setKey(this.cfg["key"].ToString());

            this.reqHandler.setParameter("out_trade_no", outTradeNo);//商户订单号
            this.reqHandler.setParameter("body", subject);//商品描述
            this.reqHandler.setParameter("attach", subject);//附加信息
            this.reqHandler.setParameter("total_fee", totalFee);//总金额
            this.reqHandler.setParameter("mch_create_ip", Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress); //用户的公网ip，不是商户服务器IP//终端IP
            this.reqHandler.setParameter("time_start", GetTime(0)); //订单生成时间
            this.reqHandler.setParameter("time_expire", GetTime(300));//订单超时时间
            this.reqHandler.setParameter("service", "pay.weixin.jspay");//接口类型：pay.weixin.jspay
            this.reqHandler.setParameter("mch_id", this.cfg["mch_id"].ToString());//必填项，商户号，由平台分配
            this.reqHandler.setParameter("version", this.cfg["version"].ToString());//接口版本号
            this.reqHandler.setParameter("notify_url", this.cfg["notify_url"].ToString());
            //通知地址，必填项，接收平台通知的URL，需给绝对路径，255字符内;此URL要保证外网能访问   
            this.reqHandler.setParameter("nonce_str", Utils.random());//随机字符串，必填项，不长于 32 位
            this.reqHandler.setParameter("charset", "UTF-8");//字符集
            this.reqHandler.setParameter("sign_type", "MD5");//签名方式
            this.reqHandler.setParameter("is_raw", "0");//是否原生泰支付方式
            this.reqHandler.setParameter("device_info", "iPhone");//终端设备号
            this.reqHandler.setParameter("sub_openid", openId);//测试账号不传值,此处默认给空值。正式账号必须传openid值，获取openid值指导文档地址：http://www.cnblogs.com/txw1958/p/weixin76-user-info.html
            this.reqHandler.setParameter("callback_url", "https://www.swiftpass.cn");//前台地址  交易完成后跳转的 URL，需给绝对路径，255字 符 内 格 式如:http://wap.tenpay.com/callback.asp
            this.reqHandler.setParameter("goods_tag", "");//商品标记                
            this.reqHandler.createSign();//创建签名
            //以上参数进行签名
            string strData = Utils.toXml(this.reqHandler.getAllParameters());//生成XML报文
            Dictionary<string, string> reqContent = new Dictionary<string, string>();
            reqContent.Add("url", this.reqHandler.getGateUrl());
            reqContent.Add("data", strData);
            this.pay.setReqContent(reqContent);

            if (this.pay.call())
            {
                LogUtil.WriteLog("测试：this.pay.call() true");
                this.resHandler.setContent(this.pay.getResContent());
                this.resHandler.setKey(this.cfg["key"].ToString());
                Hashtable param = this.resHandler.getAllParameters();
                if (this.resHandler.isTenpaySign())
                {
                    //当返回状态与业务结果都为0时才返回，其它结果请查看接口文档
                    if (int.Parse(param["status"].ToString()) == 0 && int.Parse(param["result_code"].ToString()) == 0)
                    {
                        LogUtil.WriteLog("回状态与业务结果都为0.");
                        //Response.Write("<script>alert('请使用原生态JS支付支付，原生态JS值：" + param["pay_info"].ToString() + "')</script>");
                        Response.Redirect("https://pay.swiftpass.cn/pay/jspay?token_id=" + param["token_id"].ToString() + "&showwxtitle=1");
                    }
                    else
                    {
                        Response.Write("<script>alert('错误代码1：" + param["status"] + ",错误信息：" + param["message"] + "')</script>");
                    }

                }
                else
                {
                    Response.Write("<script>alert('错误代码2：" + param["status"] + ",错误信息：" + param["message"] + "')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('错误代码3：" + this.pay.getResponseCode() + ",错误信息：" + this.pay.getErrInfo() + "')</script>");
            }

            #endregion

            ////////////////#region sign===============================

            ////////////////string sign = packageReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
            //////////////////LogUtil.WriteLog("WeiPay 页面  sign：" + sign);

            ////////////////#endregion

            ////////////////#region 获取package包======================

            ////////////////packageReqHandler.setParameter("sign", sign);

            ////////////////string data = packageReqHandler.parseXML();
            //////////////////LogUtil.WriteLog("WeiPay 页面  package（XML）：" + data);

            ////////////////string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
            //////////////////LogUtil.WriteLog("WeiPay 页面  package（Back_XML）：" + prepayXml);

            //////////////////获取预支付ID
            ////////////////string prepayId = "";
            ////////////////string package = "";
            ////////////////XmlDocument xdoc = new XmlDocument();
            ////////////////xdoc.LoadXml(prepayXml);
            ////////////////XmlNode xn = xdoc.SelectSingleNode("xml");
            ////////////////XmlNodeList xnl = xn.ChildNodes;
            ////////////////if (xnl.Count > 7)
            ////////////////{
            ////////////////    prepayId = xnl[7].InnerText;
            ////////////////    package = string.Format("prepay_id={0}", prepayId);
            ////////////////    //LogUtil.WriteLog("WeiPay 页面  package：" + package);
            ////////////////}
            ////////////////#endregion

            ////////////////#region 设置支付参数 输出页面  该部分参数请勿随意修改 ==============

            ////////////////WebChatConfig.RequestHandler paySignReqHandler = new WebChatConfig.RequestHandler(System.Web.HttpContext.Current);
            ////////////////paySignReqHandler.setParameter("appId", PayConfig.AppId);
            ////////////////paySignReqHandler.setParameter("timeStamp", timeStamp);
            ////////////////paySignReqHandler.setParameter("nonceStr", nonceStr);
            ////////////////paySignReqHandler.setParameter("package", package);
            ////////////////paySignReqHandler.setParameter("signType", "MD5");
            ////////////////string paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);

            //////////////////LogUtil.WriteLog("WeiPay 页面  paySign：" + paySign);

            ////////////////#endregion

            #endregion

            ////Dictionary<string, string> model = new Dictionary<string, string>();
            ////model.Add("oidList", oid.ToString());
            ////model.Add("timeStamp", timeStamp);
            ////model.Add("nonceStr", nonceStr);
            ////model.Add("package", package);
            ////model.Add("paySign", paySign);
            ////return PartialView("~/views/orderweixinpay/pay.cshtml", model);

            return View("OK!");
        }

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="nSeconds">0表示当前时间，加上nSeconds的延迟时间</param>
        /// <returns></returns>
        private string GetTime(int nSeconds)
        {
            return DateTime.Now.AddSeconds(nSeconds).ToString("yyyyMMddHHmmss"); ;
        }

        private int GetMechantInfo(int branchid, out int nIsXiaoXiongPay, out string strMerchantPaymentAccount, out string strMerchantPaymentKey)
        {
            nIsXiaoXiongPay = -1;
            strMerchantPaymentAccount = "NA";
            strMerchantPaymentKey = "NA";

            BD_Branch mode = DAL.BD_Branch.GetModel(branchid);
            if (mode == null) 
                return -1;

            nIsXiaoXiongPay = mode.IsXiaoXiongPay;
            strMerchantPaymentAccount = mode.MerchantPaymentAccount;
            strMerchantPaymentKey = mode.MerchantPaymentKey;

            return 0;
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
                        LogUtil.WriteLog("Notify 页面  支付成功，支付信息：商家订单号：" + out_trade_no + "、支付金额(分)：" + total_fee + "、自定义参数：" + attach);

                        /**
                         *  这里输入用户逻辑操作，比如更新订单的支付状态
                         * 
                         * **/

                        string oidList = AliceDAL.Common.SubString(out_trade_no, out_trade_no.Length - 10);//订单id列表

                        string realOidList = oidList;

                        Orders orderInfo = DAL.Orders.GetOrderInfoByID(AliceDAL.DataType.ConvertToInt(realOidList));
                        decimal allSurplusMoney = 0M;

                        allSurplusMoney += orderInfo.Amount;

                        if (orderInfo != null)
                        {
                            if (orderInfo.Amount > 0 && orderInfo.OrderState == (int)UserOrderState.WaitPaying)
                            {
                                DAL.Orders.PayOrder(orderInfo.ID, UserOrderState.Payed, transaction_id, DateTime.Now);
                                DAL.Orders.UseVip(orderInfo.VipCardID, orderInfo.CouponsID, orderInfo.ID);
                                //MessageManger.OrderSuccess(orderInfo.Mobile, orderInfo.Uid, orderInfo.Osn.Contains("XCF") ? "offline" : "online");
                                //MessageManger.FirstOrder(orderInfo.Mobile, orderInfo.Uid);
                                DAL.BD_Consumer.Add(new BD_Consumer()
                                {
                                    Money = -orderInfo.Amount,
                                    OrderID = orderInfo.ID,//Orders ID
                                    Remark = "微信支付",
                                    UserID = orderInfo.Uid
                                });
                                DAL.OrderActions.Add(new OrderActions()
                                {
                                    Oid = orderInfo.ID,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = "支付",
                                    ActionDes = "你使用微信支付订单成功，微信交易号为:" + transaction_id
                                });
                                DAL.UserOrderActions.Add(new UserOrderActions()
                                {
                                    Oid = orderInfo.ID,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = "支付",
                                    ActionDes = "你使用微信支付订单成功"
                                });
                            }

                        }
                        LogUtil.WriteLog("============ 单次支付结束 ===============");
                        return Content("success");
                    }
                    else
                    {
                        LogUtil.WriteLog("Notify 页面  支付失败，支付信息   total_fee= " + total_fee + "、err_code_des=" + err_code_des + "、result_code=" + result_code);
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
        public ActionResult ProPay()
        {
            int oid = AliceDAL.UrlParam.GetIntValue("oid");
            Pro_Orders orderInfo = DAL.Pro_Orders.GetOrderInfoByID(oid);
            if (orderInfo == null) Redirect("/BearMall/ProductList");
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect", PayConfig.AppId, string.Format("http://{0}/OrderWeiXinPay/propay?oid=" + oid.ToString(), AliceDAL.IniHelper.GetValue("WebInfo", "Url")));
                //LogUtil.WriteLog("code_url:" + code_url);
                return Redirect(code_url);
            }

            //LogUtil.WriteLog(" ============ 开始 获取微信用户相关信息 =====================");

            #region 获取支付用户 OpenID================

            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AliceDAL.IniHelper.GetValue("WeiXinInfo", "AppId"), PayConfig.AppSecret, code);
            string returnStr = HttpUtil.Send("", url);
            //LogUtil.WriteLog("Send 页面  returnStr 第一个：" + returnStr);

            OpenModel openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", PayConfig.AppId, openModel.refresh_token);
            returnStr = HttpUtil.Send("", url);
            openModel = JsonConvert.DeserializeObject<OpenModel>(returnStr);

            #endregion

            #region 支付操作============================

            #region 基本参数===========================

            //商户订单号
            string outTradeNo = oid.ToString() + AliceDAL.Common.CreateRandomValue(10, false);
            //订单名称
            string subject = "小熊洗车";
            //付款金额
            string totalFee = (orderInfo.Amount * 100).ToString("0").Replace(".", "");
            //openId
            string openId = openModel.openid;
            //时间戳 
            string timeStamp = TenpayUtil.getTimestamp();
            //随机字符串 
            string nonceStr = TenpayUtil.getNoncestr();
            //服务器异步通知页面路径
            string notifyUrl = string.Format("http://{0}/OrderWeiXinPay/pronotify", AliceDAL.IniHelper.GetValue("WebInfo", "Url"));

            //LogUtil.WriteLog("totalFee" + totalFee);

            //创建支付应答对象
            WebChatConfig.RequestHandler packageReqHandler = new WebChatConfig.RequestHandler(System.Web.HttpContext.Current);
            //初始化
            packageReqHandler.init();

            //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
            packageReqHandler.setParameter("body", subject); //商品信息 127字符
            packageReqHandler.setParameter("appid", PayConfig.AppId);
            packageReqHandler.setParameter("mch_id", PayConfig.MchId);
            packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
            packageReqHandler.setParameter("notify_url", notifyUrl);
            packageReqHandler.setParameter("openid", openId);
            packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
            packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            packageReqHandler.setParameter("total_fee", totalFee); //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.setParameter("trade_type", "JSAPI");
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

            WebChatConfig.RequestHandler paySignReqHandler = new WebChatConfig.RequestHandler(System.Web.HttpContext.Current);
            paySignReqHandler.setParameter("appId", PayConfig.AppId);
            paySignReqHandler.setParameter("timeStamp", timeStamp);
            paySignReqHandler.setParameter("nonceStr", nonceStr);
            paySignReqHandler.setParameter("package", package);
            paySignReqHandler.setParameter("signType", "MD5");
            string paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);

            //LogUtil.WriteLog("WeiPay 页面  paySign：" + paySign);

            #endregion

            #endregion

            Dictionary<string, string> model = new Dictionary<string, string>();
            model.Add("oidList", oid.ToString());
            model.Add("timeStamp", timeStamp);
            model.Add("nonceStr", nonceStr);
            model.Add("package", package);
            model.Add("paySign", paySign);
            return PartialView("~/views/orderweixinpay/pay.cshtml", model);
        }
        /// <summary>
        /// 返回调用
        /// </summary>
        public ActionResult ProNotify()
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

                        Pro_Orders orderInfo = DAL.Pro_Orders.GetOrderInfoByID(AliceDAL.DataType.ConvertToInt(realOidList));
                        decimal allSurplusMoney = 0M;

                        allSurplusMoney += orderInfo.Amount;

                        if (orderInfo != null)
                        {
                            if (orderInfo.Amount > 0 && orderInfo.OrderState == (int)UserProOrderState.WaitPaying)
                            {
                                DAL.Pro_Orders.PayOrder(orderInfo.ID, UserProOrderState.Payed, transaction_id, DateTime.Now);
                                //MessageManger.OrderSuccess(orderInfo.Mobile, orderInfo.Uid, orderInfo.Osn.Contains("XCF") ? "offline" : "online");
                                //MessageManger.FirstOrder(orderInfo.Mobile, orderInfo.Uid);
                                MessageManger.ProOrderSuccess(orderInfo.BusinessID, orderInfo.Osn, orderInfo.ID);
                                DAL.BD_Consumer.Add(new BD_Consumer()
                                {
                                    Money = -orderInfo.Amount,
                                    OrderID = orderInfo.ID,//Orders ID
                                    Remark = "微信支付",
                                    UserID = orderInfo.Uid
                                });
                                DAL.OrderActions.Add(new OrderActions()
                                {
                                    Oid = orderInfo.ID,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = "商品支付",
                                    ActionDes = "你使用微信支付订单成功，微信交易号为:" + transaction_id
                                });
                                DAL.UserOrderActions.Add(new UserOrderActions()
                                {
                                    Oid = 0,
                                    POid = orderInfo.ID,
                                    Uid = orderInfo.Uid,
                                    RealName = "本人",
                                    ActionType = "支付",
                                    ActionDes = "你使用微信支付订单成功"
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

        public JsonResult AjaxResult(string url, string result)
        {
            return Json(new { state = url, msg = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
