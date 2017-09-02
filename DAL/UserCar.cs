using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class UserCar
    {
        public static string Edit(Models.UserCar model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [UserCar] where Plate=@Plate and UserID=@UserID and ID<>@ID) " +
                             "begin " +
                               "select 'exists';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update [UserCar] set ");
                strSql.Append("Data2 = @Data2,");
                strSql.Append("Data3 = @Data3,");
                strSql.Append("Data4 = @Data4,");
                strSql.Append("Data5 = @Data5,");
                strSql.Append("Plate = @Plate,");
                strSql.Append("BrandShow = @BrandShow,");
                strSql.Append("BrandID = @BrandID,");
                strSql.Append("ModelID = @ModelID,");
                strSql.Append("Color = @Color,");
                strSql.Append("UserID = @UserID,");
                strSql.Append("UserName = @UserName,");
                strSql.Append("Data1 = @Data1 ");
                strSql.Append("where ID=@ID;");
                strSql.Append("select 'ok'; end");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@ID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Data2", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data3", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data4", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data5", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Plate", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@BrandShow", SqlDbType.NVarChar, 30), 
                                                new SqlParameter("@BrandID", SqlDbType.Int, 4), 
                                                new SqlParameter("@ModelID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Color", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@UserID", SqlDbType.Int, 4), 
                                                new SqlParameter("@UserName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@Data1", SqlDbType.NVarChar, 200) 
                                            };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.Data2;
                parameters[2].Value = model.Data3;
                parameters[3].Value = model.Data4;
                parameters[4].Value = model.Data5;
                parameters[5].Value = model.Plate;
                parameters[6].Value = model.BrandShow;
                parameters[7].Value = model.BrandID;
                parameters[8].Value = model.ModelID;
                parameters[9].Value = model.Color;
                parameters[10].Value = model.UserID;
                parameters[11].Value = model.UserName;
                parameters[12].Value = model.Data1;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static int EditSafeCompany(Models.UserCar model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [UserCar] where Plate=@Plate and UserID=@UserID) " +
                             "begin " +
                               "update [UserCar] set [Data2]=@Data2,[Data3]=@Data3,[Data4]=@Data4,[BrandShow]=@BrandShow where Plate=@Plate and UserID=@UserID;" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into [UserCar] (Data2,Data3,Data4,Data5,Plate,BrandShow,BrandID,ModelID,Color,UserID,UserName,Data1) values (");
                strSql.Append("@Data2,@Data3,@Data4,@Data5,@Plate,@BrandShow,@BrandID,@ModelID,@Color,@UserID,@UserName,@Data1);");
                strSql.Append("end");
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Data2", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data3", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data4", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Data5", SqlDbType.NVarChar, 200), 
                                                new SqlParameter("@Plate", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@BrandShow", SqlDbType.NVarChar, 30), 
                                                new SqlParameter("@BrandID", SqlDbType.Int, 4), 
                                                new SqlParameter("@ModelID", SqlDbType.Int, 4), 
                                                new SqlParameter("@Color", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@UserID", SqlDbType.Int, 4), 
                                                new SqlParameter("@UserName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@Data1", SqlDbType.NVarChar, 200) 
                                            };
                parameters[0].Value = model.Data2;
                parameters[1].Value = model.Data3;
                parameters[2].Value = model.Data4;
                parameters[3].Value = model.Data5;
                parameters[4].Value = model.Plate;
                parameters[5].Value = model.BrandShow;
                parameters[6].Value = model.BrandID;
                parameters[7].Value = model.ModelID;
                parameters[8].Value = model.Color;
                parameters[9].Value = model.UserID;
                parameters[10].Value = model.UserName;
                parameters[11].Value = model.Data1;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static string Add(Models.UserCar model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("if exists (select 1 from [UserCar] where Plate=@Plate and UserID=@UserID) " +
                             "begin " +
                               "select 'exists';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into UserCar(");
                strSql.Append("Data2,Data3,Data4,Data5,Plate,BrandShow,BrandID,ModelID,Color,UserID,UserName,Data1");
                strSql.Append(") values (");
                strSql.Append("@Data2,@Data3,@Data4,@Data5,@Plate,@BrandShow,@BrandID,@ModelID,@Color,@UserID,@UserName,@Data1");
                strSql.Append(") ");
                strSql.Append(";select @@IDENTITY; end");
                SqlParameter[] parameters = {
                                                new SqlParameter("@Data2", SqlDbType.NVarChar,200),
                                                new SqlParameter("@Data3", SqlDbType.NVarChar,200),
                                                new SqlParameter("@Data4", SqlDbType.NVarChar,200),            
                                                new SqlParameter("@Data5", SqlDbType.NVarChar,200),            
                                                new SqlParameter("@Plate", SqlDbType.NVarChar,10),           
                                                new SqlParameter("@BrandShow", SqlDbType.NVarChar,30),            
                                                new SqlParameter("@BrandID", SqlDbType.Int,4),            
                                                new SqlParameter("@ModelID", SqlDbType.Int,4),            
                                                new SqlParameter("@Color", SqlDbType.NVarChar,100),            
                                                new SqlParameter("@UserID", SqlDbType.Int,4) ,            
                                                new SqlParameter("@UserName", SqlDbType.NVarChar,10),            
                                                new SqlParameter("@Data1", SqlDbType.NVarChar,200)
                                            };

                parameters[0].Value = model.Data2;
                parameters[1].Value = model.Data3;
                parameters[2].Value = model.Data4;
                parameters[3].Value = model.Data5;
                parameters[4].Value = model.Plate;
                parameters[5].Value = model.BrandShow;
                parameters[6].Value = model.BrandID;
                parameters[7].Value = model.ModelID;
                parameters[8].Value = model.Color;
                parameters[9].Value = model.UserID;
                parameters[10].Value = model.UserName;
                parameters[11].Value = model.Data1;
                return SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public static List<Models.UserCar> GetList(int uid)
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[uv_UserModel_Car]", "UserID=" + uid, "ID", "desc", 0);
                List<Models.UserCar> list = new List<Models.UserCar>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.UserCar m = new Models.UserCar();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.BrandID = DataType.ConvertToInt(row["BrandID"].ToString());
                        m.BrandShow = row["BrandShow"].ToString();
                        m.Color = row["Color"].ToString();
                        m.Data1 = row["Data1"].ToString();
                        m.Data2 = row["Data2"].ToString();
                        m.Data3 = row["Data3"].ToString();
                        m.Data4 = row["Data4"].ToString();
                        m.Data5 = row["Data5"].ToString();
                        m.ModelID = DataType.ConvertToInt(row["ModelID"].ToString());
                        m.Plate = row["Plate"].ToString();
                        m.UserID = DataType.ConvertToInt(row["UserID"].ToString());
                        m.UserName = row["UserName1"].ToString();
                        m.Mobile = row["LoginID"].ToString();
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
        public static Models.UserCar GetModel(int cid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[UserCar]", String.Format("ID={0}", cid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.UserCar m = new Models.UserCar();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.BrandID = DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                m.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                m.Color = dt.Rows[0]["Color"].ToString();
                m.Data1 = dt.Rows[0]["Data1"].ToString();
                m.Data2 = dt.Rows[0]["Data2"].ToString();
                m.Data3 = dt.Rows[0]["Data3"].ToString();
                m.Data4 = dt.Rows[0]["Data4"].ToString();
                m.Data5 = dt.Rows[0]["Data5"].ToString();
                m.ModelID = DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                m.Plate = dt.Rows[0]["Plate"].ToString();
                m.UserID = DataType.ConvertToInt(dt.Rows[0]["UserID"].ToString());
                m.UserName = dt.Rows[0]["UserName"].ToString();
                return m;
            }
        }
        public static Models.UserCar UserExist(int uid, string plate)
        {
            DataTable dt = DAL.PageModel.Table_Model("[UserCar]", String.Format("[UserID]={0} and [Plate]='{1}'", uid.ToString(), plate));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.UserCar m = new Models.UserCar();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.BrandID = DataType.ConvertToInt(dt.Rows[0]["BrandID"].ToString());
                m.BrandShow = dt.Rows[0]["BrandShow"].ToString();
                m.Color = dt.Rows[0]["Color"].ToString();
                m.Data1 = dt.Rows[0]["Data1"].ToString();
                m.Data2 = dt.Rows[0]["Data2"].ToString();
                m.Data3 = dt.Rows[0]["Data3"].ToString();
                m.Data4 = dt.Rows[0]["Data4"].ToString();
                m.Data5 = dt.Rows[0]["Data5"].ToString();
                m.ModelID = DataType.ConvertToInt(dt.Rows[0]["ModelID"].ToString());
                m.Plate = dt.Rows[0]["Plate"].ToString();
                m.UserID = DataType.ConvertToInt(dt.Rows[0]["UserID"].ToString());
                m.UserName = dt.Rows[0]["UserName"].ToString();
                return m;
            }
        }
    }
}

