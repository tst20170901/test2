using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class AdminOrders
    {
        public static string GenerateOSN(int uid)
        {
            StringBuilder osn = new StringBuilder();
            osn.AppendFormat("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), uid);
            return osn.ToString();
        }
        public static int CreateOrder(Models.AdminOrders model)
        {
            try
            {
                string sql = "DECLARE @oid int;INSERT INTO [AdminOrders] ([Osn],[Uid],[OrderState],[TypeID],[Count],[OrderAmount],[PaySn],[PayName],[PayTime]) VALUES (" +
                             "@Osn,@Uid,@OrderState,@TypeID,@Count,@OrderAmount,@PaySn,@PayName,@PayTime);SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';";
                SqlParameter[] parameters = {
			            new SqlParameter("@PayTime", SqlDbType.DateTime) ,            
                        new SqlParameter("@Osn", SqlDbType.NVarChar,30) ,            
                        new SqlParameter("@Uid", SqlDbType.Int,4) ,            
                        new SqlParameter("@OrderState", SqlDbType.TinyInt,1) ,            
                        new SqlParameter("@TypeID", SqlDbType.Int,4) ,                   
                        new SqlParameter("@Count", SqlDbType.Int,4) ,            
                        new SqlParameter("@OrderAmount", SqlDbType.Decimal,9),     
                        new SqlParameter("@PaySn", SqlDbType.NVarChar,30) ,         
                        new SqlParameter("@PayName", SqlDbType.NVarChar,30)};

                parameters[0].Value = model.PayTime;
                parameters[1].Value = model.Osn;
                parameters[2].Value = model.Uid;
                parameters[3].Value = model.OrderState;
                parameters[4].Value = model.TypeID;
                parameters[5].Value = model.Count;
                parameters[6].Value = model.OrderAmount;
                parameters[7].Value = model.PaySn;
                parameters[8].Value = model.PayName;
                return AliceDAL.DataType.ConvertToInt(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static Models.AdminOrders GetOrderInfoByID(int oid)
        {
            try
            {
                string sql = "select * from [AdminOrders] where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = oid;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Models.AdminOrders ws = new Models.AdminOrders();
                    ws.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    ws.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    ws.OrderAmount = DataType.ConvertToDecimal(dt.Rows[0]["OrderAmount"].ToString());
                    ws.OrderState = DataType.ConvertToInt(dt.Rows[0]["OrderState"].ToString());
                    ws.Osn = dt.Rows[0]["Osn"].ToString();
                    ws.PayName = dt.Rows[0]["PayName"].ToString();
                    ws.PaySn = dt.Rows[0]["PaySn"].ToString();
                    ws.PayTime = DataType.ConvertToDateTime(dt.Rows[0]["PayTime"].ToString());
                    ws.TypeID = DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
                    ws.Count = DataType.ConvertToInt(dt.Rows[0]["Count"].ToString());
                    ws.Uid = DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    return ws;
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
        public static List<Models.AdminOrders> GetListByUserID(int userID)
        {
            try
            {
                string sql = "select * from [AdminOrders] where [Uid]=@userID and PayTime>'2000/01/01 00:00:00';";
                SqlParameter[] parameters = { new SqlParameter("@userID", SqlDbType.Int, 4) };

                parameters[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                List<Models.AdminOrders> list = new List<Models.AdminOrders>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.AdminOrders ws = new Models.AdminOrders();
                        ws.CDate = DataType.ConvertToDateTime(item["CDate"].ToString());
                        ws.ID = DataType.ConvertToInt(item["ID"].ToString());
                        ws.OrderAmount = DataType.ConvertToDecimal(item["OrderAmount"].ToString());
                        ws.OrderState = DataType.ConvertToInt(item["OrderState"].ToString());
                        ws.Osn = item["Osn"].ToString();
                        ws.PayName = item["PayName"].ToString();
                        ws.PaySn = item["PaySn"].ToString();
                        ws.PayTime = DataType.ConvertToDateTime(item["PayTime"].ToString());
                        ws.TypeID = DataType.ConvertToInt(item["TypeID"].ToString());
                        ws.Count = DataType.ConvertToInt(item["Count"].ToString());
                        ws.Uid = DataType.ConvertToInt(item["Uid"].ToString());
                        list.Add(ws);
                    }
                    return list;
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
        public static void PayOrder(int oid, Models.OrderState orderState, string paySN, DateTime payTime)
        {
            try
            {
                string sql = "UPDATE [AdminOrders] SET [OrderState]=@OrderState,[PaySn]=@PaySn,[PayTime]=@PayTime WHERE [ID]=@ID;";
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

