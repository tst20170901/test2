using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class Reviews
    {
        public static string Add(Models.Reviews model)
        {
            try
            {
                string sql = "if exists (select 1 from Reviews where Oid=@Oid) " +
                             "begin " +
                               "select 'exists';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  INSERT INTO [Reviews] ([Oid],[Uid],[StoreID],[Body],[SpeedStar],[ServiceStar],[AttitudeStar],[IP]) VALUES (@Oid,@Uid,@StoreID,@Body,@SpeedStar,@ServiceStar,@AttitudeStar,@IP);" +
                             "  select 'ok'; " +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Oid", SqlDbType.Int, 4), 
                                                new SqlParameter("@Uid", SqlDbType.Int, 4), 
                                                new SqlParameter("@StoreID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@SpeedStar", SqlDbType.TinyInt,1),
                                                new SqlParameter("@ServiceStar", SqlDbType.TinyInt,1),
                                                new SqlParameter("@AttitudeStar", SqlDbType.TinyInt,1),
                                                new SqlParameter("@IP", SqlDbType.NVarChar,20)
                                            };

                parameters[0].Value = model.Oid;
                parameters[1].Value = model.Uid;
                parameters[2].Value = model.StoreID;
                parameters[3].Value = model.Body;
                parameters[4].Value = model.SpeedStar;
                parameters[5].Value = model.ServiceStar;
                parameters[6].Value = model.AttitudeStar;
                parameters[7].Value = model.IP;
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
    }
}

