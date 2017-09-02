using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class Pro_Orders
    {
        public static string GenerateOSN(int uid)
        {
            StringBuilder osn = new StringBuilder();
            osn.AppendFormat("Pro_{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), uid);
            return osn.ToString();
        }
        public static int CreateNewOrder(Models.Pro_Orders model, List<Models.Pro_Orders_Item> list)
        {
            try
            {
                string sql = "DECLARE @oid int;INSERT INTO [Pro_Orders] ([Osn],[Uid],[OrderState],[UserName],[Mobile],[Plate],[BrandShow],[BrandID],[ModelID],[StoreID],[WorkID],[Color],[Lat],[Lng],[IP],[Remark],[FinishDate],[Address],[PaySn],[PayName],[PayTime],[Amount],[Data3],[DiscountDes],[DiscountAmount],[Data4],[VipCardID],[CouponsID],[BusinessID]) VALUES (" +
                             "@Osn,@Uid,@OrderState,@UserName,@Mobile,@Plate,@BrandShow,@BrandID,@ModelID,@StoreID,@WorkID,@Color,@Lat,@Lng,@IP,@Remark,@FinishDate,@Address,@PaySn,@PayName,@PayTime,@Amount,@Data3,@DiscountDes,@DiscountAmount,@Data4,@VipCardID,@CouponsID,@BusinessID);SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';";
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
                                                new SqlParameter("@CouponsID",SqlDbType.NVarChar,1000),
                                                new SqlParameter("@BusinessID",SqlDbType.Int,4)
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
                parameters[28].Value = model.BusinessID;                                                                                                                                                                                                                                                                                         
                int oid = AliceDAL.DataType.ConvertToInt(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());

                foreach (Models.Pro_Orders_Item item in list)
                {
                    string s = "INSERT INTO [Pro_Orders_Item]([Oid],[UserID],[ItemID],[ItemName],[StoreID],[Money],[WorkPrice])VALUES(@Oid,@UserID,@ItemID,@ItemName,@StoreID,@Money,@WorkPrice)";
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
        public static Models.Pro_Orders GetOrderInfoByID(int oid)
        {
            try
            {
                string sql = "select * from [Pro_Orders] where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Pro_Orders model = new Models.Pro_Orders();
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
                    model.BusinessID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString());
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
        public static Models.Pro_Orders GetLastestOrder(int userID)
        {
            try
            {
                string sql = "select top 1 * from [Pro_Orders] where Uid=@Uid order by ID desc;";
                SqlParameter[] parameters = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                parameters[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.Pro_Orders model = new Models.Pro_Orders();
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
                string sql = "select top 1 ID from [Pro_Orders] where WorkID=@WorkID and OrderState=10 order by ID desc;";
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
        public static int GetMaxOid(int oid, int branchID)
        {
            try
            {
                string sql = "select top 1 ID from [Pro_Orders] where [ID]>@ID and [StoreID]=@StoreID order by [ID] desc;";
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
        public static int GetMaxOid1(int oid, int businessID)
        {
            try
            {
                string sql = "select top 1 ID from [Pro_Orders] where [ID]>@ID and [BusinessID]=@BusinessID order by [ID] desc;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@StoreID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                parameters[1].Value = businessID;
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
        public static List<Models.Pro_Orders> GetListByUserID(int userID)
        {
            try
            {
                string sql = "select * from [Pro_Orders] where [Uid]=@Uid and [OrderState]<>110 order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                para[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.Pro_Orders> list = new List<Models.Pro_Orders>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.Pro_Orders model = new Models.Pro_Orders();
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
                        model.BusinessID = AliceDAL.DataType.ConvertToInt(item["BusinessID"].ToString());
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
        public static List<Models.Pro_Orders_Item> GetItemListByOrderID(int orderID)
        {
            try
            {
                string sql = "select * from [Pro_Orders_Item] where [Oid]=@Oid order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@Oid", SqlDbType.Int, 4) };
                para[0].Value = orderID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.Pro_Orders_Item> list = new List<Models.Pro_Orders_Item>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.Pro_Orders_Item model = new Models.Pro_Orders_Item();
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
                string sql = "delete from [Pro_Orders] WHERE [ID]=@ID;delete from [Orders_Item] where Oid=@ID;delete from [UserOrderActions] where Oid=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = oid;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void ChangeOrder(int oid, Models.UserProOrderState orderState)
        {
            try
            {
                string sql = "UPDATE [Pro_Orders] SET [OrderState]=@OrderState,[FinishDate]=getdate() WHERE [ID]=@ID;";
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
        public static void PayOrder(int oid, Models.UserProOrderState orderState, string paySN, DateTime payTime)
        {
            try
            {
                string sql = "UPDATE [Pro_Orders] SET [OrderState]=@OrderState,[PaySn]=@PaySn,[PayTime]=@PayTime WHERE [ID]=@ID;";
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
    }
}

