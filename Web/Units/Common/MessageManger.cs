using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Units
{
    public class MessageManger
    {
        public static void OrderSuccess(string mobile, int userID, string type = "online")
        {
            string con = "";
            int hour = DateTime.Now.Hour;
            if (hour >= 8 && hour <= 17)
            {
                con = "【小熊洗车】您的订单已提交成功，小熊洗车人员正在火速赶往中，请您耐心等待，祝您洗车愉快！客服电话:0318-8888235";
            }
            else if (hour < 8 && hour >= 0)
            {
                con = "【小熊洗车】您的订单已提交成功，小熊洗车人员将在日出之时火速赶往，请您耐心等待，祝您洗车愉快！客服电话:0318-8888235";
            }
            else if (hour > 17 && hour < 24)
            {
                con = "【小熊洗车】您的订单已提交成功，由于单量过多，小熊将在次日为您进行服务，祝您洗车愉快！客服电话:0318-8888235";
            }

            if (type == "offline")
            {
                con = "【小熊洗车】您的订单已提交成功，服务订单已经自动排队，请到已选择实体店享受服务，祝您洗车愉快！客服电话:0318-8888235";
            }
            SendMsg(mobile, con);
        }
        public static void ProOrderSuccess(int bid, string osn, int oid)
        {
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            if (bb != null && !String.IsNullOrEmpty(bb.Data2))
            {
                string con = "您有新订单，订单编号：" + osn + "，订单明细：" + Web.Units.Comm.GetProItem(oid.ToString(), 1);
                SendMsg(bb.Data2, con);
            }
        }
        public static void OrderAssign(string mobile, string workname, string workmobile)
        {
            string con = "【小熊洗车】您的订单已成功分配，洗车工：" + workname + "，手机号：" + workmobile + "。客服电话：0318-8888235，祝您生活愉快。";
            PostMsg(mobile, con);
        }
        public static void OrderFinish(string mobile)
        {
            string con = "【小熊洗车】洗车完成，请到订单管理对我们的服务做出评价，以便我们提供更完善的服务。客服电话：0318-8888235，祝您生活愉快。";
            PostMsg(mobile, con);
        }
        public static void FirstOrder(string mobile, int userID)
        {
            //int result = DAL.PageModel.DataCount("[Orders]", String.Format("[Uid]={0} and [OrderState]=10", userID));
            //int result1 = DAL.PageModel.DataCount("[Orders]", String.Format("[Uid]={0}", userID));
            //if (result == 1 && result1 == 1)
            //{
            //    string con = "【小熊洗车】尊敬的小熊用户您好，非常感谢您试用小熊移动洗车服务，小熊账户50元优惠券已到帐，可立即消费，祝您洗车愉快！客服电话:0318-8888235";
            //    int count = DAL.PageModel.DataCount("Coupons", String.Format("[Uid]={0} and [TypeID]={1}", userID, 12));//12为新注册用户5元券的ID
            //    if (count <= 0)
            //    {
            //        Coupons cou = new Coupons()
            //        {
            //            CouponState = (int)CouponState.Submitted,
            //            OriginalPrice = 5,
            //            Price = 5,
            //            TypeID = 8,
            //            Uid = userID,
            //            Count = 10,
            //            TDate = DateTime.Now.AddDays(180)
            //        };
            //        for (int i = 0; i < cou.Count; i++)
            //        {
            //            DAL.Coupons.CreateCoupons(cou);
            //        }
            //    }
            //    SendMsg(mobile, con);
            //}
        }
        public static void FirstOrder(string mobile, int userID, int youhui)
        {
            bool r = false;
            int result = DAL.PageModel.DataCount("[Orders]", String.Format("[Uid]={0} and [OrderState]=10", userID));
            int result1 = DAL.PageModel.DataCount("[Orders]", String.Format("[Uid]={0}", userID));
            if (result == 1 && result1 == 1)
            {
                string con = "";
                switch (youhui)
                {
                    case 111:
                        int resultitem = DAL.PageModel.DataCount("[uv_Orders_Item]", String.Format("[UserID]={0} and [SortID]=999", userID));
                        if (resultitem == 1)//存在用户首单选项，则查找首单返券活动
                        {
                            BD_BusiAction bb = DAL.BD_BusiAction.GetModelBySortID();

                            if (bb != null && bb.ActionState == 10)
                            {
                                con = bb.SMSContent;
                                List<BD_BusiAction_Item> list = DAL.BD_BusiAction.GetItemListByActionID(bb.ID);

                                if (list != null && list.Count > 0)
                                {
                                    foreach (BD_BusiAction_Item item in list)
                                    {
                                        int count = DAL.PageModel.DataCount("[Coupons]", String.Format("[Uid]={0} and [TypeID]={1}", userID, item.ItemID));
                                        if (count <= 0)
                                        {
                                            Coupons cou = new Coupons()
                                            {
                                                CouponState = (int)CouponState.Submitted,
                                                OriginalPrice = item.ItemPrice,
                                                Price = item.ItemPrice,
                                                TypeID = item.ItemID,
                                                Uid = userID,
                                                Count = item.Count,
                                                TDate = DateTime.Now.AddDays(item.Period)
                                            };
                                            for (int i = 0; i < cou.Count; i++)
                                            {
                                                DAL.Coupons.CreateCoupons(cou);
                                            }
                                            r = true;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 222:
                        con = "【小熊洗车】尊敬的用户您好，非常感谢您试用小熊移动洗车服务，现赠送您共计25元洗车优惠券（半年期限），祝您洗车愉快！客服电话:0318-8888235";
                        Coupons coua = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = 5,
                            Price = 5,
                            TypeID = 8,
                            Uid = userID,
                            Count = 1,
                            TDate = DateTime.Now.AddDays(181)
                        };//一张5块
                        for (int i = 0; i < coua.Count; i++)
                        {
                            DAL.Coupons.CreateCoupons(coua);
                        }
                        Coupons coub = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = 10,
                            Price = 10,
                            TypeID = 9,
                            Uid = userID,
                            Count = 2,
                            TDate = DateTime.Now.AddDays(181)
                        };//两张10块
                        for (int i = 0; i < coub.Count; i++)
                        {
                            DAL.Coupons.CreateCoupons(coub);
                        }
                        break;
                    case 333:
                        con = "【小熊洗车】尊敬的用户您好，非常感谢您试用小熊移动洗车服务，现赠送您共计25元洗车优惠券（半年期限），祝您洗车愉快！客服电话:0318-8888235";
                        Coupons couc = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = 5,
                            Price = 5,
                            TypeID = 8,
                            Uid = userID,
                            Count = 1,
                            TDate = DateTime.Now.AddDays(181)
                        };//一张5块
                        for (int i = 0; i < couc.Count; i++)
                        {
                            DAL.Coupons.CreateCoupons(couc);
                        }
                        Coupons coud = new Coupons()
                        {
                            CouponState = (int)CouponState.Submitted,
                            OriginalPrice = 10,
                            Price = 10,
                            TypeID = 9,
                            Uid = userID,
                            Count = 2,
                            TDate = DateTime.Now.AddDays(181)
                        };//两张10块
                        for (int i = 0; i < coud.Count; i++)
                        {
                            DAL.Coupons.CreateCoupons(coud);
                        }
                        break;
                    default:
                        break;
                }
                if (r && !String.IsNullOrEmpty(con))
                {
                    SendMsg(mobile, con);
                }
            }
        }
        public static void PostMsg(string mobile, string content)
        {
            string userId = "SDK-HFY-010-00079";
            string pass = AliceDAL.SecureHelper.MD5("SDK-HFY-010-000797CC9bf-1").ToUpper();
            string con = content;
            string url = "http://sdk2.zucp.net/webservice.asmx/mdSmsSend";
            string data = String.Format("sn={0}&pwd={1}&mobile={2}&content={3}&ext=&stime=&rrid=", userId, pass, mobile, System.Web.HttpUtility.UrlEncode(con, Encoding.GetEncoding("gb2312")));
            AliceDAL.Error.WriteErrorLogTemp(mobile + content);
            AMap.Common.PostUrl(url, data);
        }
        public static void SendMsg(string mobile, string content)
        {
            string userId = "SDK-HFY-010-00079";
            string pass = AliceDAL.SecureHelper.MD5("SDK-HFY-010-000797CC9bf-1").ToUpper();
            string con = content;
            string u = String.Format("http://sdk2.entinfo.cn/webservice.asmx/MongateCsSpSendSmsNew?userId={0}&password={1}&pszMobis={2}&pszMsg={3}&iMobiCount=1&pszSubPort=", userId, pass, mobile, System.Web.HttpUtility.UrlEncode(con, Encoding.GetEncoding("gb2312")));
            AliceDAL.Error.WriteErrorLogTemp(mobile + content);
            AMap.Common.RequestUrl(u);
        }
    }
}