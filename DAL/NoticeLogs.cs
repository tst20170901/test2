using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class NoticeLogs
    {
        public static int Add(Models.NoticeLogs model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO [NoticeLogs] ([Mobile],[Plate],[WorkID],[WorkName],[BranchID],[SMSContent]) VALUES (@Mobile,@Plate,@WorkID,@WorkName,@BranchID,@SMSContent);");
                SqlParameter[] parameters = { new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Plate", SqlDbType.NVarChar, 10), new SqlParameter("@WorkID", SqlDbType.Int, 4), new SqlParameter("@WorkName", SqlDbType.NVarChar, 50), new SqlParameter("@BranchID", SqlDbType.Int, 4), new SqlParameter("@SMSContent", SqlDbType.NVarChar, 500) };
                parameters[0].Value = model.Mobile;
                parameters[1].Value = model.Plate;
                parameters[2].Value = model.WorkID;
                parameters[3].Value = model.WorkName;
                parameters[4].Value = model.BranchID;
                parameters[5].Value = model.SMSContent;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 以手机为准，防止一用户多辆车提醒多次。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static Models.NoticeLogs GetNoticeByMobile(string mobile)
        {
            try
            {
                string sql = "select top 1 [ID],[Mobile],[Plate],[CDate] from [NoticeLogs] where [Mobile]=@Mobile order by [ID] desc";
                SqlParameter[] param = { new SqlParameter("@Mobile", mobile) };
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, sql, param);
                if (reader.Read())
                {
                    Models.NoticeLogs model = new Models.NoticeLogs();
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                    model.ID = AliceDAL.DataType.ConvertToInt(reader["ID"].ToString());
                    model.Mobile = reader["Mobile"].ToString();
                    model.Plate = reader["Plate"].ToString();
                    reader.Close();
                    return model;
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
    }
}

