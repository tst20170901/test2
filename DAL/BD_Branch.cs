using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class BD_Branch
    {
        public static string Edit(Models.BD_Branch model)
        {
            try
            {
                string sql = "if exists (select 1 from [BD_Branch] where Title=@Title and ID<>@ID) " +
                             "begin " +
                               "select 'exists title';" +
                             "end " +
                             "else " +
                             "begin " +
                                "UPDATE [BD_Branch] set [Title]=@Title,[Mobile]=@Mobile,[Province]=@Province,[City]=@City,[CityCode]=@CityCode," +
                                "[District]=@District,[Adcode]=@Adcode,[CenterLng]=@CenterLng,[CenterLat]=@CenterLat,[Points]=@Points,[StateMsg]=@StateMsg," +
                                "[OrderCount]=@OrderCount, [IsXiaoXiongPay]=@IsXiaoXiongPay, [MerchantPaymentAccount]=@MerchantPaymentAccount, [MerchantPaymentKey]=@MerchantPaymentKey where ID=@ID;" +
                                "SELECT 'ok';" +
                             "end";
                SqlParameter[] parameters = {
                                                new SqlParameter("@ID", SqlDbType.Int,4),
                                                new SqlParameter("@CenterLng", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CenterLat", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Points", SqlDbType.NVarChar,1000),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Province", SqlDbType.NVarChar,50),
                                                new SqlParameter("@City", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CityCode", SqlDbType.NVarChar,10),
                                                new SqlParameter("@District", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Adcode", SqlDbType.VarChar,10),
                                                new SqlParameter("@StateMsg",SqlDbType.NVarChar,1000),
                                                new SqlParameter("@OrderCount",SqlDbType.Int,4),
                                                new SqlParameter("@IsXiaoXiongPay",SqlDbType.Int,4),
                                                new SqlParameter("@MerchantPaymentAccount",SqlDbType.NVarChar,100),
                                                new SqlParameter("@MerchantPaymentKey",SqlDbType.NVarChar,200)
                                            };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.CenterLng;
                parameters[2].Value = model.CenterLat;
                parameters[3].Value = model.Points;
                parameters[4].Value = model.Title;
                parameters[5].Value = model.Mobile;
                parameters[6].Value = model.Province;
                parameters[7].Value = model.City;
                parameters[8].Value = model.CityCode;
                parameters[9].Value = model.District;
                parameters[10].Value = model.Adcode;
                parameters[11].Value = model.StateMsg;
                parameters[12].Value = model.OrderCount;

                parameters[13].Value = model.IsXiaoXiongPay;
                parameters[14].Value = model.MerchantPaymentAccount;
                parameters[15].Value = model.MerchantPaymentKey;

                CacheManager.Remove(CacheKeys.BRANCHLIST);
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static string Add(Models.BD_Branch model)
        {
            try
            {
                string sql = "if exists (select 1 from [BD_Branch] where Title=@Title) " +
                             "begin " +
                               "select 'exists title';" +
                             "end " +
                             "else " +
                             "begin " +
                                "DECLARE @oid int;INSERT INTO [BD_Branch] ([CenterLng],[CenterLat],[Points],[Title],[Mobile],[Province],[City],[CityCode],[District],[Adcode],[StateMsg],[OrderCount],[IsXiaoXiongPay],[MerchantPaymentAccount],[MerchantPaymentKey])" +
                                " VALUES (@CenterLng,@CenterLat,@Points,@Title,@Mobile,@Province,@City,@CityCode,@District,@Adcode,@StateMsg,@OrderCount,@IsXiaoXiongPay,@MerchantPaymentAccount,@MerchantPaymentKey);" +
                                "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                             "end";

                SqlParameter[] parameters = {
                                                new SqlParameter("@CenterLng", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CenterLat", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Points", SqlDbType.NVarChar,1000),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Province", SqlDbType.NVarChar,50),
                                                new SqlParameter("@City", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CityCode", SqlDbType.NVarChar,10),
                                                new SqlParameter("@District", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Adcode", SqlDbType.VarChar,10),
                                                new SqlParameter("@StateMsg",SqlDbType.NVarChar,1000),
                                                new SqlParameter("@OrderCount",SqlDbType.Int,4),
                                                new SqlParameter("@IsXiaoXiongPay",SqlDbType.Int,4),
                                                new SqlParameter("@MerchantPaymentAccount",SqlDbType.NVarChar,100),
                                                new SqlParameter("@MerchantPaymentKey",SqlDbType.NVarChar,100)
                                            };
                parameters[0].Value = model.CenterLng;
                parameters[1].Value = model.CenterLat;
                parameters[2].Value = model.Points;
                parameters[3].Value = model.Title;
                parameters[4].Value = model.Mobile;
                parameters[5].Value = model.Province;
                parameters[6].Value = model.City;
                parameters[7].Value = model.CityCode;
                parameters[8].Value = model.District;
                parameters[9].Value = model.Adcode;
                parameters[10].Value = model.StateMsg;
                parameters[11].Value = model.OrderCount;

                parameters[12].Value = model.IsXiaoXiongPay;
                parameters[13].Value = model.MerchantPaymentAccount;
                parameters[14].Value = model.MerchantPaymentKey;

                CacheManager.Remove(CacheKeys.BRANCHLIST);
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        private static Models.BD_Branch BuildBranch(DataRow row)
        {
            try
            {
                Models.BD_Branch bb = new Models.BD_Branch();
                if (row != null)
                {
                    bb.Adcode = row["Adcode"].ToString();
                    bb.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                    bb.City = row["City"].ToString();
                    bb.CityCode = row["CityCode"].ToString();
                    bb.District = row["District"].ToString();
                    bb.ID = DataType.ConvertToInt(row["ID"].ToString());
                    bb.CenterLat = row["CenterLat"].ToString();
                    bb.CenterLng = row["CenterLng"].ToString();
                    bb.Province = row["Province"].ToString();
                    bb.Title = row["Title"].ToString();
                    bb.Mobile = row["Mobile"].ToString();
                    bb.Points = row["Points"].ToString();
                    bb.StateMsg = row["StateMsg"].ToString();
                    bb.OrderCount = DataType.ConvertToInt(row["OrderCount"].ToString());
                    bb.BranchState = DataType.ConvertToInt(row["BranchState"].ToString());

                    bb.IsXiaoXiongPay = DataType.ConvertToInt(row["IsXiaoXiongPay"].ToString());
                    bb.MerchantPaymentAccount = row["MerchantPaymentAccount"].ToString();
                    bb.MerchantPaymentKey = row["MerchantPaymentKey"].ToString();
                }
                return bb;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static void ChangeBranchState(int bid, int state)
        {
            try
            {
                string sql = "UPDATE [BD_Branch] SET [BranchState]=@BranchState WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@BranchState", SqlDbType.TinyInt, 1), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = state;
                parameters[1].Value = bid;
                CacheManager.Remove(CacheKeys.BRANCHLIST);
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void ChangeBranchState(int bid, int state, string statemsg, int ordercount)
        {
            try
            {
                string sql = "UPDATE [BD_Branch] SET [BranchState]=@BranchState,[StateMsg]=@StateMsg,[OrderCount]=@OrderCount WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@BranchState", SqlDbType.TinyInt, 1), new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@StateMsg", SqlDbType.NVarChar, 1000), new SqlParameter("@OrderCount", SqlDbType.Int, 4) };
                parameters[0].Value = state;
                parameters[1].Value = bid;
                parameters[2].Value = statemsg;
                parameters[3].Value = ordercount;
                CacheManager.Remove(CacheKeys.BRANCHLIST);
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// 获取本城市可用服务点列表
        /// </summary>
        /// <param name="co"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static DataTable BranchForCode(string co, string ad)
        {
            try
            {
                string sql = "select top 100 * from [BD_Branch] where CityCode=@CityCode and Adcode=@Adcode";
                SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10), new SqlParameter("@Adcode", SqlDbType.NVarChar, 10) };
                parameters[0].Value = co;
                parameters[1].Value = ad;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }

        public static DataTable BranchForCode(string co)
        {
            try
            {
                string sql = "select top 100 * from [BD_Branch] where CityCode=@CityCode";
                SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10) };
                parameters[0].Value = co;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.BD_Branch GetModel(int id)
        {
            try
            {
                string sql = "select * from [BD_Branch] where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = id;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return BuildBranch(dt.Rows[0]);
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
    }
}

