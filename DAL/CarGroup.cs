using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class CarGroup
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static string Add(Models.CarGroup model)
        {
            try
            {
                string sql = "if exists(select 1 from CarGroup where TypeName=@TypeName and ParentID=@ParentID and UserID=@UserID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into CarGroup (TypeName,ParentID,SortID,UserID) values (@TypeName,@ParentID,@SortID,@UserID);" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@UserID", SqlDbType.Int, 4) };
                parameters[0].Value = model.TypeName;
                parameters[1].Value = model.ParentID;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.UserID;
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
        public static string Edit(Models.CarGroup model)
        {
            try
            {
                string sql = "if exists(select 1 from CarGroup where TypeName=@TypeName and UserID=@UserID and ID<>@ID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update CarGroup set TypeName=@TypeName,SortID=@SortID where ID=@ID;" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@UserID", SqlDbType.Int, 4) };

                parameters[0].Value = model.ID;
                parameters[1].Value = model.TypeName;
                parameters[2].Value = model.ParentID;
                parameters[3].Value = model.SortID;
                parameters[4].Value = model.UserID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        public static bool Delete(int ID)
        {
            try
            {
                string sql = "delete from CarGroup where ID=@ID";
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

