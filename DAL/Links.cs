using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class Links
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.Links model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Links(Title,Url,Img,LinkType,SortID) values (@Title,@Url,@Img,@LinkType,@SortID)");
                SqlParameter[] parameters = { new SqlParameter("@Title", SqlDbType.NVarChar, 50), new SqlParameter("@Url", SqlDbType.NVarChar, 500), new SqlParameter("@Img", SqlDbType.NVarChar, 500), new SqlParameter("@LinkType", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4) };
                parameters[0].Value = model.Title;
                parameters[1].Value = model.Url;
                parameters[2].Value = model.Img;
                parameters[3].Value = model.LinkType;
                parameters[4].Value = model.SortID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
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
        public bool Edit(Models.Links model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Links set ");
                strSql.Append("Title=@Title,");
                strSql.Append("Url=@Url,");
                strSql.Append("Img=@Img,");
                strSql.Append("LinkType=@LinkType,");
                strSql.Append("SortID=@SortID");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@Title", SqlDbType.NVarChar, 100), new SqlParameter("@Url", SqlDbType.NVarChar, 500), new SqlParameter("@Img", SqlDbType.NVarChar, 500), new SqlParameter("@LinkType", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4) };

                parameters[0].Value = model.ID;
                parameters[1].Value = model.Title;
                parameters[2].Value = model.Url;
                parameters[3].Value = model.Img;
                parameters[4].Value = model.LinkType;
                parameters[5].Value = model.SortID;
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

