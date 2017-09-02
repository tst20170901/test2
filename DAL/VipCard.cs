using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class VipCard
    {
        public static string Add(Models.VipCard model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [VipCard] where [CardNo]=@CardNo) " +
                              "begin " +
                                "select 'exists no';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  insert into [VipCard] (");
                strSql.Append("[TypeID],[BusinessID],[CardState],[SMSContent],[CardNo],[CardPwd],[Title],[Price],[CardCount],[TDate]");
                strSql.Append(") values (");
                strSql.Append("@TypeID,@BusinessID,@CardState,@SMSContent,@CardNo,@CardPwd,@Title,@Price,@CardCount,@TDate");
                strSql.Append(") ");
                strSql.Append(";select @@IDENTITY; end ");
                SqlParameter[] parameters = {
                                                new SqlParameter("@TypeID", SqlDbType.Int,4),
                                                new SqlParameter("@BusinessID", SqlDbType.Int,4),
                                                new SqlParameter("@CardState", SqlDbType.TinyInt,1),
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@CardCount",SqlDbType.Int,4),
                                                new SqlParameter("@TDate",SqlDbType.DateTime)
                                            };
                parameters[0].Value = model.TypeID;
                parameters[1].Value = model.BusinessID;
                parameters[2].Value = model.CardState;
                parameters[3].Value = model.SMSContent;
                parameters[4].Value = model.CardNo;
                parameters[5].Value = model.CardPwd;
                parameters[6].Value = model.Title;
                parameters[7].Value = model.Price;
                parameters[8].Value = model.CardCount;
                parameters[9].Value = model.TDate;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static string AddOnline(Models.VipCard model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("insert into [VipCard] ([Uid],[Mobile],[Plate],[UseDate],[TypeID],[BusinessID],[CardState],[SMSContent],[CardNo],[CardPwd],[Title],[Price],[CardCount],[TDate]) values (@Uid,@Mobile,@Plate,@UseDate,@TypeID,@BusinessID,@CardState,@SMSContent,@CardNo,@CardPwd,@Title,@Price,@CardCount,@TDate)");
                SqlParameter[] parameters = {
                                                new SqlParameter("@TypeID", SqlDbType.Int,4),
                                                new SqlParameter("@BusinessID", SqlDbType.Int,4),
                                                new SqlParameter("@CardState", SqlDbType.TinyInt,1),
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@CardCount",SqlDbType.Int,4),
                                                new SqlParameter("@TDate",SqlDbType.DateTime),
                                                new SqlParameter("@Uid",SqlDbType.Int,4),
                                                new SqlParameter("@Mobile",SqlDbType.NVarChar,50),
                                                new SqlParameter("@Plate",SqlDbType.NVarChar,50),
                                                new SqlParameter("@UseDate",SqlDbType.DateTime)
                                            };
                parameters[0].Value = model.TypeID;
                parameters[1].Value = model.BusinessID;
                parameters[2].Value = model.CardState;
                parameters[3].Value = model.SMSContent;
                parameters[4].Value = model.CardNo;
                parameters[5].Value = model.CardPwd;
                parameters[6].Value = model.Title;
                parameters[7].Value = model.Price;
                parameters[8].Value = model.CardCount;
                parameters[9].Value = model.TDate;
                parameters[10].Value = model.Uid;
                parameters[11].Value = model.Mobile;
                parameters[12].Value = model.Plate;
                parameters[13].Value = model.UseDate;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static string Edit(Models.VipCard model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [VipCard] where [CardNo]=@CardNo and [ID]<>@ID) " +
                              "begin " +
                                "select 'exists no';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  update [VipCard] set [CardNo]=@CardNo,[CardPwd]=@CardPwd,[Title]=@Title,[Price]=@Price,[SMSContent]=@SMSContent,[TDate]=@TDate where [ID]=@ID;");
                strSql.Append("select '1'; end ");
                SqlParameter[] parameters = {
                                                new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                new SqlParameter("@TDate",SqlDbType.DateTime),
                                                new SqlParameter("@ID", SqlDbType.Int,4)
                                            };
                parameters[0].Value = model.SMSContent;
                parameters[1].Value = model.CardNo;
                parameters[2].Value = model.CardPwd;
                parameters[3].Value = model.Title;
                parameters[4].Value = model.Price;
                parameters[5].Value = model.TDate;
                parameters[6].Value = model.ID;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static bool AddList(List<Models.VipCard> list)
        {
            try
            {
                List<SQLHelper.TransactionModel> listtm = new List<SQLHelper.TransactionModel>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [VipCard] where [CardNo]=@CardNo) " +
                              "begin " +
                                "select '2';" +
                              "end " +
                              "else " +
                              "begin ");
                strSql.Append("  insert into [VipCard] (");
                strSql.Append("[TypeID],[BusinessID],[CardState],[SMSContent],[CardNo],[CardPwd],[Title],[Price],[CardCount],[TDate]");
                strSql.Append(") values (");
                strSql.Append("@TypeID,@BusinessID,@CardState,@SMSContent,@CardNo,@CardPwd,@Title,@Price,@CardCount,@TDate");
                strSql.Append("); select '1'; end");


                foreach (Models.VipCard model in list)
                {
                    SqlParameter[] parameters = { 
                                                    new SqlParameter("@TypeID", SqlDbType.Int,4),
                                                    new SqlParameter("@BusinessID", SqlDbType.Int,4),
                                                    new SqlParameter("@CardState", SqlDbType.TinyInt,1),
                                                    new SqlParameter("@SMSContent", SqlDbType.NVarChar,-1),
                                                    new SqlParameter("@CardNo", SqlDbType.NVarChar,50),
                                                    new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                                                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                                                    new SqlParameter("@Price", SqlDbType.Decimal,9),
                                                    new SqlParameter("@CardCount",SqlDbType.Int,4),
                                                    new SqlParameter("@TDate",SqlDbType.DateTime)
                                                };
                    parameters[0].Value = model.TypeID;
                    parameters[1].Value = model.BusinessID;
                    parameters[2].Value = model.CardState;
                    parameters[3].Value = model.SMSContent;
                    parameters[4].Value = model.CardNo;
                    parameters[5].Value = model.CardPwd;
                    parameters[6].Value = model.Title;
                    parameters[7].Value = model.Price;
                    parameters[8].Value = model.CardCount;
                    parameters[9].Value = model.TDate;
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
        public static Models.VipCard GetModel(int vid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[uv_VipCard]", String.Format("ID={0}", vid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.VipCard m = new Models.VipCard();
                m.BusinessID = DataType.ConvertToInt(dt.Rows[0]["BusinessID"].ToString());
                m.CardCount = DataType.ConvertToInt(dt.Rows[0]["CardCount"].ToString());
                m.CardNo = dt.Rows[0]["CardNo"].ToString();
                m.CardPwd = dt.Rows[0]["CardPwd"].ToString();
                m.CardState = DataType.ConvertToInt(dt.Rows[0]["CardState"].ToString());
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.Mobile = dt.Rows[0]["Mobile"].ToString();
                m.Plate = dt.Rows[0]["Plate"].ToString();
                m.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                m.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                m.TDate = DataType.ConvertToDateTime(dt.Rows[0]["TDate"].ToString());
                m.Title = dt.Rows[0]["Title"].ToString();
                m.TypeID = DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
                m.Uid = DataType.ConvertToInt(dt.Rows[0]["Uid"].ToString());
                m.UseDate = DataType.ConvertToDateTime(dt.Rows[0]["UseDate"].ToString() == "" ? "1900/01/01" : dt.Rows[0]["UseDate"].ToString());
                m.UserCount = DataType.ConvertToInt(dt.Rows[0]["UserCount"].ToString());
                m.FreeID = DataType.ConvertToInt(dt.Rows[0]["FreeID"].ToString());
                m.ItemPrice = DataType.ConvertToDecimal(dt.Rows[0]["ItemPrice"].ToString());
                m.ItemTitle = dt.Rows[0]["ItemTitle"].ToString();
                m.BusinessName = dt.Rows[0]["BusinessName"].ToString();
                m.VipDes = dt.Rows[0]["VipDes"].ToString();
                return m;
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [VipCard] set [CardState]=@CardState where ID=@ID;";

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
        public static int ChargeToUser(int ID, int userID, string mobile, string plate, DateTime newTDate)
        {
            try
            {
                string sql = "Update [VipCard] set [CardState]=30,[Uid]=@UserID,[Mobile]=@Mobile,[Plate]=@Plate,[UseDate]=getdate(),[TDate]=@TDate where ID=@ID;";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@UserID", SqlDbType.Int, 4),
                                                new SqlParameter("@Mobile",SqlDbType.NVarChar,50),
                                                new SqlParameter("@Plate",SqlDbType.NVarChar,10),
                                                new SqlParameter("@ID",SqlDbType.Int,4),
                                                new SqlParameter("@TDate",SqlDbType.DateTime)
                                            };
                parameters[0].Value = userID;
                parameters[1].Value = mobile;
                parameters[2].Value = plate;
                parameters[3].Value = ID;
                parameters[4].Value = newTDate;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static Models.VipCard Exchange(string cardno, string cardpwd, out string result)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                string str = "select * from [VipCard] where [CardNo]=@CardNo;";
                param[0] = param[0] = new SqlParameter("@CardNo", cardno);

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (cardpwd == reader["CardPwd"].ToString())
                    {
                        Models.VipCard m = new Models.VipCard();
                        m.BusinessID = DataType.ConvertToInt(reader["BusinessID"].ToString());
                        m.CardCount = DataType.ConvertToInt(reader["CardCount"].ToString());
                        m.CardNo = reader["CardNo"].ToString();
                        m.CardPwd = reader["CardPwd"].ToString();
                        m.CardState = DataType.ConvertToInt(reader["CardState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        m.ID = DataType.ConvertToInt(reader["ID"].ToString());
                        m.Mobile = reader["Mobile"].ToString();
                        m.Plate = reader["Plate"].ToString();
                        m.Price = DataType.ConvertToDecimal(reader["Price"].ToString());
                        m.SMSContent = reader["SMSContent"].ToString();
                        m.TDate = DataType.ConvertToDateTime(reader["TDate"].ToString());
                        m.Title = reader["Title"].ToString();
                        m.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        m.Uid = DataType.ConvertToInt(reader["Uid"].ToString());
                        m.UseDate = DataType.ConvertToDateTime(reader["UseDate"].ToString() == "" ? "1900/01/01" : reader["UseDate"].ToString());
                        m.UserCount = DataType.ConvertToInt(reader["UserCount"].ToString());
                        result = "";
                        reader.Close();
                        return m;
                    }
                    else result = "密码不正确！";
                }
                else result = "卡号不存在！";
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

