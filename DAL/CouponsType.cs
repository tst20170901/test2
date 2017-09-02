using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class CouponsType
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static string Add(Models.CouponsType model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [CouponsType] where [Title]=@Title and [BusinessID]=@BusinessID and [IsSpecial]=10) " +
                              "begin " +
                                "select 'exists title';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  insert into [CouponsType] (");
                strSql.Append("Data1,Data2,Data3,Data4,Data5,Data6,Data7,Data8,Data9,Title,Data10,Price,OriginalPrice,Period,Count,BusinessID,IsSpecial,SMSContent");
                strSql.Append(") values (");
                strSql.Append("@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Title,@Data10,@Price,@OriginalPrice,@Period,@Count,@BusinessID,@IsSpecial,@SMSContent");
                strSql.Append(") ");
                strSql.Append(";select @@IDENTITY; end ");
                SqlParameter[] parameters = {
                                                new SqlParameter("@Data1", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data2", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data3", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data4", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data5", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data6", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data7", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data8", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data9", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Data10", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@OriginalPrice", SqlDbType.Decimal,9),
                                                new SqlParameter("@Period", SqlDbType.Int,4), 
                                                new SqlParameter("@Count", SqlDbType.Int,4), 
                                                new SqlParameter("@BusinessID", SqlDbType.Int,4), 
                                                new SqlParameter("@IsSpecial", SqlDbType.TinyInt,1), 
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1)
                                            };
                parameters[0].Value = model.Data1;
                parameters[1].Value = model.Data2;
                parameters[2].Value = model.Data3;
                parameters[3].Value = model.Data4;
                parameters[4].Value = model.Data5;
                parameters[5].Value = model.Data6;
                parameters[6].Value = model.Data7;
                parameters[7].Value = model.Data8;
                parameters[8].Value = model.Data9;
                parameters[9].Value = model.Title;
                parameters[10].Value = model.Data10;
                parameters[11].Value = model.Price;
                parameters[12].Value = model.OriginalPrice;
                parameters[13].Value = model.Period;
                parameters[14].Value = model.Count;
                parameters[15].Value = model.BusinessID;
                parameters[16].Value = model.IsSpecial;
                parameters[17].Value = model.SMSContent;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static string Edit(Models.CouponsType model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [CouponsType] where Title=@Title and BusinessID=@BusinessID and [ID]<>@ID) " +
                              "begin " +
                                "select 'exists title';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  update [CouponsType] set [Data1]=@Data1,[Data2]=@Data2,[Data3]=@Data3,[Data4]=@Data4,[Data5]=@Data5,[Data6]=@Data6,[Data7]=@Data7,[Data8]=@Data8,[Data9]=@Data9,[Title]=@Title,[Data10]=@Data10,[Price]=@Price,[OriginalPrice]=@OriginalPrice,[Period]=@Period,[Count]=@Count,[BusinessID]=@BusinessID,[IsSpecial]=@IsSpecial,[SMSContent]=@SMSContent where [ID]=@ID; ");
                strSql.Append("select '1'; end ");
                SqlParameter[] parameters = {
                                                new SqlParameter("@Data1", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data2", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data3", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data4", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data5", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data6", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data7", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data8", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Data9", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Data10", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@OriginalPrice", SqlDbType.Decimal,9),
                                                new SqlParameter("@Period", SqlDbType.Int,4), 
                                                new SqlParameter("@Count", SqlDbType.Int,4), 
                                                new SqlParameter("@BusinessID", SqlDbType.Int,4), 
                                                new SqlParameter("@IsSpecial", SqlDbType.TinyInt,1), 
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@ID",SqlDbType.Int,4)
                                            };
                parameters[0].Value = model.Data1;
                parameters[1].Value = model.Data2;
                parameters[2].Value = model.Data3;
                parameters[3].Value = model.Data4;
                parameters[4].Value = model.Data5;
                parameters[5].Value = model.Data6;
                parameters[6].Value = model.Data7;
                parameters[7].Value = model.Data8;
                parameters[8].Value = model.Data9;
                parameters[9].Value = model.Title;
                parameters[10].Value = model.Data10;
                parameters[11].Value = model.Price;
                parameters[12].Value = model.OriginalPrice;
                parameters[13].Value = model.Period;
                parameters[14].Value = model.Count;
                parameters[15].Value = model.BusinessID;
                parameters[16].Value = model.IsSpecial;
                parameters[17].Value = model.SMSContent;
                parameters[18].Value = model.ID;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [CouponsType] set [IsSpecial]=@IsSpecial where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@IsSpecial", SqlDbType.TinyInt, 1),
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
        public static List<Models.CouponsType> GetList()
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[CouponsType]", "", "ID", "asc", 0);
                List<Models.CouponsType> list = new List<Models.CouponsType>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.CouponsType m = new Models.CouponsType();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.IsSpecial = DataType.ConvertToInt(row["IsSpecial"].ToString());
                        m.OriginalPrice = DataType.ConvertToDecimal(row["OriginalPrice"].ToString());
                        m.Period = DataType.ConvertToInt(row["Period"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.Count = DataType.ConvertToInt(row["Count"].ToString());
                        m.Title = row["Title"].ToString();
                        m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                        m.Data1 = row["Data1"].ToString();
                        m.Data2 = row["Data2"].ToString();
                        m.Data3 = row["Data3"].ToString();
                        m.Data4 = row["Data4"].ToString();
                        m.Data5 = row["Data5"].ToString();
                        m.Data6 = row["Data6"].ToString();
                        m.Data7 = row["Data7"].ToString();
                        m.Data8 = row["Data8"].ToString();
                        m.Data9 = row["Data9"].ToString();
                        m.Data10 = row["Data10"].ToString();
                        m.SMSContent = row["SMSContent"].ToString();
                        list.Add(m);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.CouponsType GetModel(int cid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[CouponsType]", String.Format("ID={0}", cid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.CouponsType m = new Models.CouponsType();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.IsSpecial = DataType.ConvertToInt(dt.Rows[0]["IsSpecial"].ToString());
                m.OriginalPrice = DataType.ConvertToDecimal(dt.Rows[0]["OriginalPrice"].ToString());
                m.Period = DataType.ConvertToInt(dt.Rows[0]["Period"].ToString());
                m.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                m.Count = DataType.ConvertToInt(dt.Rows[0]["Count"].ToString());
                m.Title = dt.Rows[0]["Title"].ToString();
                m.BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString());
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.Data1 = dt.Rows[0]["Data1"].ToString();
                m.Data2 = dt.Rows[0]["Data2"].ToString();
                m.Data3 = dt.Rows[0]["Data3"].ToString();
                m.Data4 = dt.Rows[0]["Data4"].ToString();
                m.Data5 = dt.Rows[0]["Data5"].ToString();
                m.Data6 = dt.Rows[0]["Data6"].ToString();
                m.Data7 = dt.Rows[0]["Data7"].ToString();
                m.Data8 = dt.Rows[0]["Data8"].ToString();
                m.Data9 = dt.Rows[0]["Data9"].ToString();
                m.Data10 = dt.Rows[0]["Data10"].ToString();
                m.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                return m;
            }
        }
    }
}

