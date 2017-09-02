using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class UserOrderActions
    {
        public static int Add(Models.UserOrderActions model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [UserOrderActions] ([Oid],[POid],[Uid],[RealName],[ActionType],[ActionDes]) values (@Oid,@POid,@Uid,@RealName,@ActionType,@ActionDes);");
                SqlParameter[] parameters = { new SqlParameter("@Oid", SqlDbType.Int, 4), new SqlParameter("@Uid", SqlDbType.Int, 4), new SqlParameter("@RealName", SqlDbType.NVarChar, 100), new SqlParameter("@ActionType", SqlDbType.NVarChar, 100), new SqlParameter("@ActionDes", SqlDbType.NVarChar, 250), new SqlParameter("@POid", SqlDbType.Int, 4) };
                parameters[0].Value = model.Oid;
                parameters[1].Value = model.Uid;
                parameters[2].Value = model.RealName;
                parameters[3].Value = model.ActionType;
                parameters[4].Value = model.ActionDes;
                parameters[5].Value = model.POid;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }

        public static List<Models.UserOrderActions> GetListByOrderID(int orderID, int pid)
        {
            try
            {
                string sql = ""; SqlParameter[] para = { new SqlParameter("@Oid", SqlDbType.Int, 4) };
                if (orderID > 0)
                {
                    sql = "select * from [UserOrderActions] where [Oid]=@Oid order by [ActionTime] asc";
                    para[0].Value = orderID;
                }
                else
                {
                    sql = "select * from [UserOrderActions] where [POid]=@Oid order by [ActionTime] asc";
                    para[0].Value = pid;
                }
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.UserOrderActions> list = new List<Models.UserOrderActions>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.UserOrderActions model = new Models.UserOrderActions();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.ActionDes = item["ActionDes"].ToString();
                        model.ActionTime = AliceDAL.DataType.ConvertToDateTime(item["ActionTime"].ToString());
                        model.ActionType = item["ActionType"].ToString();
                        model.Oid = AliceDAL.DataType.ConvertToInt(item["Oid"].ToString());
                        model.RealName = item["RealName"].ToString();
                        model.Uid = AliceDAL.DataType.ConvertToInt(item["Uid"].ToString());
                        model.POid = AliceDAL.DataType.ConvertToInt(item["POid"].ToString());
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

