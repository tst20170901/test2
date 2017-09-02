using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class Case
    {
        public static int Add(Models.Case model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO [Case] ([Title],[TitleSpell],[TitleWeb],[KeyWords],[Description],[Source],[Body],[TypeID],[Hot],[Author],[Img],[ProvinceName]," +
                              "[CityName],[CompanyName]) VALUES (@Title,@TitleSpell,@TitleWeb,@KeyWords,@Description,@Source,@Body,@TypeID,@Hot,@Author,@Img,@ProvinceName,@CityName,@CompanyName);");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleSpell", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Description", SqlDbType.NVarChar, 1000), 
                                                new SqlParameter("@Source", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Hot", SqlDbType.Int, 4), 
                                                new SqlParameter("@Author", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Img", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CityName", SqlDbType.NVarChar, 20), 
                                                new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.TitleSpell;
                parameters[2].Value = model.TitleWeb;
                parameters[3].Value = model.KeyWords;
                parameters[4].Value = model.Description;
                parameters[5].Value = model.Source;
                parameters[6].Value = model.Body;
                parameters[7].Value = model.TypeID;
                parameters[8].Value = model.Hot;
                parameters[9].Value = model.Author;
                parameters[10].Value = model.Img;
                parameters[11].Value = model.ProvinceName;
                parameters[12].Value = model.CityName;
                parameters[13].Value = model.CompanyName;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int Edit(Models.Case model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("UPDATE [Case] SET [Title]=@Title,[TitleSpell]=@TitleSpell,[TitleWeb]=@TitleWeb,[KeyWords]=@KeyWords,[Description]=@Description,[Source]=@Source,[Body]=@Body,[TypeID]=@TypeID,[Author]=@Author,[Img]=@Img,[ProvinceName]=@ProvinceName," +
                              "[CityName]=@CityName,[CompanyName]=@CompanyName WHERE ID=@ID;");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleSpell", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Description", SqlDbType.NVarChar, 1000), 
                                                new SqlParameter("@Source", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Author", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Img", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CityName", SqlDbType.NVarChar, 20), 
                                                new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@ID", SqlDbType.Int,4)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.TitleSpell;
                parameters[2].Value = model.TitleWeb;
                parameters[3].Value = model.KeyWords;
                parameters[4].Value = model.Description;
                parameters[5].Value = model.Source;
                parameters[6].Value = model.Body;
                parameters[7].Value = model.TypeID;
                parameters[8].Value = model.Author;
                parameters[9].Value = model.Img;
                parameters[10].Value = model.ProvinceName;
                parameters[11].Value = model.CityName;
                parameters[12].Value = model.CompanyName;
                parameters[13].Value = model.ID;
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

