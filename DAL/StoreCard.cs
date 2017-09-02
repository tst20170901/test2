using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class StoreCard
    {
        public static string Add(Models.StoreCard model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [StoreCard] where [CardNo]=@CardNo) " +
                              "begin " +
                                "select 'exists no';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  insert into [StoreCard] (");
                strSql.Append("Data1,Data2,Data3,Data4,Data5,Data6,Data7,Data8,Data9,Data10,[CardNo],[CardPwd],[Title],[Price],[BusinessID],[CardState],[SMSContent]");
                strSql.Append(") values (");
                strSql.Append("@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10,@CardNo,@CardPwd,@Title,@Price,@BusinessID,@CardState,@SMSContent");
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
                                                new SqlParameter("@Data10", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal,9), 
                                                new SqlParameter("@BusinessID", SqlDbType.Int,4), 
                                                new SqlParameter("@CardState", SqlDbType.TinyInt,1), 
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
                parameters[9].Value = model.Data10;
                parameters[10].Value = model.CardNo;
                parameters[11].Value = model.CardPwd;
                parameters[12].Value = model.Title;
                parameters[13].Value = model.Price;
                parameters[14].Value = model.BusinessID;
                parameters[15].Value = model.CardState;
                parameters[16].Value = model.SMSContent;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static string Edit(Models.StoreCard model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [StoreCard] where [CardNo]=@CardNo and [ID]<>@ID) " +
                              "begin " +
                                "select 'exists no';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  update [StoreCard] set [Data1]=@Data1,[Data2]=@Data2,[Data3]=@Data3,[Data4]=@Data4,[Data5]=@Data5,[Data6]=@Data6,[Data7]=@Data7,[Data8]=@Data8,[Data9]=@Data9,[Data10]=@Data10,[CardNo]=@CardNo,[CardPwd]=@CardPwd,[Title]=@Title,[Price]=@Price,[SMSContent]=@SMSContent where [ID]=@ID;");
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
                                                new SqlParameter("@Data10", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@ID", SqlDbType.Int,4)
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
                parameters[9].Value = model.Data10;
                parameters[10].Value = model.CardNo;
                parameters[11].Value = model.CardPwd;
                parameters[12].Value = model.Title;
                parameters[13].Value = model.Price;
                parameters[14].Value = model.SMSContent;
                parameters[15].Value = model.ID;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static bool AddList(List<Models.StoreCard> list)
        {
            try
            {
                List<SQLHelper.TransactionModel> listtm = new List<SQLHelper.TransactionModel>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [StoreCard] where [CardNo]=@CardNo) " +
                              "begin " +
                                "select '2';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  insert into [StoreCard] (");
                strSql.Append("Data1,Data2,Data3,Data4,Data5,Data6,Data7,Data8,Data9,Data10,[CardNo],[CardPwd],[Title],[Price],[BusinessID],[CardState],[SMSContent]");
                strSql.Append(") values (");
                strSql.Append("@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10,@CardNo,@CardPwd,@Title,@Price,@BusinessID,@CardState,@SMSContent");
                strSql.Append("); select '1'; end");


                foreach (Models.StoreCard model in list)
                {
                    SqlParameter[] parameters = { new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1), new SqlParameter("@CardNo", SqlDbType.NVarChar, 50), new SqlParameter("@CardPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Title", SqlDbType.NVarChar, 50), new SqlParameter("@Price", SqlDbType.Decimal, 9), new SqlParameter("@BusinessID", SqlDbType.Int, 4), new SqlParameter("@CardState", SqlDbType.TinyInt, 1), new SqlParameter("@SMSContent", SqlDbType.NVarChar, -1) };
                    parameters[0].Value = model.Data1;
                    parameters[1].Value = model.Data2;
                    parameters[2].Value = model.Data3;
                    parameters[3].Value = model.Data4;
                    parameters[4].Value = model.Data5;
                    parameters[5].Value = model.Data6;
                    parameters[6].Value = model.Data7;
                    parameters[7].Value = model.Data8;
                    parameters[8].Value = model.Data9;
                    parameters[9].Value = model.Data10;
                    parameters[10].Value = model.CardNo;
                    parameters[11].Value = model.CardPwd;
                    parameters[12].Value = model.Title;
                    parameters[13].Value = model.Price;
                    parameters[14].Value = model.BusinessID;
                    parameters[15].Value = model.CardState;
                    parameters[16].Value = model.SMSContent;
                    SQLHelper.TransactionModel tm = new SQLHelper.TransactionModel();
                    tm.Params = parameters;
                    tm.StoredProcedureName = strSql.ToString();
                    tm.Result = "1";
                    listtm.Add(tm);

                }
                return SQLHelper.ExecuteTransactionSQL(listtm);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
        public static List<Models.StoreCard> GetList()
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[StoreCard]", "", "ID", "asc", 0);
                List<Models.StoreCard> list = new List<Models.StoreCard>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.StoreCard m = new Models.StoreCard();
                        m.BusinessID = DataType.ConvertToInt(row["BusinessID"].ToString());
                        m.CardNo = row["CardNo"].ToString();
                        m.CardPwd = row["CardPwd"].ToString();
                        m.CardState = DataType.ConvertToInt(row["CardState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
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
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.SMSContent = row["SMSContent"].ToString();
                        m.Title = row["Title"].ToString();
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
        public static Models.StoreCard GetModel(int sid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[StoreCard]", String.Format("ID={0}", sid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.StoreCard m = new Models.StoreCard();
                m.BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString());
                m.CardNo = dt.Rows[0]["CardNo"].ToString();
                m.CardPwd = dt.Rows[0]["CardPwd"].ToString();
                m.CardState = DataType.ConvertToInt(dt.Rows[0]["CardState"].ToString());
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
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                m.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                m.Title = dt.Rows[0]["Title"].ToString();
                return m;
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [StoreCard] set [CardState]=@CardState where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@CardState", SqlDbType.TinyInt, 1),
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

        public static Models.StoreCard Exchange(string cardno, string cardpwd, out string result)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                string str = "select * from StoreCard where [CardNo]=@CardNo;";
                param[0] = param[0] = new SqlParameter("@CardNo", cardno);

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (cardpwd == reader["CardPwd"].ToString())
                    {
                        Models.StoreCard m = new Models.StoreCard();
                        m.BusinessID = DataType.ConvertToInt(reader["BusinessID"].ToString());
                        m.CardNo = reader["CardNo"].ToString();
                        m.CardPwd = reader["CardPwd"].ToString();
                        m.CardState = DataType.ConvertToInt(reader["CardState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        m.Data1 = reader["Data1"].ToString();
                        m.Data2 = reader["Data2"].ToString();
                        m.Data3 = reader["Data3"].ToString();
                        m.Data4 = reader["Data4"].ToString();
                        m.Data5 = reader["Data5"].ToString();
                        m.Data6 = reader["Data6"].ToString();
                        m.Data7 = reader["Data7"].ToString();
                        m.Data8 = reader["Data8"].ToString();
                        m.Data9 = reader["Data9"].ToString();
                        m.Data10 = reader["Data10"].ToString();
                        m.ID = DataType.ConvertToInt(reader["ID"].ToString());
                        m.Price = DataType.ConvertToDecimal(reader["Price"].ToString());
                        m.SMSContent = reader["SMSContent"].ToString();
                        m.Title = reader["Title"].ToString();
                        result = "";
                        reader.Close();
                        return m;
                    }
                    else result = "密码不正确！";
                }
                else result = "充值卡号不存在！";
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                result = "系统异常！";
                return null;
            }
        }
    }
}

