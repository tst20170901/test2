using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class Pro_Case
    {
        public static int Add(Models.Pro_Case model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO [Pro_Case] ([Title],[TitleSpell],[TitleWeb],[KeyWords],[Description],[Source],[Body],[TypeID],[Hot],[Author],[Img],[Price]," +
                              "[SortID],[BranchID],[Price1],[BusinessID],[ProType]) VALUES (@Title,@TitleSpell,@TitleWeb,@KeyWords,@Description,@Source,@Body,@TypeID,@Hot,@Author,@Img,@Price," +
                              "@SortID,@BranchID,@Price1,@BusinessID,@ProType);");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleSpell", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Description", SqlDbType.NVarChar, 1000), 
                                                new SqlParameter("@Source", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Hot", SqlDbType.Int, 4), 
                                                new SqlParameter("@Author", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Img", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@SortID", SqlDbType.Int, 4), 
                                                new SqlParameter("@BranchID", SqlDbType.Int, 4),
                                                new SqlParameter("@Price1",SqlDbType.Decimal,9),
                                                new SqlParameter("@BusinessID", SqlDbType.Int, 4),
                                                new SqlParameter("@ProType", SqlDbType.Int, 4)
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
                parameters[11].Value = model.Price;
                parameters[12].Value = model.SortID;
                parameters[13].Value = model.BranchID;
                parameters[14].Value = model.Price1;
                parameters[15].Value = model.BusinessID;
                parameters[16].Value = model.ProType;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int Edit(Models.Pro_Case model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("UPDATE [Pro_Case] SET [Title]=@Title,[TitleSpell]=@TitleSpell,[TitleWeb]=@TitleWeb,[KeyWords]=@KeyWords,[Description]=@Description,[Source]=@Source,[Body]=@Body,[TypeID]=@TypeID,[Author]=@Author,[Img]=@Img,[Price]=@Price,[Price1]=@Price1," +
                              "[SortID]=@SortID,[BranchID]=@BranchID,[BusinessID]=@BusinessID,[ProType]=@ProType WHERE ID=@ID;");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleSpell", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@TitleWeb", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@KeyWords", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Description", SqlDbType.NVarChar, 1000), 
                                                new SqlParameter("@Source", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Author", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Img", SqlDbType.NVarChar, -1), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@SortID", SqlDbType.Int, 4), 
                                                new SqlParameter("@BranchID", SqlDbType.Int, 4),
                                                new SqlParameter("@ID", SqlDbType.Int,4),
                                                new SqlParameter("@Price1",SqlDbType.Decimal,9),
                                                new SqlParameter("@BusinessID", SqlDbType.Int, 4),
                                                new SqlParameter("@ProType", SqlDbType.Int, 4)
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
                parameters[10].Value = model.Price;
                parameters[11].Value = model.SortID;
                parameters[12].Value = model.BranchID;
                parameters[13].Value = model.ID;
                parameters[14].Value = model.Price1;
                parameters[15].Value = model.BusinessID;
                parameters[16].Value = model.ProType;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static Models.Pro_Case GetModel(int id)
        {
            try
            {
                Models.Pro_Case model = null;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("Select * from Pro_Case WHERE ID=@ID;");
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = id;
                IDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
                if (reader.Read())
                {
                    model = new Models.Pro_Case();
                    model.Author = reader["Author"].ToString();
                    model.Body = reader["Body"].ToString();
                    model.BranchID = AliceDAL.DataType.ConvertToInt(reader["BranchID"].ToString());
                    model.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                    model.Description = reader["Description"].ToString();
                    model.Hit = AliceDAL.DataType.ConvertToInt(reader["Hit"].ToString());
                    model.Hot = AliceDAL.DataType.ConvertToInt(reader["Hot"].ToString());
                    model.ID = AliceDAL.DataType.ConvertToInt(reader["ID"].ToString());
                    model.Img = reader["Img"].ToString();
                    model.KeyWords = reader["KeyWords"].ToString();
                    model.Price = AliceDAL.DataType.ConvertToDecimal(reader["Price"].ToString());
                    model.Price1 = AliceDAL.DataType.ConvertToDecimal(reader["Price1"].ToString());
                    model.SortID = AliceDAL.DataType.ConvertToInt(reader["SortID"].ToString());
                    model.Source = reader["Source"].ToString();
                    model.Title = reader["Title"].ToString();
                    model.TitleSpell = reader["TitleSpell"].ToString();
                    model.TitleWeb = reader["TitleWeb"].ToString();
                    model.TypeID = AliceDAL.DataType.ConvertToInt(reader["TypeID"].ToString());
                    model.ProState = AliceDAL.DataType.ConvertToInt(reader["ProState"].ToString());
                    model.BusinessID = AliceDAL.DataType.ConvertToInt(reader["BusinessID"].ToString());
                    model.ProType = AliceDAL.DataType.ConvertToInt(reader["ProType"].ToString());
                }
                reader.Close();
                return model;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [Pro_Case] set [ProState]=@ProState where [ID]=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@ProState", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@ID",SqlDbType.Int,4)
                                            };

                parameters[0].Value = state;
                parameters[1].Value = ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
    }
}

