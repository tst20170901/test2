using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class Coupons
    {
        public static bool CreateCoupons(Models.Coupons model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO [Coupons] ([Uid],[TypeID],[Price],[OriginalPrice],[CouponState],[TDate])VALUES(@Uid,@TypeID,@Price,@OriginalPrice,@CouponState,@TDate);");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Uid", SqlDbType.Int, 4), 
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@OriginalPrice", SqlDbType.Decimal,9), 
                                                new SqlParameter("@CouponState", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@TDate",SqlDbType.DateTime)
                                            };
                parameters[0].Value = model.Uid;
                parameters[1].Value = model.TypeID;
                parameters[2].Value = model.Price;
                parameters[3].Value = model.OriginalPrice;
                parameters[4].Value = model.CouponState;
                parameters[5].Value = model.TDate;
                return SQLHelper.ExecuteTransactionSQL(strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
        public static Models.Coupons BuidCoupon(DataRow row)
        {
            Models.Coupons cp = new Models.Coupons();
            if (row != null)
            {
                cp.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                cp.CouponState = DataType.ConvertToInt(row["CouponState"].ToString());
                cp.ID = DataType.ConvertToInt(row["ID"].ToString());
                cp.OriginalPrice = DataType.ConvertToDecimal(row["OriginalPrice"].ToString());
                cp.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                cp.TDate = DataType.ConvertToDateTime(row["TDate"].ToString());
                cp.TypeID = DataType.ConvertToInt(row["TypeID"].ToString());
                cp.Uid = DataType.ConvertToInt(row["Uid"].ToString());
                cp.UseDate = DataType.ConvertToDateTime(row["UseDate"].ToString());
                cp.UserOid = DataType.ConvertToInt(row["UserOid"].ToString());
            }
            return cp;
        }
        public static List<Models.Coupons> GetListByUserID(int userID)
        {
            try
            {
                string sql = "select * from [Coupons] where [Uid]=@Uid order by [CouponState] asc,[TDate] asc";
                SqlParameter[] para = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                para[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.Coupons> list = new List<Models.Coupons>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        list.Add(BuidCoupon(item));
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
        public static Models.Coupons GetCouponsByID(int ID)
        {
            try
            {
                string sql = "select * from [uv_Coupons] where ID=@ID";
                SqlParameter[] para = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                para[0].Value = ID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                Models.Coupons cp = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    cp = new Models.Coupons();
                    cp.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                    cp.CouponState = DataType.ConvertToInt(dt.Rows[0]["CouponState"].ToString());
                    cp.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                    cp.OriginalPrice = DataType.ConvertToDecimal(dt.Rows[0]["OriginalPrice"].ToString());
                    cp.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                    cp.TDate = DataType.ConvertToDateTime(dt.Rows[0]["TDate"].ToString());
                    cp.TypeID = DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
                    cp.Uid = DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                    cp.UseDate = DataType.ConvertToDateTime(dt.Rows[0]["UseDate"].ToString());
                    cp.UserOid = DataType.ConvertToInt(dt.Rows[0]["UserOid"].ToString());
                    cp.Title = dt.Rows[0]["Title"].ToString();
                    return cp;
                }
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [Coupons] set [CouponState]=@CouponState where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@CouponState", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@ID",SqlDbType.Int,4)
                                            };

                parameters[0].Value = state;
                parameters[1].Value = ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static Models.Coupons GetAvailableCoupons(int userID)
        {
            try
            {
                string sql = "select top 1 * from [Coupons] where Uid=@Uid and CouponState=10 order by [TDate] asc";
                SqlParameter[] para = { new SqlParameter("@Uid", SqlDbType.Int, 4) };
                para[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                Models.Coupons c = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    c = BuidCoupon(dt.Rows[0]);
                    return c;
                }
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static void ChangeCoupons(int id, Models.CouponState couponState)
        {
            try
            {
                string sql = "UPDATE [Coupons] SET [CouponState]=@CouponState WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@CouponState", SqlDbType.TinyInt, 1), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = (int)couponState;
                parameters[1].Value = id;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
    }
}

