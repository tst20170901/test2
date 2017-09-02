using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Art_Common
    {
        public int Add(Models.Art_Common model)
        {
            try
            {
                SqlParameter[] param = { 
                                           new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                           new SqlParameter("@TitleSpell", SqlDbType.NVarChar,100),
                                           new SqlParameter("@TitleWeb", SqlDbType.NVarChar,200),
                                           new SqlParameter("@KeyWords", SqlDbType.NVarChar,200),
                                           new SqlParameter("@Description", SqlDbType.NVarChar,200),
                                           new SqlParameter("@Source", SqlDbType.NVarChar,50),
                                           new SqlParameter("@Body", SqlDbType.NVarChar,-1), 
                                           new SqlParameter("@TypeID", SqlDbType.Int,4),
                                           new SqlParameter("@Hot", SqlDbType.Int,4), 
                                           new SqlParameter("@Author", SqlDbType.NVarChar,50), 
                                           new SqlParameter("@Img", SqlDbType.NVarChar,100),
                                           new SqlParameter("@BranchID", SqlDbType.Int,4)
                                       };
                param[0].Value = model.Title;
                param[1].Value = model.TitleSpell;
                param[2].Value = model.TitleWeb;
                param[3].Value = model.KeyWords;
                param[4].Value = model.Description;
                param[5].Value = model.Source;
                param[6].Value = model.Body;
                param[7].Value = model.TypeID;
                param[8].Value = model.Hot;
                param[9].Value = model.Author;
                param[10].Value = model.Img;
                param[11].Value = model.BranchID;
                string sql = "INSERT INTO [Art_Common] ([Title],[TitleSpell],[TitleWeb],[KeyWords],[Description],[Source],[Body],[TypeID],[Hot],[Author],[Img],[BranchID]) values (@Title,@TitleSpell,@TitleWeb,@KeyWords,@Description,@Source,@Body,@TypeID,@Hot,@Author,@Img,@BranchID)";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Edit(Models.Art_Common model)
        {
            try
            {
                SqlParameter[] param = {
                                           new SqlParameter("@ID",model.ID),
                                           new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                           new SqlParameter("@TitleSpell", SqlDbType.NVarChar,100),
                                           new SqlParameter("@TitleWeb", SqlDbType.NVarChar,200),
                                           new SqlParameter("@KeyWords", SqlDbType.NVarChar,200),
                                           new SqlParameter("@Description", SqlDbType.NVarChar,200),
                                           new SqlParameter("@Source", SqlDbType.NVarChar,50),
                                           new SqlParameter("@Body", SqlDbType.NVarChar,-1), 
                                           new SqlParameter("@TypeID", SqlDbType.Int,4),
                                           new SqlParameter("@Hot", SqlDbType.Int,4), 
                                           new SqlParameter("@Author", SqlDbType.NVarChar,50), 
                                           new SqlParameter("@Img", SqlDbType.NVarChar,100),
                                           new SqlParameter("@BranchID", SqlDbType.Int,4)
                                       };

                param[0].Value = model.ID;
                param[1].Value = model.Title;
                param[2].Value = model.TitleSpell;
                param[3].Value = model.TitleWeb;
                param[4].Value = model.KeyWords;
                param[5].Value = model.Description;
                param[6].Value = model.Source;
                param[7].Value = model.Body;
                param[8].Value = model.TypeID;
                param[9].Value = model.Hot;
                param[10].Value = model.Author;
                param[11].Value = model.Img;
                param[12].Value = model.BranchID;
                string sql = "UPDATE Art_Common SET [Title] = @Title,[TitleSpell] = @TitleSpell,[TitleWeb] = @TitleWeb,[KeyWords] = @KeyWords,[Description] = @Description,[Source] = @Source,[Body] = @Body,[TypeID] = @TypeID,[Hot] = @Hot,[Author] = @Author,[Img] = @Img,[BranchID]=@BranchID WHERE ID=@ID;";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
    }
}
