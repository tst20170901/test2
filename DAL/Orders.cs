using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class Orders
    {
        public static string GenerateOSN(int uid)
        {
            StringBuilder osn = new StringBuilder();
            osn.AppendFormat("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), uid);
            return osn.ToString();
        }
        /// <summary>
        /// 今天预约时间内，订单数
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public static int GetTimeNodeOrderCount(int branchID, string timestr)
        {
            try
            {
                string sql = "select count(1) ID from [Orders] where [StoreID]=@StoreID and [Remark]=@Remark and OrderState=10 and [CDate] between '" + DateTime.Now.ToString("yyyy/MM/dd") + "' and '" + DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + "' order by ID desc;";
                SqlParameter[] parameters = { new SqlParameter("@StoreID", SqlDbType.Int, 4), new SqlParameter("@Remark", SqlDbType.NVarChar, 500) };
                parameters[0].Value = branchID;
                parameters[1].Value = timestr;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int CreateNewOrder(Models.Orders model, List<Models.Orders_Item> list)
        {
            try
            {
                string sql = "DECLARE @oid int;INSERT INTO [Orders] ([Osn],[Uid],[OrderState],[UserName],[Mobile],[Plate],[BrandShow],[BrandID],[ModelID],[StoreID],[WorkID],[Color],[Lat],[Lng],[IP],[Remark],[FinishDate],[Address],[PaySn],[PayName],[PayTime],[Amount],[Data3],[DiscountDes],[DiscountAmount],[Data4],[VipCardID],[CouponsID]) VALUES (" +
                             "@Osn,@Uid,@OrderState,@UserName,@Mobile,@Plate,@BrandShow,@BrandID,@ModelID,@StoreID,@WorkID,@Color,@Lat,@Lng,@IP,@Remark,@FinishDate,@Address,@PaySn,@PayName,@PayTime,@Amount,@Data3,@DiscountDes,@DiscountAmount,@Data4,@VipCardID,@CouponsID);SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';";
                SqlParameter[] parameters = {
                                                new SqlParameter("@ModelID", SqlDbType.Int,4),
                                                new SqlParameter("@StoreID", SqlDbType.Int,4),
                                                new SqlParameter("@WorkID", SqlDbType.Int,1),
                                                new SqlParameter("@Color", SqlDbType.NVarChar,10),
                                                new SqlParameter("@Lat", SqlDbType.NVarChar,30),
                                                new SqlParameter("@Lng", SqlDbType.NVarChar,30),
                                                new SqlParameter("@IP", SqlDbType.NVarChar,20), 
                                                new SqlParameter("@Osn", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Uid", SqlDbType.Int,4),
                                                new SqlParameter("@OrderState", SqlDbType.TinyInt,1),            
                                                new SqlParameter("@UserName", SqlDbType.NVarChar,50),            
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar,50),            
                                                new SqlParameter("@Plate", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@BrandShow", SqlDbType.NVarChar,30),            
                                                new SqlParameter("@BrandID", SqlDbType.Int,4),
                                                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                                                new SqlParameter("@FinishDate",SqlDbType.DateTime),
                                                new SqlParameter("@Address",SqlDbType.NVarChar,200),
                                                new SqlParameter("@PayTime", SqlDbType.DateTime),
                                                new SqlParameter("@PaySn", SqlDbType.NVarChar,30),
                                                new SqlParameter("@PayName", SqlDbType.NVarChar,30),
                                                new SqlParameter("@Amount",SqlDbType.Decimal,9),
                                                new SqlParameter("@Data3",SqlDbType.NVarChar,-1),
                                                new SqlParameter("@DiscountDes",SqlDbType.NVarChar,-1),
                                                new SqlParameter("@DiscountAmount",SqlDbType.Decimal,9),
                                                new SqlParameter("@Data4",SqlDbType.NVarChar,-1),
                                                new SqlParameter("@VipCardID",SqlDbType.Int,4),
                                                new SqlParameter("@CouponsID",SqlDbType.NVarChar,1000)
                                            };
                parameters[0].Value = model.ModelID;
                parameters[1].Value = model.StoreID;
                parameters[2].Value = model.WorkID;
                parameters[3].Value = model.Color;
                parameters[4].Value = model.Lat;
                parameters[5].Value = model.Lng;
                parameters[6].Value = model.IP;
                parameters[7].Value = model.Osn;
                parameters[8].Value = model.Uid;
                parameters[9].Value = model.OrderState;
                parameters[10].Value = model.UserName;
                parameters[11].Value = model.Mobile;
                parameters[12].Value = model.Plate;
                parameters[13].Value = model.BrandShow;
                parameters[14].Value = model.BrandID;
                parameters[15].Value = model.Remark;
                parameters[16].Value = model.FinishDate;
                parameters[17].Value = model.Address;
                parameters[18].Value = model.PayTime;
                parameters[19].Value = model.PaySn;
                parameters[20].Value = model.PayName;
                parameters[21].Value = model.Amount;
                parameters[22].Value = model.Data3;
                parameters[23].Value = model.DiscountDes;
                parameters[24].Value = model.DiscountAmount;
                parameters[25].Value = model.Data4;
                parameters[26].Value = model.VipCardID;
                parameters[27].Value = model.CouponsID;
                int oid = AliceDAL.DataType.ConvertToInt(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());

                foreach (Models.Orders_Item item in list)
                {
                    string s = "INSERT INTO [Orders_Item]([Oid],[UserID],[ItemID],[ItemName],[StoreID],[Money],[WorkPrice])VALUES(@Oid,@UserID,@ItemID,@ItemName,@StoreID,@Money,@WorkPrice)";
                    SqlParameter[] p = { 
                                           new SqlParameter("@Oid", SqlDbType.Int, 4), 
                                           new SqlParameter("@UserID", SqlDbType.Int, 4), 
                                           new SqlParameter("@ItemID", SqlDbType.Int, 4),
                                           new SqlParameter("@ItemName",SqlDbType.NVarChar,50),
                                           new SqlParameter("@StoreID",SqlDbType.Int,4),
                                           new SqlParameter("@Money",SqlDbType.Decimal,9),
                                           new SqlParameter("@WorkPrice",SqlDbType.Decimal,9)
                                       };
                    p[0].Value = oid;
                    p[1].Value = item.UserID;
                    p[2].Value = item.ItemID;
                    p[3].Value = item.ItemName;
                    p[4].Value = item.StoreID;
                    p[5].Value = item.Money;
                    p[6].Value = item.WorkPrice;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, s, p);
                }
                return oid;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 此方法放弃，新方法CreateNewOrder
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int CreateOrder(Models.Orders model, List<Models.Orders_Item> list)
        {
            try
            {
                string sql = "DECLARE @oid int;INSERT INTO [Orders] ([Osn],[Uid],[OrderState],[UserName],[Mobile],[Plate],[BrandShow],[BrandID],[ModelID],[StoreID],[WorkID],[Color],[Lat],[Lng],[IP],[Remark],[FinishDate],[Address],[PaySn],[PayName],[PayTime],[Amount],[Data3]) VALUES (" +
                             "@Osn,@Uid,@OrderState,@UserName,@Mobile,@Plate,@BrandShow,@BrandID,@ModelID,@StoreID,@WorkID,@Color,@Lat,@Lng,@IP,@Remark,@FinishDate,@Address,@PaySn,@PayName,@PayTime,@Amount,@Data3);SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';";
                SqlParameter[] parameters = {
                                                new SqlParameter("@ModelID", SqlDbType.Int,4),
                                                new SqlParameter("@StoreID", SqlDbType.Int,4),
                                                new SqlParameter("@WorkID", SqlDbType.Int,1),
                                                new SqlParameter("@Color", SqlDbType.NVarChar,10),
                                                new SqlParameter("@Lat", SqlDbType.NVarChar,30),
                                                new SqlParameter("@Lng", SqlDbType.NVarChar,30),
                                                new SqlParameter("@IP", SqlDbType.NVarChar,20), 
                                                new SqlParameter("@Osn", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Uid", SqlDbType.Int,4),
                                                new SqlParameter("@OrderState", SqlDbType.TinyInt,1),            
                                                new SqlParameter("@UserName", SqlDbType.NVarChar,50),            
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar,50),            
                                                new SqlParameter("@Plate", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@BrandShow", SqlDbType.NVarChar,30),            
                                                new SqlParameter("@BrandID", SqlDbType.Int,4),
                                                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                                                new SqlParameter("@FinishDate",SqlDbType.DateTime),
                                                new SqlParameter("@Address",SqlDbType.NVarChar,200),
                                                new SqlParameter("@PayTime", SqlDbType.DateTime),
                                                new SqlParameter("@PaySn", SqlDbType.NVarChar,30),
                                                new SqlParameter("@PayName", SqlDbType.NVarChar,30),
                                                new SqlParameter("@Amount",SqlDbType.Decimal,9),
                                                new SqlParameter("@Data3",SqlDbType.NVarChar,-1)
                                            };
                parameters[0].Value = model.ModelID;
                parameters[1].Value = model.StoreID;
                parameters[2].Value = model.WorkID;
                parameters[3].Value = model.Color;
                parameters[4].Value = model.Lat;
                parameters[5].Value = model.Lng;
                parameters[6].Value = model.IP;
                parameters[7].Value = model.Osn;
                parameters[8].Value = model.Uid;
                parameters[9].Value = model.OrderState;
                parameters[10].Value = model.UserName;
                parameters[11].Value = model.Mobile;
                parameters[12].Value = model.Plate;
                parameters[13].Value = model.BrandShow;
                parameters[14].Value = model.BrandID;
                parameters[15].Value = model.Remark;
                parameters[16].Value = model.FinishDate;
                parameters[17].Value = model.Address;
                parameters[18].Value = model.PayTime;
                parameters[19].Value = model.PaySn;
                parameters[20].Value = model.PayName;
                parameters[21].Value = model.Amount;
                parameters[22].Value = model.Data3;
                int oid = AliceDAL.DataType.ConvertToInt(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());

                foreach (Models.Orders_Item item in list)
                {
                    string s = "INSERT INTO [Orders_Item]([Oid],[UserID],[ItemID],[ItemName],[StoreID],[Money],[WorkPrice])VALUES(@Oid,@UserID,@ItemID,@ItemName,@StoreID,@Money,@WorkPrice)";
                    SqlParameter[] p = { 
                                           new SqlParameter("@Oid", SqlDbType.Int, 4), 
                                           new SqlParameter("@UserID", SqlDbType.Int, 4), 
                                           new SqlParameter("@ItemID", SqlDbType.Int, 4),
                                           new SqlParameter("@ItemName",SqlDbType.NVarChar,50),
                                           new SqlParameter("@StoreID",SqlDbType.Int,4),
                                           new SqlParameter("@Money",SqlDbType.Decimal,9),
                                           new SqlParameter("@WorkPrice",SqlDbType.Decimal,9)
                                       };
                    p[0].Value = oid;
                    p[1].Value = item.UserID;
                    p[2].Value = item.ItemID;
                    p[3].Value = item.ItemName;
                    p[4].Value = item.StoreID;
                    p[5].Value = item.Money;
                    p[6].Value = item.WorkPrice;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, s, p);
                }
                return oid;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 使用优惠券或者会员卡
        /// </summary>
        /// <param name="vipid"></param>
        /// <param name="couponids"></param>
        /// <param name="oid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void UseVip(int vipid, string couponids, int oid)
        {
            try
            {
                if (vipid > 0)
                {
                    string sql = "Update [VipCard] set [UserCount]=[UserCount]+1 where ID=@ID;";
                    SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                    parameters[0].Value = vipid;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
                }
                if (!String.IsNullOrEmpty(couponids) && couponids.Split(',') != null && couponids.Split(',').Length > 0)
                {
                    List<SQLHelper.TransactionModel> listtm = new List<SQLHelper.TransactionModel>();
                    string sql1 = "Update [Coupons] set [UserOid]=@Oid,[CouponState]=30,[UseDate]=getdate() where ID=@ID;select '1';";

                    foreach (var item in couponids.Split(','))
                    {
                        SqlParameter[] parameters = { new SqlParameter("@Oid", SqlDbType.Int, 4), new SqlParameter("@ID", SqlDbType.Int, 4) };
                        parameters[0].Value = oid;
                        parameters[1].Value = item;
                        SQLHelper.TransactionModel tm = new SQLHelper.TransactionModel();
                        tm.Params = parameters;
                        tm.StoredProcedureName = sql1;
                        tm.Result = "1";
                        listtm.Add(tm);
                    }
                    SQLHelper.ExecuteTransactionSQL(listtm);
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// 取消订单后，会员卡优惠券返回
        /// </summary>
        /// <param name="vipid"></param>
        /// <param name="couponids"></param>
        /// <param name="oid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void CancelUseVip(int vipid, string couponids, int oid)
        {
            try
            {
                if (vipid > 0)
                {
                    string sql = "if exists(select 1 from [VipCard] where [ID]=@ID and [UserCount]>0) " +
                                 "begin " +
                                 "  Update [VipCard] set [UserCount]=[UserCount]-1 where ID=@ID;" +
                                 "end ";

                    SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                    parameters[0].Value = vipid;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
                }
                if (!String.IsNullOrEmpty(couponids) && couponids.Split(',') != null && couponids.Split(',').Length > 0)
                {
                    List<SQLHelper.TransactionModel> listtm = new List<SQLHelper.TransactionModel>();
                    string sql1 = "if exists(select 1 from [Coupons] where [ID]=@ID and [CouponState]=30) " +
                                  "begin " +
                                  "  Update [Coupons] set [UserOid]=0,[CouponState]=10,[UseDate]='1900-01-01' where ID=@ID;select '1';" +
                                  "end ";

                    foreach (var item in couponids.Split(','))
                    {
                        SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                        parameters[0].Value = item;
                        SQLHelper.TransactionModel tm = new SQLHelper.TransactionModel();
                        tm.Params = parameters;
                        tm.StoredProcedureName = sql1;
                        tm.Result = "1";
                        listtm.Add(tm);
                    }
                    SQLHelper.ExecuteTransactionSQL(listtm);
                }

            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static Models.Orders GetOrderInfoByID(int oid)
        {
            try
            {
                string sql = "select * from [Orders] where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Orders model = new Models.Orders();
                    model.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    model.ModelID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                    model.WorkID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["WorkID"].ToString());
                    model.StoreID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["StoreID"].ToString());
                    model.Color = dt.Rows[0]["Color"].ToString();
                    model.Lat = dt.Rows[0]["Lat"].ToString();
                    model.Lng = dt.Rows[0]["Lng"].ToString();
                    model.IP = dt.Rows[0]["IP"].ToString();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    model.Osn = dt.Rows[0]["Osn"].ToString();
                    model.Uid = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    model.OrderState = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    model.UserName = dt.Rows[0]["UserName"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Plate = dt.Rows[0]["Plate"].ToString();
                    model.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                    model.BrandID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                    model.Remark = dt.Rows[0]["Remark"].ToString();
                    model.FinishDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["FinishDate"].ToString());
                    model.Address = dt.Rows[0]["Address"].ToString();
                    model.PayName = dt.Rows[0]["PayName"].ToString();
                    model.PaySn = dt.Rows[0]["PaySn"].ToString();
                    model.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    model.Amount = DataType.ConvertToDecimal(dt.Rows[0]["Amount"].ToString());
                    model.Data1 = dt.Rows[0]["Data1"].ToString();
                    model.Data2 = dt.Rows[0]["Data2"].ToString();
                    model.Data3 = dt.Rows[0]["Data3"].ToString();
                    model.Data4 = dt.Rows[0]["Data4"].ToString();
                    model.Data5 = dt.Rows[0]["Data5"].ToString();
                    model.Data6 = dt.Rows[0]["Data6"].ToString();
                    model.Data7 = dt.Rows[0]["Data7"].ToString();
                    model.Data8 = dt.Rows[0]["Data8"].ToString();
                    model.Data9 = dt.Rows[0]["Data9"].ToString();
                    model.Data10 = dt.Rows[0]["Data10"].ToString();
                    model.DiscountDes = dt.Rows[0]["DiscountDes"].ToString();
                    model.DiscountAmount = DataType.ConvertToDecimal(dt.Rows[0]["DiscountAmount"].ToString());
                    model.VipCardID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["VipCardID"].ToString());
                    model.CouponsID = dt.Rows[0]["CouponsID"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 用户名下是否有订单，什么状态的也不能有，防止作弊。
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Models.Orders ExistOrder(int userID)
        {
            try
            {
                string sql = "select top 1 * from [Orders] where Uid=@Uid order by ID desc;";
                SqlParameter[] parameters = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                parameters[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Orders model = new Models.Orders();
                    model.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    model.ModelID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                    model.WorkID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["WorkID"].ToString());
                    model.StoreID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["StoreID"].ToString());
                    model.Color = dt.Rows[0]["Color"].ToString();
                    model.Lat = dt.Rows[0]["Lat"].ToString();
                    model.Lng = dt.Rows[0]["Lng"].ToString();
                    model.IP = dt.Rows[0]["IP"].ToString();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    model.Osn = dt.Rows[0]["Osn"].ToString();
                    model.Uid = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    model.OrderState = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    model.UserName = dt.Rows[0]["UserName"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Plate = dt.Rows[0]["Plate"].ToString();
                    model.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                    model.BrandID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                    model.Remark = dt.Rows[0]["Remark"].ToString();
                    model.FinishDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["FinishDate"].ToString());
                    model.Address = dt.Rows[0]["Address"].ToString();
                    model.PayName = dt.Rows[0]["PayName"].ToString();
                    model.PaySn = dt.Rows[0]["PaySn"].ToString();
                    model.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    model.Data1 = dt.Rows[0]["Data1"].ToString();
                    model.Data2 = dt.Rows[0]["Data2"].ToString();
                    model.Data3 = dt.Rows[0]["Data3"].ToString();
                    model.Data4 = dt.Rows[0]["Data4"].ToString();
                    model.Data5 = dt.Rows[0]["Data5"].ToString();
                    model.Data6 = dt.Rows[0]["Data6"].ToString();
                    model.Data7 = dt.Rows[0]["Data7"].ToString();
                    model.Data8 = dt.Rows[0]["Data8"].ToString();
                    model.Data9 = dt.Rows[0]["Data9"].ToString();
                    model.Data10 = dt.Rows[0]["Data10"].ToString();
                    model.DiscountDes = dt.Rows[0]["DiscountDes"].ToString();
                    model.DiscountAmount = DataType.ConvertToDecimal(dt.Rows[0]["DiscountAmount"].ToString());
                    model.VipCardID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["VipCardID"].ToString());
                    model.CouponsID = dt.Rows[0]["CouponsID"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.Orders ExistOrder(string plate)
        {
            try
            {
                string sql = "select top 1 * from [Orders] where [Plate]=@Plate and (OrderState=10 or OrderState=30 or OrderState=50 or OrderState=70) order by FinishDate desc";
                SqlParameter[] parameters = { new SqlParameter("@Plate", SqlDbType.NVarChar, -1) };
                parameters[0].Value = plate;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Orders model = new Models.Orders();
                    model.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    model.ModelID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                    model.WorkID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["WorkID"].ToString());
                    model.StoreID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["StoreID"].ToString());
                    model.Color = dt.Rows[0]["Color"].ToString();
                    model.Lat = dt.Rows[0]["Lat"].ToString();
                    model.Lng = dt.Rows[0]["Lng"].ToString();
                    model.IP = dt.Rows[0]["IP"].ToString();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    model.Osn = dt.Rows[0]["Osn"].ToString();
                    model.Uid = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    model.OrderState = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    model.UserName = dt.Rows[0]["UserName"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Plate = dt.Rows[0]["Plate"].ToString();
                    model.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                    model.BrandID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                    model.Remark = dt.Rows[0]["Remark"].ToString();
                    model.FinishDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["FinishDate"].ToString());
                    model.Address = dt.Rows[0]["Address"].ToString();
                    model.PayName = dt.Rows[0]["PayName"].ToString();
                    model.PaySn = dt.Rows[0]["PaySn"].ToString();
                    model.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    model.Data1 = dt.Rows[0]["Data1"].ToString();
                    model.Data2 = dt.Rows[0]["Data2"].ToString();
                    model.Data3 = dt.Rows[0]["Data3"].ToString();
                    model.Data4 = dt.Rows[0]["Data4"].ToString();
                    model.Data5 = dt.Rows[0]["Data5"].ToString();
                    model.Data6 = dt.Rows[0]["Data6"].ToString();
                    model.Data7 = dt.Rows[0]["Data7"].ToString();
                    model.Data8 = dt.Rows[0]["Data8"].ToString();
                    model.Data9 = dt.Rows[0]["Data9"].ToString();
                    model.Data10 = dt.Rows[0]["Data10"].ToString();
                    model.DiscountDes = dt.Rows[0]["DiscountDes"].ToString();
                    model.DiscountAmount = DataType.ConvertToDecimal(dt.Rows[0]["DiscountAmount"].ToString());
                    model.VipCardID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["VipCardID"].ToString());
                    model.CouponsID = dt.Rows[0]["CouponsID"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.Orders GetLastestOrder(int userID)
        {
            try
            {
                string sql = "select top 1 * from [Orders] where Uid=@Uid order by ID desc;";
                SqlParameter[] parameters = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                parameters[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Orders model = new Models.Orders();
                    model.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    model.ModelID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                    model.WorkID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["WorkID"].ToString());
                    model.StoreID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["StoreID"].ToString());
                    model.Color = dt.Rows[0]["Color"].ToString();
                    model.Lat = dt.Rows[0]["Lat"].ToString();
                    model.Lng = dt.Rows[0]["Lng"].ToString();
                    model.IP = dt.Rows[0]["IP"].ToString();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    model.Osn = dt.Rows[0]["Osn"].ToString();
                    model.Uid = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    model.OrderState = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    model.UserName = dt.Rows[0]["UserName"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Plate = dt.Rows[0]["Plate"].ToString();
                    model.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                    model.BrandID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                    model.Remark = dt.Rows[0]["Remark"].ToString();
                    model.FinishDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["FinishDate"].ToString());
                    model.Address = dt.Rows[0]["Address"].ToString();
                    model.PayName = dt.Rows[0]["PayName"].ToString();
                    model.PaySn = dt.Rows[0]["PaySn"].ToString();
                    model.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    model.Data1 = dt.Rows[0]["Data1"].ToString();
                    model.Data2 = dt.Rows[0]["Data2"].ToString();
                    model.Data3 = dt.Rows[0]["Data3"].ToString();
                    model.Data4 = dt.Rows[0]["Data4"].ToString();
                    model.Data5 = dt.Rows[0]["Data5"].ToString();
                    model.Data6 = dt.Rows[0]["Data6"].ToString();
                    model.Data7 = dt.Rows[0]["Data7"].ToString();
                    model.Data8 = dt.Rows[0]["Data8"].ToString();
                    model.Data9 = dt.Rows[0]["Data9"].ToString();
                    model.Data10 = dt.Rows[0]["Data10"].ToString();
                    model.DiscountDes = dt.Rows[0]["DiscountDes"].ToString();
                    model.DiscountAmount = DataType.ConvertToDecimal(dt.Rows[0]["DiscountAmount"].ToString());
                    model.VipCardID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["VipCardID"].ToString());
                    model.CouponsID = dt.Rows[0]["CouponsID"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int GetLastestOid(int workID)
        {
            try
            {
                string sql = "select top 1 ID from [Orders] where WorkID=@WorkID and OrderState=10 order by ID desc;";
                SqlParameter[] parameters = { new SqlParameter("@WorkID", SqlDbType.Int, 4) };
                parameters[0].Value = workID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static Models.Orders GetWorkOrder(int workID)
        {
            try
            {
                string sql = "select top 1 * from [Orders] where WorkID=@WorkID and (OrderState=10 or OrderState=30 or OrderState=50 or OrderState=70) order by FinishDate desc;";
                SqlParameter[] parameters = { new SqlParameter("@WorkID", SqlDbType.Int, 4) };
                parameters[0].Value = workID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Orders model = new Models.Orders();
                    model.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    model.ModelID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                    model.WorkID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["WorkID"].ToString());
                    model.StoreID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["StoreID"].ToString());
                    model.Color = dt.Rows[0]["Color"].ToString();
                    model.Lat = dt.Rows[0]["Lat"].ToString();
                    model.Lng = dt.Rows[0]["Lng"].ToString();
                    model.IP = dt.Rows[0]["IP"].ToString();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    model.Osn = dt.Rows[0]["Osn"].ToString();
                    model.Uid = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    model.OrderState = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    model.UserName = dt.Rows[0]["UserName"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Plate = dt.Rows[0]["Plate"].ToString();
                    model.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                    model.BrandID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                    model.Remark = dt.Rows[0]["Remark"].ToString();
                    model.FinishDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["FinishDate"].ToString());
                    model.Address = dt.Rows[0]["Address"].ToString();
                    model.PayName = dt.Rows[0]["PayName"].ToString();
                    model.PaySn = dt.Rows[0]["PaySn"].ToString();
                    model.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    model.Data1 = dt.Rows[0]["Data1"].ToString();
                    model.Data2 = dt.Rows[0]["Data2"].ToString();
                    model.Data3 = dt.Rows[0]["Data3"].ToString();
                    model.Data4 = dt.Rows[0]["Data4"].ToString();
                    model.Data5 = dt.Rows[0]["Data5"].ToString();
                    model.Data6 = dt.Rows[0]["Data6"].ToString();
                    model.Data7 = dt.Rows[0]["Data7"].ToString();
                    model.Data8 = dt.Rows[0]["Data8"].ToString();
                    model.Data9 = dt.Rows[0]["Data9"].ToString();
                    model.Data10 = dt.Rows[0]["Data10"].ToString();
                    model.DiscountDes = dt.Rows[0]["DiscountDes"].ToString();
                    model.DiscountAmount = DataType.ConvertToDecimal(dt.Rows[0]["DiscountAmount"].ToString());
                    model.VipCardID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["VipCardID"].ToString());
                    model.CouponsID = dt.Rows[0]["CouponsID"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int GetMaxOid(int oid, int branchID)
        {
            try
            {
                string sql = "select top 1 ID from [Orders] where [ID]>@ID and [StoreID]=@StoreID order by [ID] desc;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@StoreID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                parameters[1].Value = branchID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static List<Models.Orders> GetListByUserID(int userID)
        {
            try
            {
                string sql = "select * from [Orders] where [Uid]=@Uid and [OrderState]<>110 order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                para[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.Orders> list = new List<Models.Orders>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.Orders model = new Models.Orders();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.ModelID = AliceDAL.DataType.ConvertToInt(item["ModelID"].ToString());
                        model.WorkID = AliceDAL.DataType.ConvertToInt(item["WorkID"].ToString());
                        model.StoreID = AliceDAL.DataType.ConvertToInt(item["StoreID"].ToString());
                        model.Color = item["Color"].ToString();
                        model.Lat = item["Lat"].ToString();
                        model.Lng = item["Lng"].ToString();
                        model.IP = item["IP"].ToString();
                        model.CDate = AliceDAL.DataType.ConvertToDateTime(item["CDate"].ToString());
                        model.Osn = item["Osn"].ToString();
                        model.Uid = AliceDAL.DataType.ConvertToInt(item["Uid"].ToString());
                        model.OrderState = AliceDAL.DataType.ConvertToInt(item["OrderState"].ToString());
                        model.UserName = item["UserName"].ToString();
                        model.Mobile = item["Mobile"].ToString();
                        model.Plate = item["Plate"].ToString();
                        model.BrandShow = item["BrandShow"].ToString();
                        model.BrandID = AliceDAL.DataType.ConvertToInt(item["BrandID"].ToString());
                        model.Remark = item["Remark"].ToString();
                        model.FinishDate = AliceDAL.DataType.ConvertToDateTime(item["FinishDate"].ToString());
                        model.Address = item["Address"].ToString();
                        model.PayName = item["PayName"].ToString();
                        model.PaySn = item["PaySn"].ToString();
                        model.PayTime = DataType.ConvertToDateTime(item["PayTime"].ToString());
                        model.Amount = DataType.ConvertToDecimal(item["Amount"].ToString());
                        model.Data1 = item["Data1"].ToString();
                        model.Data2 = item["Data2"].ToString();
                        model.Data3 = item["Data3"].ToString();
                        model.Data4 = item["Data4"].ToString();
                        model.Data5 = item["Data5"].ToString();
                        model.Data6 = item["Data6"].ToString();
                        model.Data7 = item["Data7"].ToString();
                        model.Data8 = item["Data8"].ToString();
                        model.Data9 = item["Data9"].ToString();
                        model.Data10 = item["Data10"].ToString();
                        model.DiscountDes = item["DiscountDes"].ToString();
                        model.DiscountAmount = DataType.ConvertToDecimal(item["DiscountAmount"].ToString());
                        model.VipCardID = AliceDAL.DataType.ConvertToInt(item["VipCardID"].ToString());
                        model.CouponsID = item["CouponsID"].ToString();
                        list.Add(model);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static List<Models.Orders_Item> GetItemListByOrderID(int orderID)
        {
            try
            {
                string sql = "select * from [Orders_Item] where [Oid]=@Oid order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@Oid", SqlDbType.Int, 4) };
                para[0].Value = orderID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.Orders_Item> list = new List<Models.Orders_Item>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.Orders_Item model = new Models.Orders_Item();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.Oid = AliceDAL.DataType.ConvertToInt(item["Oid"].ToString());
                        model.UserID = AliceDAL.DataType.ConvertToInt(item["UserID"].ToString());
                        model.ItemID = AliceDAL.DataType.ConvertToInt(item["ItemID"].ToString());
                        model.ItemName = item["ItemName"].ToString();
                        model.StoreID = AliceDAL.DataType.ConvertToInt(item["StoreID"].ToString());
                        model.Money = AliceDAL.DataType.ConvertToDecimal(item["Money"].ToString());
                        model.CDate = DataType.ConvertToDateTime(item["CDate"].ToString());
                        list.Add(model);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static void Delete(int oid)
        {
            try
            {
                string sql = "delete from [Orders] WHERE [ID]=@ID;delete from [Orders_Item] where Oid=@ID;delete from [UserOrderActions] where Oid=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void ChangeOrder(int oid, Models.UserOrderState orderState)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [OrderState]=@OrderState,[FinishDate]=getdate() WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@OrderState", SqlDbType.TinyInt, 1), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = (int)orderState;
                parameters[1].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void EditOrderCar(int oid, string brand, string color, string plate)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [BrandShow]=@BrandShow,[Color]=@Color,[Plate]=@Plate WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@BrandShow", SqlDbType.NVarChar, 30), new SqlParameter("@Color", SqlDbType.NVarChar, 10), new SqlParameter("@Plate", SqlDbType.NVarChar, -1), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = brand;
                parameters[1].Value = color;
                parameters[2].Value = plate;
                parameters[3].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void EditOrderCar(int oid, string brand, string color, string plate, string remark, string yy)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [BrandShow]=@BrandShow,[Color]=@Color,[Plate]=@Plate,[Data4]=@Data4,[Remark]=@Remark WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@BrandShow", SqlDbType.NVarChar, 30), new SqlParameter("@Color", SqlDbType.NVarChar, 10), new SqlParameter("@Plate", SqlDbType.NVarChar, -1), new SqlParameter("@Remark", SqlDbType.NVarChar, 500), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = brand;
                parameters[1].Value = color;
                parameters[2].Value = plate;
                parameters[3].Value = yy;
                parameters[4].Value = remark;
                parameters[5].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }

        public static void GetWorker(int oid, int workID)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [WorkID]=@WorkID WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@WorkID", SqlDbType.Int, 4), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = workID;
                parameters[1].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void UpdateStartImg(int oid, string img)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [Data1]=@Data1 WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = img;
                parameters[1].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void UpdateEndImg(int oid, string img)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [Data2]=@Data2 WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = img;
                parameters[1].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void PayOrder(int oid, Models.UserOrderState orderState, string paySN, DateTime payTime)
        {
            try
            {
                string sql = "UPDATE [Orders] SET [OrderState]=@OrderState,[PaySn]=@PaySn,[PayTime]=@PayTime WHERE [ID]=@ID;";
                SqlParameter[] parameters = {
			            new SqlParameter("@OrderState", SqlDbType.TinyInt,1) ,            
                        new SqlParameter("@PaySn", SqlDbType.NVarChar,30) ,            
                        new SqlParameter("@PayTime", SqlDbType.DateTime) ,            
                        new SqlParameter("@ID", SqlDbType.Int,4)};

                parameters[0].Value = (int)orderState;
                parameters[1].Value = paySN;
                parameters[2].Value = payTime;
                parameters[3].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static DataSet AccountingOrders(int branchID, string start, string end)
        {
            try
            {
                string sql = "[AccountingOrders]";
                SqlParameter[] parameters = { new SqlParameter("@BranchID", SqlDbType.Int, 4), new SqlParameter("@StartDate", SqlDbType.DateTime), new SqlParameter("@EndDate", SqlDbType.DateTime) };

                parameters[0].Value = branchID;
                parameters[1].Value = start;
                parameters[2].Value = end;
                return SQLHelper.ExecuteDataSet(CommandType.StoredProcedure, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
    }
}

