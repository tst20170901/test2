using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AliceDAL;
namespace DAL
{
    public partial class BD_Business
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static string Add(Models.BD_Business model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_Business] where [BranchID]=@BranchID and ([LoginID]=@LoginID or [BusinessName]=@BusinessName)) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into BD_Business (LoginID,Password,BusinessName,SortID,BranchID,Data1,Data2,Data3,Data4,Data5,Data6,Data7,Data8,Data9,Data10,BusinessState,Wallet,[TypeID]) values (@LoginID,@Password,@BusinessName,@SortID,@BranchID,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10,@BusinessState,@Wallet,@TypeID);" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@BusinessName", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@SortID", SqlDbType.Int, 4), 
                                                new SqlParameter("@BranchID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Data1", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data2", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data3", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data4", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data5", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data6", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data7", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data8", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data9", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data10", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@BusinessState",SqlDbType.TinyInt,1),                                                
                                                new SqlParameter("@LoginID", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Password", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Wallet",SqlDbType.Decimal,9),
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4) 
                                            };
                parameters[0].Value = model.BusinessName;
                parameters[1].Value = model.SortID;
                parameters[2].Value = model.BranchID;
                parameters[3].Value = model.Data1;
                parameters[4].Value = model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                parameters[13].Value = model.BusinessState;
                parameters[14].Value = model.LoginID;
                parameters[15].Value = model.Password;
                parameters[16].Value = model.Wallet;
                parameters[17].Value = model.TypeID;
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
        public static string Edit(Models.BD_Business model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_Business] where [BranchID]=@BranchID and [ID]<>@ID and [BusinessName]=@BusinessName) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update [BD_Business] set [BusinessName]=@BusinessName,Password=@Password,SortID=@SortID,Data1=@Data1,Data2=@Data2,Data3=@Data3,Data4=@Data4,Data5=@Data5,Data6=@Data6,Data7=@Data7,Data8=@Data8,Data9=@Data9,Data10=@Data10,[BranchID]=@BranchID,[TypeID]=@TypeID where ID=@ID;" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@BusinessName", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@SortID", SqlDbType.Int, 4),
                                                new SqlParameter("@ID", SqlDbType.Int, 4),
                                                new SqlParameter("@Data1", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data2", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data3", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data4", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data5", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data6", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data7", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data8", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data9", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@Data10", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@BranchID", SqlDbType.Int, 4),
                                                new SqlParameter("@Password",SqlDbType.NVarChar,50),
                                                new SqlParameter("@TypeID", SqlDbType.Int, 4)
                                             };

                parameters[0].Value = model.BusinessName;
                parameters[1].Value = model.SortID;
                parameters[2].Value = model.ID;
                parameters[3].Value = model.Data1;
                parameters[4].Value = model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                parameters[13].Value = model.BranchID;
                parameters[14].Value = model.Password;
                parameters[15].Value = model.TypeID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        public static Models.BD_Business GetModel(int bid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[BD_Business]", String.Format("ID={0}", bid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.BD_Business m = new Models.BD_Business();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
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
                m.BusinessName = dt.Rows[0]["BusinessName"].ToString();
                m.SortID = DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
                m.BranchID = DataType.ConvertToInt(dt.Rows[0]["BranchID"].ToString());
                m.BusinessState = DataType.ConvertToInt(dt.Rows[0]["BusinessState"].ToString());
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.LoginID = dt.Rows[0]["LoginID"].ToString();
                m.Password = dt.Rows[0]["Password"].ToString();
                m.Wallet = DataType.ConvertToDecimal(dt.Rows[0]["Wallet"].ToString());
                m.TypeID = DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
                return m;
            }
        }
        public static List<Models.BD_Business> GetList(string where)
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[BD_Business]", where, "SortID", "desc", 0);
                List<Models.BD_Business> list = new List<Models.BD_Business>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.BD_Business m = new Models.BD_Business();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
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
                        m.BusinessName = row["BusinessName"].ToString();
                        m.SortID = DataType.ConvertToInt(row["SortID"].ToString());
                        m.BranchID = DataType.ConvertToInt(row["BranchID"].ToString());
                        m.BusinessState = DataType.ConvertToInt(row["BusinessState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                        m.LoginID = row["LoginID"].ToString();
                        m.Password = row["Password"].ToString();
                        m.Wallet = DataType.ConvertToDecimal(row["Wallet"].ToString());
                        m.TypeID = DataType.ConvertToInt(row["TypeID"].ToString());
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
        public static int EditPwd(string loginID, string password)
        {
            try
            {
                string sql = "";
                SqlParameter[] param = new SqlParameter[2];

                sql = "update BD_Business set Password=@UserPwd where LoginID=@LoginID;";
                param[0] = new SqlParameter("@LoginID", loginID);
                param[1] = new SqlParameter("@UserPwd", password);

                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [BD_Business] set [BusinessState]=@BusinessState where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@BusinessState", SqlDbType.TinyInt, 1),
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
        public static int EditWallet(int ID, decimal wallet)
        {
            try
            {
                string sql = "Update [BD_Business] set [Wallet]=[Wallet]+@Wallet where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Wallet", SqlDbType.Decimal, 9),
                                                new SqlParameter("@ID",SqlDbType.Int,4)
                                            };

                parameters[0].Value = wallet;
                parameters[1].Value = ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static Models.BD_Business GetBusinessByApp(int bid, string password)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                string str = "select * from [BD_Business] where ID=@ID;";
                param[0] = new SqlParameter("@ID", bid);

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == AliceDAL.SecureHelper.MD5(reader["Password"].ToString() + reader["ID"].ToString()))
                    {
                        Models.BD_Business m = new Models.BD_Business();
                        m.ID = DataType.ConvertToInt(reader["ID"].ToString());
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
                        m.BusinessName = reader["BusinessName"].ToString();
                        m.SortID = DataType.ConvertToInt(reader["SortID"].ToString());
                        m.BranchID = DataType.ConvertToInt(reader["BranchID"].ToString());
                        m.BusinessState = DataType.ConvertToInt(reader["BusinessState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        m.LoginID = reader["LoginID"].ToString();
                        m.Password = reader["Password"].ToString();
                        m.Wallet = DataType.ConvertToDecimal(reader["Wallet"].ToString());
                        m.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        reader.Close();
                        return m;
                    }
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.BD_Business Login(string loginID, string password, out string result)
        {
            try
            {
                string str = "select * from [BD_Business] where LoginID=@LoginID;";
                SqlParameter[] param = { new SqlParameter("@LoginID", loginID) };
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["Password"].ToString())
                    {
                        Models.BD_Business m = new Models.BD_Business();
                        m.ID = DataType.ConvertToInt(reader["ID"].ToString());
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
                        m.BusinessName = reader["BusinessName"].ToString();
                        m.SortID = DataType.ConvertToInt(reader["SortID"].ToString());
                        m.BranchID = DataType.ConvertToInt(reader["BranchID"].ToString());
                        m.BusinessState = DataType.ConvertToInt(reader["BusinessState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        m.LoginID = reader["LoginID"].ToString();
                        m.Password = reader["Password"].ToString();
                        m.Wallet = DataType.ConvertToDecimal(reader["Wallet"].ToString());
                        m.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        result = "";
                        reader.Close();
                        return m;
                    }
                    else result = "密码不正确！";

                }
                else result = "用户名不存在！";
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
        public static Models.BD_Business Login(string loginID, string password)
        {
            try
            {
                string str = "select * from [BD_Business] where LoginID=@LoginID;";
                SqlParameter[] param = { new SqlParameter("@LoginID", loginID) };
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["Password"].ToString())
                    {
                        Models.BD_Business m = new Models.BD_Business();
                        m.ID = DataType.ConvertToInt(reader["ID"].ToString());
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
                        m.BusinessName = reader["BusinessName"].ToString();
                        m.SortID = DataType.ConvertToInt(reader["SortID"].ToString());
                        m.BranchID = DataType.ConvertToInt(reader["BranchID"].ToString());
                        m.BusinessState = DataType.ConvertToInt(reader["BusinessState"].ToString());
                        m.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        m.LoginID = reader["LoginID"].ToString();
                        m.Password = reader["Password"].ToString();
                        m.Wallet = DataType.ConvertToDecimal(reader["Wallet"].ToString());
                        m.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        reader.Close();
                        return m;
                    }
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
    }
}

