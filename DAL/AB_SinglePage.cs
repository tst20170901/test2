using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class AB_SinglePage
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.AB_SinglePage model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into AB_SinglePage(Title,TitleWeb,KeyWords,Description,Body,Img,SortID,Lan,TypeID) values ");
                strSql.Append("(@Title,@TitleWeb,@KeyWords,@Description,@Body,@Img,@SortID,@Lan,@TypeID);select @@IDENTITY;");
                SqlParameter[] parameters = { new SqlParameter("@Title", SqlDbType.NVarChar, 100), new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 200), new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), new SqlParameter("@Description", SqlDbType.NVarChar, 200), new SqlParameter("@Body", SqlDbType.NVarChar, -1), new SqlParameter("@Img", SqlDbType.NVarChar, -1), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@Lan", SqlDbType.Int, 4), new SqlParameter("@TypeID", SqlDbType.Int, 4) };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.TitleWeb;
                parameters[2].Value = model.KeyWords;
                parameters[3].Value = model.Description;
                parameters[4].Value = model.Body;
                parameters[5].Value = model.Img;
                parameters[6].Value = model.SortID;
                parameters[7].Value = model.Lan;
                parameters[8].Value = model.TypeID;

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
        public bool Edit(Models.AB_SinglePage model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update AB_SinglePage set Title=@Title,TitleWeb=@TitleWeb,KeyWords=@KeyWords,Description=@Description,Body=@Body,Img=@Img,SortID=@SortID,Lan=@Lan where ID=@ID");
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@Title", SqlDbType.NVarChar, 100), new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 200), new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), new SqlParameter("@Description", SqlDbType.NVarChar, 200), new SqlParameter("@Body", SqlDbType.NVarChar, -1), new SqlParameter("@Img", SqlDbType.NVarChar, -1), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@Lan", SqlDbType.Int, 4) };

                parameters[0].Value = model.ID;
                parameters[1].Value = model.Title;
                parameters[2].Value = model.TitleWeb;
                parameters[3].Value = model.KeyWords;
                parameters[4].Value = model.Description;
                parameters[5].Value = model.Body;
                parameters[6].Value = model.Img;
                parameters[7].Value = model.SortID;
                parameters[8].Value = model.Lan;
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

