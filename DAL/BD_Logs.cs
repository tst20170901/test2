using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public class BD_Logs
    {
        public static int Add(Models.BD_Logs model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into BD_Logs(LoginID,NickName,Description,IP) values (@LoginID,@NickName,@Description,@IP);");
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NChar, 20), new SqlParameter("@NickName", SqlDbType.NVarChar, 200), new SqlParameter("@Description", SqlDbType.NVarChar, -1), new SqlParameter("@IP", SqlDbType.VarChar, 15) };
                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.NickName;
                parameters[2].Value = model.Description;
                parameters[3].Value = model.IP;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);

            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
    }
}

