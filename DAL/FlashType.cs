using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    //FlashType
    public partial class FlashType
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Models.FlashType model)
        {
            try
            {
                string sql = "if exists(select 1 from FlashType where TypeName=@TypeName and ParentID=@ParentID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into FlashType (TypeName,ParentID,SortID) values (@TypeName,@ParentID,@SortID);" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4) };
                parameters[0].Value = model.TypeName;
                parameters[1].Value = model.ParentID;
                parameters[2].Value = model.SortID;
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public string Edit(Models.FlashType model)
        {
            try
            {
                string sql = "if exists(select 1 from FlashType where TypeName=@TypeName and ID<>@ID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update FlashType set TypeName=@TypeName,SortID=@SortID where ID=@ID;" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4) };

                parameters[0].Value = model.ID;
                parameters[1].Value = model.TypeName;
                parameters[2].Value = model.ParentID;
                parameters[3].Value = model.SortID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        public bool Delete(int ID)
        {
            try
            {
                string sql = "delete from FlashType where ID=@ID or ParentID=@ID;delete from Flash where TypeID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = ID;
                return SQLHelper.ExecuteTransactionSQL(sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
    }
}

