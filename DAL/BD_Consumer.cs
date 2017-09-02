using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class BD_Consumer
    {
        public static int Add(Models.BD_Consumer model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [BD_Consumer] ([UserID],[Money],[OrderID],[Remark]) values (@UserID,@Money,@OrderID,@Remark);");
                SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4), new SqlParameter("@Money", SqlDbType.Decimal, 9), new SqlParameter("@OrderID", SqlDbType.Int, 4), new SqlParameter("@Remark", SqlDbType.NVarChar, 500) };
                parameters[0].Value = model.UserID;
                parameters[1].Value = model.Money;
                parameters[2].Value = model.OrderID;
                parameters[3].Value = model.Remark;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }

        public static List<Models.BD_Consumer> GetListByUserID(int userID)
        {
            try
            {
                string sql = "select * from [BD_Consumer] where [UserID]=@UserID order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
                para[0].Value = userID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.BD_Consumer> list = new List<Models.BD_Consumer>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.BD_Consumer model = new Models.BD_Consumer();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.CDate = AliceDAL.DataType.ConvertToDateTime(item["CDate"].ToString());
                        model.Money = AliceDAL.DataType.ConvertToDecimal(item["Money"].ToString());
                        model.OrderID = AliceDAL.DataType.ConvertToInt(item["OrderID"].ToString());
                        model.Remark = item["Remark"].ToString();
                        model.UserID = AliceDAL.DataType.ConvertToInt(item["UserID"].ToString());
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
    }
}

