using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class VipCardOrdersActions
    {
        public static int Add(Models.VipCardOrdersActions model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [VipCardOrdersActions] ([Oid],[Uid],[RealName],[ActionType],[ActionDes]) values (@Oid,@Uid,@RealName,@ActionType,@ActionDes);");
                SqlParameter[] parameters = { new SqlParameter("@Oid", SqlDbType.Int, 4), new SqlParameter("@Uid", SqlDbType.Int, 4), new SqlParameter("@RealName", SqlDbType.NVarChar, 100), new SqlParameter("@ActionType", SqlDbType.NVarChar, 10), new SqlParameter("@ActionDes", SqlDbType.NVarChar, 250) };
                parameters[0].Value = model.Oid;
                parameters[1].Value = model.Uid;
                parameters[2].Value = model.RealName;
                parameters[3].Value = model.ActionType;
                parameters[4].Value = model.ActionDes;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static List<Models.VipCardOrdersActions> GetListByOrderID(int orderID)
        {
            try
            {
                string sql = "select * from [UserVipCardOrdersActions] where [Oid]=@Oid order by [ActionTime] desc";
                SqlParameter[] para = { new SqlParameter("@Oid", SqlDbType.Int, 4) };
                para[0].Value = orderID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.VipCardOrdersActions> list = new List<Models.VipCardOrdersActions>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.VipCardOrdersActions model = new Models.VipCardOrdersActions();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.ActionDes = item["ActionDes"].ToString();
                        model.ActionTime = AliceDAL.DataType.ConvertToDateTime(item["ActionTime"].ToString());
                        model.ActionType = item["ActionType"].ToString();
                        model.Oid = AliceDAL.DataType.ConvertToInt(item["Oid"].ToString());
                        model.RealName = item["RealName"].ToString();
                        model.Uid = AliceDAL.DataType.ConvertToInt(item["Uid"].ToString());
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

