using AliceDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Models;

namespace Web.Units
{
    public class Comm
    {
        public static string GetItem(string oid, int i = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<Orders_Item> list = DAL.Orders.GetItemListByOrderID(int.Parse(oid));
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (i == 1) sb.Append(item.ItemName + "(" + item.Money + "元)    ");
                    else sb.Append(item.ItemName + "(" + item.Money + "元)&nbsp;");
                }
            }
            return sb.ToString();
        }
        public static string GetProItem(string oid, int i = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<Pro_Orders_Item> list = DAL.Pro_Orders.GetItemListByOrderID(int.Parse(oid));
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (i == 1) sb.Append(item.ItemName + "(" + item.Money + "元)    ");
                    else sb.Append(item.ItemName + "(" + item.Money + "元)&nbsp;");
                }
            }
            return sb.ToString();
        }
        public static string GetItemNoPrice(string oid, int i = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<Orders_Item> list = DAL.Orders.GetItemListByOrderID(int.Parse(oid));
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (i == 1) sb.Append(item.ItemName + "   ");
                    else if (i == 0) sb.Append(item.ItemName + "&nbsp;");
                    else if (i == 2) sb.Append(item.ItemName + "+");
                }
            }
            return sb.ToString().TrimEnd('+');
        }
        public static string GetProItemNoPrice(string oid, int i = 0)
        {
            StringBuilder sb = new StringBuilder();
            List<Pro_Orders_Item> list = DAL.Pro_Orders.GetItemListByOrderID(int.Parse(oid));
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (i == 1) sb.Append(item.ItemName + "   ");
                    else if (i == 0) sb.Append(item.ItemName + "&nbsp;");
                    else if (i == 2) sb.Append(item.ItemName + "+");
                }
            }
            return sb.ToString().TrimEnd('+');
        }
        public static string GetWorkName(string wid, int i = 0)
        {
            WorkShop ws = DAL.WorkShop.WorkShopByID(int.Parse(wid));
            if (ws != null)
            {
                if (i == 1) return ws.Title;
                else return "<span style='color:#F60'>" + ws.Title + "</span>";
            }
            return "未分配";
        }
        public static string GetBusinessName(string bid, int i = 0)
        {
            BD_Business bb = DAL.BD_Business.GetModel(AliceDAL.DataType.ConvertToInt(bid));
            if (bb != null)
            {
                if (i == 1) return bb.BusinessName;
                else return "<span style='color:#F60'>" + bb.BusinessName + "</span>";
            }
            return "";
        }
        public static List<UserVipCardModel> GetVipCard(int uid, int branchid)
        {
            List<UserVipCardModel> list = new List<UserVipCardModel>();
            DataTable dt = DAL.PageModel.DataKeysList("[uv_VipCard]", "[ID],[Title],[FreeID],[Plate],[UseDate],[TDate],[VipDes],[CardCount],[UserCount],[BusinessName]", "[Uid]=" + uid + " and [BranchID]=" + branchid + " and [CardState]=30 and UserCount<CardCount", "ID", 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    UserVipCardModel uvcm = new UserVipCardModel();
                    uvcm.CardState = "";
                    uvcm.FreeID = DataType.ConvertToInt(item["FreeID"].ToString());
                    uvcm.ID = DataType.ConvertToInt(item["ID"].ToString());
                    uvcm.CardCount = DataType.ConvertToInt(item["CardCount"].ToString());
                    uvcm.UserCount = DataType.ConvertToInt(item["UserCount"].ToString());
                    uvcm.Plate = item["Plate"].ToString();
                    uvcm.Title = item["Title"].ToString();
                    uvcm.CDate = DataType.ConvertToDateTimeStr(item["UseDate"]);//有效期自开卡起
                    uvcm.TDate = DataType.ConvertToDateTimeStr(item["TDate"]);
                    uvcm.VipDes = item["VipDes"].ToString();
                    uvcm.BusinessName = item["BusinessName"].ToString();
                    list.Add(uvcm);
                }
            }
            return list;
        }
        public static List<UserCarModel> GetCarList(int uid, int page = 1)
        {
            ListModel listmodel = new ListModel();
            StringBuilder strsql = new StringBuilder(String.Format("[UserID]={0}", uid));
            ListModel.CreatePage(listmodel, "[UserCar]", strsql, page, 5);

            List<UserCarModel> list = new List<UserCarModel>();
            if (page > listmodel.Page.TotalPageCount)
            {
                return list;
            }
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                foreach (DataRow item in listmodel.Page)
                {
                    UserCarModel ucm = new UserCarModel();
                    ucm.BrandShow = item["BrandShow"].ToString();
                    ucm.Color = item["Color"].ToString();
                    ucm.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                    ucm.Plate = item["Plate"].ToString();
                    ucm.UserID = AliceDAL.DataType.ConvertToInt(item["UserID"].ToString());
                    list.Add(ucm);
                }
            }
            return list;
        }
        public static List<IGrouping<int, UserCouponsModel>> GetCoupons(int uid, int branchid)
        {
            List<UserCouponsModel> list1 = new List<UserCouponsModel>();
            DataTable dt1 = DAL.PageModel.DataKeysList("[uv_Coupons]", "[ID],[Title],[Price],[BusinessID],[TypeID],[BusinessName],[CDate],[TDate]", "[Uid]=" + uid + " and [BranchID]=" + branchid + " and [CouponState]=10 and TDate>'" + DateTime.Now.ToString() + "'", "BusinessID desc, ID", 0);
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                foreach (DataRow item in dt1.Rows)
                {
                    UserCouponsModel ucm = new UserCouponsModel();
                    ucm.BusinessID = DataType.ConvertToInt(item["BusinessID"].ToString());
                    ucm.BusinessName = item["BusinessName"].ToString();
                    ucm.CDate = DataType.ConvertToDateTimeStr(item["CDate"]);
                    ucm.TypeID = DataType.ConvertToInt(item["TypeID"].ToString());
                    ucm.ID = DataType.ConvertToInt(item["ID"].ToString());
                    ucm.Price = DataType.ConvertToDecimal(item["Price"].ToString());
                    ucm.Title = item["Title"].ToString();
                    ucm.TDate = DataType.ConvertToDateTimeStr(item["TDate"]);
                    list1.Add(ucm);
                }
            }
            List<IGrouping<int, UserCouponsModel>> group = list1.GroupBy(r => r.TypeID).ToList();
            //foreach (var item in group)
            //{
            //    IGrouping<int, UserCouponsModel> converted = item;
            //    for (int i = 0; i < converted.Count(); i++)
            //    {
            //        UserCouponsModel ucm = converted.ElementAt(i);
            //    }
            //    foreach (UserCouponsModel obj in converted)
            //    {
            //        Console.WriteLine(obj.Title);
            //    }
            //}
            return group;
        }
    }
}