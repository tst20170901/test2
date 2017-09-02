using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class Edo_CheckLogs
    {
        public static int Add(Models.Edo_CheckLogs model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO [Edo_CheckLogs] ([Mobile],[Plate],[UserID],[UserMobile]) VALUES (@Mobile,@Plate,@UserID,@UserMobile);");
                SqlParameter[] parameters = { new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Plate", SqlDbType.NVarChar, 10), new SqlParameter("@UserID", SqlDbType.Int, 4), new SqlParameter("@UserMobile", SqlDbType.NVarChar, 50) };
                parameters[0].Value = model.Mobile;
                parameters[1].Value = model.Plate;
                parameters[2].Value = model.UserID;
                parameters[3].Value = model.UserMobile;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 以车牌号为准，一天只能查6个
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static int GetNoticeByUidAndPlate(string plate, int uid)
        {
            try
            {
                string sql = "select count(distinct [Plate]) from [Edo_CheckLogs] where [UserID]=@UserID and [Plate]<>@Plate and ([CDate] BETWEEN '" + DateTime.Now.ToString("yyyy/MM/dd 00:00:00") + "' and '" + DateTime.Now.ToString("yyyy/MM/dd 23:59:59") + "')";
                SqlParameter[] param = { new SqlParameter("@UserID", uid), new SqlParameter("@Plate", plate) };
                int c = AliceDAL.DataType.ConvertToInt(SQLHelper.ExecuteScalar(CommandType.Text, sql, param).ToString());
                return c;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
    }
}

