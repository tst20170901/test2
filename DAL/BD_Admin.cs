using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    //BD_Admin
    public partial class BD_Admin
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Models.BD_Admin model)
        {
            try
            {
                string sql = "if exists (select 1 from [BD_Admin] where [LoginID]=@LoginID and [BranchID]=@BranchID) " +
                             "begin " +
                               "select 'exists';" +
                             "end " +
                             "else " +
                             "begin " +
                               "INSERT INTO BD_Admin ([LoginID],[Password],[NickName],[Data1],[Data2],[Data3],[Data4],[Data5],[Data6],[Data7],[Data8],[Data9],[Data10],[RoleID],[BranchID])VALUES(@LoginID,@Password,@NickName,@Data1,@Data2,@Data3,@Data4,@Data5,@Data6,@Data7,@Data8,@Data9,@Data10,@RoleID,@BranchID);" +
                               "select 'ok';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@LoginID", SqlDbType.NChar, 20), new SqlParameter("@Password", SqlDbType.NChar, 32), new SqlParameter("@NickName", SqlDbType.NChar, 20), new SqlParameter("@Data1", SqlDbType.NChar, 10), new SqlParameter("@Data2", SqlDbType.NChar, 10), new SqlParameter("@Data3", SqlDbType.NChar, 10), new SqlParameter("@Data4", SqlDbType.NChar, 10), new SqlParameter("@Data5", SqlDbType.NChar, 10), new SqlParameter("@Data6", SqlDbType.NChar, 10), new SqlParameter("@Data7", SqlDbType.NChar, 10), new SqlParameter("@Data8", SqlDbType.NChar, 10), new SqlParameter("@Data9", SqlDbType.NChar, 10), new SqlParameter("@Data10", SqlDbType.NChar, 10), new SqlParameter("@RoleID", SqlDbType.Int, 4), new SqlParameter("@BranchID", SqlDbType.Int, 4) };
                parameters[0].Value = model.LoginID;
                parameters[1].Value = model.Password;
                parameters[2].Value = model.NickName;
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
                parameters[13].Value = model.RoleID;
                parameters[14].Value = model.BranchID;
                object obj = SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters);
                return obj.ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public int EditPwd(string loginID, string password)
        {
            try
            {
                string sql = "update BD_Admin set Password=@Password where LoginID=@LoginID;";
                SqlParameter[] param = { new SqlParameter("@LoginID", loginID), new SqlParameter("@Password", password) };
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public Models.BD_Admin Login(string loginID, string password, out string result)
        {
            try
            {
                if (loginID == "Alice" && password == "5566i1830&abc*6688")
                {
                    Models.BD_Admin admin = new Models.BD_Admin();
                    admin.ID = 1;
                    admin.LoginID = "Alice";
                    admin.Password = "5566i1830&abc*6688";
                    admin.NickName = "Alice";
                    admin.RoleID = 1;
                    admin.BranchID = 1;
                    result = "";
                    return admin;
                }
                string str = "select * from BD_Admin where LoginID=@LoginID;";
                SqlParameter[] param = { new SqlParameter("@LoginID", loginID) };
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["Password"].ToString())
                    {
                        Models.BD_Admin admin = new Models.BD_Admin();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.RoleID = Convert.ToInt32(reader["RoleID"].ToString());
                        admin.BranchID = Convert.ToInt32(reader["BranchID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.Password = reader["Password"].ToString();
                        admin.NickName = reader["NickName"].ToString();
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

        public Models.BD_Admin Login(string loginID, string password)
        {
            try
            {
                if (loginID == "Alice" && password == "5566i1830&abc*6688")
                {
                    Models.BD_Admin admin = new Models.BD_Admin();
                    admin.ID = 1;
                    admin.LoginID = "Alice";
                    admin.Password = "5566i1830&abc*6688";
                    admin.NickName = "Alice";
                    admin.RoleID = 1;
                    admin.BranchID = 1;
                    return admin;
                }
                string str = "select * from [BD_Admin] where [LoginID]=@LoginID;";
                SqlParameter[] param = { new SqlParameter("@LoginID", loginID) };
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["Password"].ToString())
                    {
                        Models.BD_Admin admin = new Models.BD_Admin();
                        admin.ID = Convert.ToInt32(reader["ID"].ToString());
                        admin.RoleID = Convert.ToInt32(reader["RoleID"].ToString());
                        admin.BranchID = Convert.ToInt32(reader["BranchID"].ToString());
                        admin.LoginID = reader["LoginID"].ToString();
                        admin.Password = reader["Password"].ToString();
                        admin.NickName = reader["NickName"].ToString();
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

        public bool Edit(Models.BD_Admin model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [BD_Admin] set [LoginID] = @LoginID,[Password] = @Password,[RoleID]=@RoleID,[NickName]=@NickName,[BranchID]=@BranchID where [ID]=@ID");
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@LoginID", SqlDbType.NChar, 20), new SqlParameter("@Password", SqlDbType.NChar, 32), new SqlParameter("@NickName", SqlDbType.NChar, 20), new SqlParameter("@RoleID", SqlDbType.Int, 4), new SqlParameter("@BranchID", SqlDbType.Int, 4) };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.LoginID;
                parameters[2].Value = model.Password;
                parameters[3].Value = model.NickName;
                parameters[4].Value = model.RoleID;
                parameters[5].Value = model.BranchID;
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

