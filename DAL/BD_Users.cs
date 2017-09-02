using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    //BD_Users
    public partial class BD_Users
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static string Add(Models.BD_Users model)
        {
            try
            {
                string sql = "if exists (select 1 from BD_Users where LoginID=@LoginID) " +
                             "begin " +
                               "select 'exists loginid';" +
                             "end " +
                             "else if exists (select 1 from BD_Users where Mobile=@Mobile) " +
                             "begin " +
                                "select 'exists mobile';" +
                             "end " +
                             "else " +
                             "begin " +
                               "DECLARE @oid int;INSERT INTO BD_Users ([LoginID],[UserPwd],[Mobile],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10],[BranchID])VALUES(@LoginID,@UserPwd,@Mobile,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10,@BranchID);" +
                               "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50), new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1), new SqlParameter("@BranchID", SqlDbType.Int, 4) };

                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.UserPwd;
                parameters[2].Value = model.Mobile;
                parameters[3].Value = model.Data1;
                parameters[4].Value = String.IsNullOrEmpty(model.Data2) ? "1" : model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                parameters[13].Value = model.BranchID;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                if (AliceDAL.DataType.ConvertToInt(obj.ToString()) > 0)
                {
                    string sql1 = "INSERT INTO [BD_Wallet] ([UserID],[Money1],[Money2]) VALUES (@UserID,0,0)";
                    SqlParameter[] p = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
                    p[0].Value = obj;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, sql1, p);
                }
                return obj.ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static void EditCode(Models.BD_Users model)
        {
            try
            {
                string sql = "if exists (select 1 from BD_Users where LoginID=@LoginID) " +
                             "begin " +
                               "update BD_Users set Data10=@Data10 where LoginID=@LoginID;" +
                             "end " +
                             "else if exists (select 1 from BD_Users where Mobile=@Mobile) " +
                             "begin " +
                                "update BD_Users set Data10=@Data10 where Mobile=@Mobile;" +
                             "end " +
                             "else " +
                             "begin " +
                               "DECLARE @oid int;INSERT INTO BD_Users ([LoginID],[UserPwd],[Mobile],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10])VALUES(@LoginID,@UserPwd,@Mobile,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10);" +
                               "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50), new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1) };

                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.UserPwd;
                parameters[2].Value = model.Mobile;
                parameters[3].Value = model.Data1;
                parameters[4].Value = String.IsNullOrEmpty(model.Data2) ? "1" : model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                if (AliceDAL.DataType.ConvertToInt(obj == null ? "0" : obj.ToString()) > 0)
                {
                    string sql1 = "INSERT INTO [BD_Wallet] ([UserID],[Money1],[Money2]) VALUES (@UserID,0,0)";
                    SqlParameter[] p = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
                    p[0].Value = obj;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, sql1, p);
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static int GiveMoney(Models.BD_Users model, decimal money)
        {
            try
            {
                string sql = "declare @oid int;if exists (select 1 from BD_Users where LoginID=@LoginID) " +
                             "begin " +
                               "select @oid=ID from BD_Users where LoginID=@LoginID;" +
                               "UPDATE BD_Wallet set Money1=Money1+@Money1 where UserID=@oid;" +
                               "SELECT @oid AS 'oid';" +
                             "end " +
                             "else if exists (select 1 from BD_Users where Mobile=@Mobile) " +
                             "begin " +
                                "select @oid=ID from BD_Users where Mobile=@Mobile;" +
                                "UPDATE BD_Wallet set Money1=Money1+@Money1 where UserID=@oid;" +
                                "SELECT @oid AS 'oid';" +
                             "end " +
                             "else " +
                             "begin " +
                               "INSERT INTO BD_Users ([LoginID],[UserPwd],[Mobile],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10])VALUES(@LoginID,@UserPwd,@Mobile,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10);" +
                               "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                               "INSERT INTO [BD_Wallet] ([UserID],[Money1],[Money2]) VALUES (@oid,@Money1,0);" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50), new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1), new SqlParameter("@Money1", SqlDbType.Decimal, 9) };
                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.UserPwd;
                parameters[2].Value = model.Mobile;
                parameters[3].Value = model.Data1;
                parameters[4].Value = String.IsNullOrEmpty(model.Data2) ? "1" : model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                parameters[13].Value = money;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                return AliceDAL.DataType.ConvertToInt(obj.ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static int GiveMoneyUser(Models.BD_Users model, decimal money, decimal money1)
        {
            try
            {
                string sql = "declare @oid int;if exists (select 1 from BD_Users where LoginID=@LoginID) " +
                             "begin " +
                               "select @oid=ID from BD_Users where LoginID=@LoginID;" +
                               "UPDATE BD_Wallet set Money1=Money1+@Money1,Money2=Money2+@Money2 where UserID=@oid;" +
                               "SELECT @oid AS 'oid';" +
                             "end " +
                             "else if exists (select 1 from BD_Users where Mobile=@Mobile) " +
                             "begin " +
                                "select @oid=ID from BD_Users where Mobile=@Mobile;" +
                                "UPDATE BD_Wallet set Money1=Money1+@Money1,Money2=Money2+@Money2 where UserID=@oid;" +
                                "SELECT @oid AS 'oid';" +
                             "end " +
                             "else " +
                             "begin " +
                               "INSERT INTO BD_Users ([LoginID],[UserPwd],[Mobile],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10])VALUES(@LoginID,@UserPwd,@Mobile,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10);" +
                               "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                               "INSERT INTO [BD_Wallet] ([UserID],[Money1],[Money2]) VALUES (@oid,@Money1,@Money2);" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50), new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1), new SqlParameter("@Money1", SqlDbType.Decimal, 9), new SqlParameter("@Money2", SqlDbType.Decimal, 9) };

                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.UserPwd;
                parameters[2].Value = model.Mobile;
                parameters[3].Value = model.Data1;
                parameters[4].Value = String.IsNullOrEmpty(model.Data2) ? "1" : model.Data2;
                parameters[5].Value = model.Data3;
                parameters[6].Value = model.Data4;
                parameters[7].Value = model.Data5;
                parameters[8].Value = model.Data6;
                parameters[9].Value = model.Data7;
                parameters[10].Value = model.Data8;
                parameters[11].Value = model.Data9;
                parameters[12].Value = model.Data10;
                parameters[13].Value = money;
                parameters[14].Value = money1;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                return AliceDAL.DataType.ConvertToInt(obj.ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static int EditPwd(string loginID, string password, string t = "l")
        {
            try
            {
                string sql = "";
                SqlParameter[] param = new SqlParameter[2];
                if (t == "l")
                {
                    sql = "update BD_Users set UserPwd=@UserPwd where LoginID=@LoginID;";
                    param[0] = new SqlParameter("@LoginID", loginID);
                    param[1] = new SqlParameter("@UserPwd", password);
                }
                else
                {
                    sql = "update BD_Users set UserPwd=@UserPwd where Mobile=@Mobile;";
                    param[0] = new SqlParameter("@Mobile", loginID);
                    param[1] = new SqlParameter("@UserPwd", password);
                }

                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public Models.BD_Users Login(string loginID, string password, out string result, string t = "l")
        {
            try
            {
                string str = "";
                SqlParameter[] param = new SqlParameter[1];
                if (t == "l")
                {
                    str = "select * from BD_Users where LoginID=@LoginID;";
                    param[0] = param[0] = new SqlParameter("@LoginID", loginID);
                }
                else
                {
                    str = "select * from BD_Users where Mobile=@Mobile;";
                    param[0] = new SqlParameter("@Mobile", loginID);
                }
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["UserPwd"].ToString())
                    {
                        Models.BD_Users admin = new Models.BD_Users();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.UserPwd = reader["UserPwd"].ToString();
                        admin.Mobile = reader["Mobile"].ToString();
                        admin.Data1 = reader["Data1"].ToString();
                        admin.Data2 = reader["Data2"].ToString();
                        admin.Data3 = reader["Data3"].ToString();
                        admin.Data4 = reader["Data4"].ToString();
                        admin.Data5 = reader["Data5"].ToString();
                        admin.Data6 = reader["Data6"].ToString();
                        admin.Data7 = reader["Data7"].ToString();
                        admin.Data8 = reader["Data8"].ToString();
                        admin.Data9 = reader["Data9"].ToString();
                        admin.Data10 = reader["Data10"].ToString();
                        admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                        admin.BranchID = AliceDAL.DataType.ConvertToInt(reader["BranchID"].ToString());
                        result = "";
                        reader.Close();
                        return admin;
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
        public Models.BD_Users LoginCode(string loginID, string code, out string result)
        {
            try
            {
                string str = "select * from BD_Users where LoginID=@LoginID;";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@LoginID", loginID);
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    //角色 1是普通客户 2是大客户，只能清洗限定车牌号
                    if (code == reader["Data10"].ToString().TrimEnd() || (AliceDAL.SecureHelper.MD5(code) == reader["UserPwd"].ToString() && reader["Data2"].ToString().TrimEnd() == "2"))
                    {
                        Models.BD_Users admin = new Models.BD_Users();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.UserPwd = reader["UserPwd"].ToString();
                        admin.Mobile = reader["Mobile"].ToString();
                        admin.Data1 = reader["Data1"].ToString();
                        admin.Data2 = reader["Data2"].ToString();
                        admin.Data3 = reader["Data3"].ToString();
                        admin.Data4 = reader["Data4"].ToString();
                        admin.Data5 = reader["Data5"].ToString();
                        admin.Data6 = reader["Data6"].ToString();
                        admin.Data7 = reader["Data7"].ToString();
                        admin.Data8 = reader["Data8"].ToString();
                        admin.Data9 = reader["Data9"].ToString();
                        admin.Data10 = reader["Data10"].ToString();
                        admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                        reader.Close();
                        result = "";
                        return admin;
                    }
                    else result = "验证码不正确！";
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
        public static Models.BD_Users GetUserByApp(int Uid, string password)
        {
            try
            {
                string str = "select * from BD_Users where ID=@ID;";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", Uid);
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == AliceDAL.SecureHelper.MD5(reader["UserPwd"].ToString() + reader["ID"].ToString() + reader["Data10"].ToString().TrimEnd()))
                    {
                        Models.BD_Users admin = new Models.BD_Users();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.UserPwd = reader["UserPwd"].ToString();
                        admin.Mobile = reader["Mobile"].ToString();
                        admin.Data1 = reader["Data1"].ToString();
                        admin.Data2 = reader["Data2"].ToString();
                        admin.Data3 = reader["Data3"].ToString();
                        admin.Data4 = reader["Data4"].ToString();
                        admin.Data5 = reader["Data5"].ToString();
                        admin.Data6 = reader["Data6"].ToString();
                        admin.Data7 = reader["Data7"].ToString();
                        admin.Data8 = reader["Data8"].ToString();
                        admin.Data9 = reader["Data9"].ToString();
                        admin.Data10 = reader["Data10"].ToString();
                        admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                        reader.Close();
                        return admin;
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
        public static Models.BD_Users GetUserByIDPassword(int Uid, string password)
        {
            try
            {
                string str = "select * from BD_Users where ID=@ID;";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", Uid);
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["UserPwd"].ToString())
                    {
                        Models.BD_Users admin = new Models.BD_Users();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.UserPwd = reader["UserPwd"].ToString();
                        admin.Mobile = reader["Mobile"].ToString();
                        admin.Data1 = reader["Data1"].ToString();
                        admin.Data2 = reader["Data2"].ToString();
                        admin.Data3 = reader["Data3"].ToString();
                        admin.Data4 = reader["Data4"].ToString();
                        admin.Data5 = reader["Data5"].ToString();
                        admin.Data6 = reader["Data6"].ToString();
                        admin.Data7 = reader["Data7"].ToString();
                        admin.Data8 = reader["Data8"].ToString();
                        admin.Data9 = reader["Data9"].ToString();
                        admin.Data10 = reader["Data10"].ToString();
                        admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                        reader.Close();
                        return admin;
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
        public static Models.BD_Users GetUserInfoByID(int userID)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID) };
                string str = "select * from BD_Users where ID=@ID;";

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    Models.BD_Users admin = new Models.BD_Users();
                    admin.ID = Convert.ToInt32(reader["ID"].ToString());
                    admin.LoginID = reader["LoginID"].ToString();
                    admin.UserPwd = reader["UserPwd"].ToString();
                    admin.Mobile = reader["Mobile"].ToString();
                    admin.Data1 = reader["Data1"].ToString();
                    admin.Data2 = reader["Data2"].ToString();
                    admin.Data3 = reader["Data3"].ToString();
                    admin.Data4 = reader["Data4"].ToString();
                    admin.Data5 = reader["Data5"].ToString();
                    admin.Data6 = reader["Data6"].ToString();
                    admin.Data7 = reader["Data7"].ToString();
                    admin.Data8 = reader["Data8"].ToString();
                    admin.Data9 = reader["Data9"].ToString();
                    admin.Data10 = reader["Data10"].ToString();
                    admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                    admin.IsCheck = AliceDAL.DataType.ConvertToInt(reader["IsCheck"].ToString());
                    reader.Close();
                    return admin;
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
        public static int ChangeUserIsCheck(int uid, int ischeck)
        {
            try
            {
                string sql = "UPDATE [BD_Users] SET [IsCheck]=@IsCheck WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@IsCheck", SqlDbType.Int, 4), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = ischeck;
                parameters[1].Value = uid;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static Models.BD_Users GetUserInfoByMobile(string mobile)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@Mobile", mobile) };
                string str = "select * from BD_Users where Mobile=@Mobile;";

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    Models.BD_Users admin = new Models.BD_Users();
                    admin.ID = Convert.ToInt32(reader["ID"].ToString());
                    admin.LoginID = reader["LoginID"].ToString();
                    admin.UserPwd = reader["UserPwd"].ToString();
                    admin.Mobile = reader["Mobile"].ToString();
                    admin.Data1 = reader["Data1"].ToString();
                    admin.Data2 = reader["Data2"].ToString();
                    admin.Data3 = reader["Data3"].ToString();
                    admin.Data4 = reader["Data4"].ToString();
                    admin.Data5 = reader["Data5"].ToString();
                    admin.Data6 = reader["Data6"].ToString();
                    admin.Data7 = reader["Data7"].ToString();
                    admin.Data8 = reader["Data8"].ToString();
                    admin.Data9 = reader["Data9"].ToString();
                    admin.Data10 = reader["Data10"].ToString();
                    admin.IsCheck = AliceDAL.DataType.ConvertToInt(reader["IsCheck"].ToString());
                    admin.CDate = AliceDAL.DataType.ConvertToDateTime(reader["CDate"].ToString());
                    reader.Close();
                    return admin;
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
        public static Models.BD_Wallet GetUserWalletByUserID(int userID)
        {
            Models.BD_Wallet bw = new Models.BD_Wallet();
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID) };
                string str = "select * from BD_Wallet where UserID=@ID;";

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    bw.Money1 = AliceDAL.DataType.ConvertToDecimal(reader["Money1"].ToString());
                    bw.Money2 = AliceDAL.DataType.ConvertToDecimal(reader["Money2"].ToString());
                    bw.ID = AliceDAL.DataType.ConvertToInt(reader["ID"].ToString());
                    bw.UserID = AliceDAL.DataType.ConvertToInt(reader["UserID"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
            return bw;
        }
        public static int GetUserCouponCountByUserID(int userID)
        {
            try
            {
                //SqlParameter[] param = { new SqlParameter("@ID", userID) };
                //string str = "select * from BD_Wallet where UserID=@ID;";

                //SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                //if (reader.Read())
                //{
                //    bw.Money1 = AliceDAL.DataType.ConvertToDecimal(reader["Money1"].ToString());
                //    bw.Money2 = AliceDAL.DataType.ConvertToDecimal(reader["Money2"].ToString());
                //    bw.ID = AliceDAL.DataType.ConvertToInt(reader["ID"].ToString());
                //    bw.UserID = AliceDAL.DataType.ConvertToInt(reader["UserID"].ToString());
                //}
                //reader.Close();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
            return 0;//未实现
        }
        public static int EditUserWalletByUserID(int userID, decimal m1, decimal m2)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID), new SqlParameter("@Money1", m1), new SqlParameter("@Money2", m2) };
                string sql = "UPDATE BD_Wallet set Money1=Money1+@Money1,Money2=Money2+@Money2 where UserID=@ID;";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int SetUserWalletByUserID(int userID, decimal m1, decimal m2)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID), new SqlParameter("@Money1", m1), new SqlParameter("@Money2", m2) };
                string sql = "UPDATE BD_Wallet set Money1=@Money1,Money2=@Money2 where UserID=@ID;";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int EditUserName(int userID, string username)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID), new SqlParameter("@UserName", username) };
                string sql = "UPDATE [BD_Users] set Data1=@UserName where ID=@ID;";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static int EditAddress(int userID, string address)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID), new SqlParameter("@Address", address) };
                string sql = "UPDATE [BD_Users] set Data3=@Address where ID=@ID;";
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }

        public static string PreAdd(string mobile, string name, string plate, string brand, string model, string bid, string mid, string color)
        {
            try
            {
                string sql = "if exists (select 1 from BD_Users where LoginID=@LoginID) " +
                             "begin " +
                               "select 'exists loginid';" +
                             "end " +
                             "else if exists (select 1 from BD_Users where Mobile=@Mobile) " +
                             "begin " +
                                "select 'exists mobile';" +
                             "end " +
                             "else " +
                             "begin " +
                               "DECLARE @oid int;INSERT INTO BD_Users ([LoginID],[UserPwd],[Mobile],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10])VALUES(@LoginID,@UserPwd,@Mobile,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10);" +
                               "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50), new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50), new SqlParameter("@Mobile", SqlDbType.NVarChar, 50), new SqlParameter("@Data1", SqlDbType.NVarChar, -1), new SqlParameter("@Data2", SqlDbType.NVarChar, -1), new SqlParameter("@Data3", SqlDbType.NVarChar, -1), new SqlParameter("@Data4", SqlDbType.NVarChar, -1), new SqlParameter("@Data5", SqlDbType.NVarChar, -1), new SqlParameter("@Data6", SqlDbType.NVarChar, -1), new SqlParameter("@Data7", SqlDbType.NVarChar, -1), new SqlParameter("@Data8", SqlDbType.NVarChar, -1), new SqlParameter("@Data9", SqlDbType.NVarChar, -1), new SqlParameter("@Data10", SqlDbType.NVarChar, -1) };

                parameters[0].Value = mobile;
                parameters[1].Value = AliceDAL.Common.CreateRandomValue(6, true);
                parameters[2].Value = mobile;
                parameters[3].Value = name;
                parameters[4].Value = "1";
                parameters[5].Value = "";
                parameters[6].Value = "";
                parameters[7].Value = "";
                parameters[8].Value = "";
                parameters[9].Value = "";
                parameters[10].Value = "";
                parameters[11].Value = "1";//1为预备用户
                parameters[12].Value = "";
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                if (AliceDAL.DataType.ConvertToInt(obj.ToString()) > 0)
                {
                    string sql1 = "INSERT INTO [BD_Wallet] ([UserID],[Money1],[Money2]) VALUES (@UserID,0,0)";
                    SqlParameter[] p = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
                    p[0].Value = obj;
                    SQLHelper.ExecuteNonQuery(CommandType.Text, sql1, p);

                    Models.UserCar uc = new Models.UserCar()
                      {
                          BrandID = AliceDAL.DataType.ConvertToInt(bid),
                          BrandShow = brand + " " + model,
                          Color = color ?? "",
                          Data1 = "",
                          Data2 = "",
                          Data3 = "",
                          Data4 = "",
                          Data5 = "",
                          ModelID = AliceDAL.DataType.ConvertToInt(mid),
                          Plate = plate,
                          UserID = AliceDAL.DataType.ConvertToInt(obj.ToString()),
                          UserName = name
                      };
                    string result = DAL.UserCar.Add(uc);
                }
                return obj.ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        /// <summary>
        /// 更新预备用户为普通用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int EditPreUser(int userID, string state)
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@ID", userID), new SqlParameter("@Data9", state) };
                string sql = "UPDATE [BD_Users] set [Data9]=@Data9 where ID=@ID;";
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

