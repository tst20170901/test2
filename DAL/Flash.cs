using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    //Flash
    public partial class Flash
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.Flash model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Flash(Img,Title,Url,SortID,TypeID) values (@Img,@Title,@Url,@SortID,@TypeID)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = { new SqlParameter("@Img", SqlDbType.NVarChar, 500), new SqlParameter("@Title", SqlDbType.NVarChar, 500), new SqlParameter("@Url", SqlDbType.NVarChar, 500), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@TypeID", SqlDbType.Int, 4) };
                parameters[0].Value = model.Img;
                parameters[1].Value = model.Title;
                parameters[2].Value = model.Url;
                parameters[3].Value = model.SortID;
                parameters[4].Value = model.TypeID;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);

                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Edit(Models.Flash model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Flash set ");

                strSql.Append("Img = @Img , ");
                strSql.Append("Title = @Title , ");
                strSql.Append("Url = @Url , ");
                strSql.Append("SortID = @SortID , ");
                strSql.Append("TypeID = @TypeID  ");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@Img", SqlDbType.NVarChar, 500), new SqlParameter("@Title", SqlDbType.NVarChar, 500), new SqlParameter("@Url", SqlDbType.NVarChar, 500), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@TypeID", SqlDbType.Int, 4) };

                parameters[0].Value = model.ID;
                parameters[1].Value = model.Img;
                parameters[2].Value = model.Title;
                parameters[3].Value = model.Url;
                parameters[4].Value = model.SortID;
                parameters[5].Value = model.TypeID;
                int result = SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
    }
}

