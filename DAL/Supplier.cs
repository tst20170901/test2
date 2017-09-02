using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
namespace DAL
{
    public partial class Supplier
    {
        public static int Add(Models.Supplier model)
        {
            try
            {
                string sql = "INSERT INTO [Supplier] ([Title],[Phone],[Address],[ActArea],[SortID],[ProvinceName],[CityName],[IsShow])VALUES(@Title,@Phone,@Address,@ActArea,@SortID,@ProvinceName,@CityName,@IsShow);";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Phone", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Address", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@ActArea", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@SortID", SqlDbType.Int,4), 
                                                new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CityName", SqlDbType.NVarChar, 20), 
                                                new SqlParameter("@IsShow", SqlDbType.TinyInt,1 )
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Phone;
                parameters[2].Value = model.Address;
                parameters[3].Value = model.ActArea;
                parameters[4].Value = model.SortID;
                parameters[5].Value = model.ProvinceName;
                parameters[6].Value = model.CityName;
                parameters[7].Value = model.IsShow;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }

        public static string AddReg(Models.Supplier model)
        {
            try
            {
                string sql = "if exists (select 1 from [Supplier] where Title=@Title and Phone=@Phone and CityName=@CityName and ProvinceName=@ProvinceName) " +
                             "begin " +
                                "select 'exists';" +
                             "end " +
                             "else " +
                             "begin " +
                                "INSERT INTO [Supplier] ([Title],[Phone],[Address],[ActArea],[SortID],[ProvinceName],[CityName],[IsShow])VALUES(@Title,@Phone,@Address,@ActArea,@SortID,@ProvinceName,@CityName,@IsShow);" +
                                "select 'ok';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Phone", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Address", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@ActArea", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@SortID", SqlDbType.Int,4), 
                                                new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CityName", SqlDbType.NVarChar, 20), 
                                                new SqlParameter("@IsShow", SqlDbType.TinyInt,1 )
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Phone;
                parameters[2].Value = model.Address;
                parameters[3].Value = model.ActArea;
                parameters[4].Value = model.SortID;
                parameters[5].Value = model.ProvinceName;
                parameters[6].Value = model.CityName;
                parameters[7].Value = model.IsShow;
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }

        public static int Edit(Models.Supplier model)
        {
            try
            {
                string sql = "UPDATE [Supplier] set [Title]=@Title,[Phone]=@Phone,[Address]=@Address,[ActArea]=@ActArea,[SortID]=@SortID,[ProvinceName]=@ProvinceName,[CityName]=@CityName,[IsShow]=@IsShow where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Phone", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Address", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@ActArea", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@SortID", SqlDbType.Int,4), 
                                                new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CityName", SqlDbType.NVarChar, 20), 
                                                new SqlParameter("@IsShow", SqlDbType.TinyInt,1),
                                                new SqlParameter("@ID", SqlDbType.Int,4)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Phone;
                parameters[2].Value = model.Address;
                parameters[3].Value = model.ActArea;
                parameters[4].Value = model.SortID;
                parameters[5].Value = model.ProvinceName;
                parameters[6].Value = model.CityName;
                parameters[7].Value = model.IsShow;
                parameters[8].Value = model.ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }

        public static DataTable GetList()
        {
            DataTable dt = DAL.PageModel.DateList("[Supplier]", "IsShow=1", "SortID", 0);
            return dt;
        }

        public static Models.Supplier GetModel(int sid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[Supplier]", String.Format("ID={0}", sid.ToString()));
            if (dt == null) return null;
            var m = (from field in dt.AsEnumerable()
                     select new Models.Supplier()
                     {
                         ActArea = field.Field<string>("ActArea"),
                         CityName = field.Field<string>("CityName"),
                         Address = field.Field<string>("Address"),
                         Phone = field.Field<string>("Phone"),
                         ProvinceName = field.Field<string>("ProvinceName"),
                         Title = field.Field<string>("Title"),
                         SortID = field.Field<int>("SortID"),
                         ID = field.Field<int>("ID"),
                         IsShow = field.Field<byte>("IsShow")
                     }).ToList().First();
            return m;
        }
    }
}

