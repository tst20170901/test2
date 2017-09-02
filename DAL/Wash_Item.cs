using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class Wash_Item
    {
        public static int Edit(Models.Wash_Item model)
        {
            try
            {
                string sql = "Update [Wash_Item] set [Title]=@Title,[Price]=@Price,[IsMust]=@IsMust,[SortID]=@SortID,[BranID]=@BranID,[Online]=@Online,[WorkPrice]=@WorkPrice where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@SortID", SqlDbType.Int, 4),
                                                new SqlParameter("@ID",SqlDbType.Int,4),
                                                new SqlParameter("@BranID", SqlDbType.Int, 4),                                                 
                                                new SqlParameter("@Online", SqlDbType.TinyInt, 1),                                             
                                                new SqlParameter("@IsMust", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@WorkPrice",SqlDbType.Decimal,9)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Price;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.ID;
                parameters[4].Value = model.BranID;
                parameters[5].Value = model.Online;
                parameters[6].Value = model.IsMust;
                parameters[7].Value = model.WorkPrice;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
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
                string sql = "Update [Wash_Item] set [ItemState]=@ItemState where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@ItemState", SqlDbType.TinyInt, 1),
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
        public static int Add(Models.Wash_Item model)
        {
            try
            {
                string sql = "INSERT INTO [Wash_Item] ([Title],[Price],[IsMust],[ItemState],[SortID],[BranID],[Online],[WorkPrice])VALUES(@Title,@Price,@IsMust,@ItemState,@SortID,@BranID,@Online,@WorkPrice);";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@IsMust", SqlDbType.TinyInt, 1), 
                                                new SqlParameter("@ItemState", SqlDbType.TinyInt, 1), 
                                                new SqlParameter("@SortID", SqlDbType.Int,4),
                                                new SqlParameter("@BranID", SqlDbType.Int, 4),                                                 
                                                new SqlParameter("@Online", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@WorkPrice",SqlDbType.Decimal,9)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Price;
                parameters[2].Value = model.IsMust;
                parameters[3].Value = model.ItemState;
                parameters[4].Value = model.SortID;
                parameters[5].Value = model.BranID;
                parameters[6].Value = model.Online;
                parameters[7].Value = model.WorkPrice;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static List<Models.Wash_Item> GetList()
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[Wash_Item]", "", "SortID", "desc", 0);
                List<Models.Wash_Item> list = new List<Models.Wash_Item>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.Wash_Item m = new Models.Wash_Item();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.IsMust = DataType.ConvertToInt(row["IsMust"].ToString());
                        m.ItemState = DataType.ConvertToInt(row["ItemState"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.SortID = DataType.ConvertToInt(row["SortID"].ToString());
                        m.Title = row["Title"].ToString();
                        m.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                        m.Online = DataType.ConvertToInt(row["Online"].ToString());
                        m.BranID = DataType.ConvertToInt(row["BranID"].ToString());
                        m.WorkPrice = DataType.ConvertToDecimal(row["WorkPrice"].ToString());
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
        public static List<Models.Wash_Item> GetList(string where)
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[Wash_Item]", where, "SortID", "desc", 0);
                List<Models.Wash_Item> list = new List<Models.Wash_Item>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.Wash_Item m = new Models.Wash_Item();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.IsMust = DataType.ConvertToInt(row["IsMust"].ToString());
                        m.ItemState = DataType.ConvertToInt(row["ItemState"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.SortID = DataType.ConvertToInt(row["SortID"].ToString());
                        m.Title = row["Title"].ToString();
                        m.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                        m.Online = DataType.ConvertToInt(row["Online"].ToString());
                        m.BranID = DataType.ConvertToInt(row["BranID"].ToString());
                        m.WorkPrice = DataType.ConvertToDecimal(row["WorkPrice"].ToString());
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
        public static Models.Wash_Item GetModel(int wid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[Wash_Item]", String.Format("ID={0}", wid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.Wash_Item m = new Models.Wash_Item();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.IsMust = DataType.ConvertToInt(dt.Rows[0]["IsMust"].ToString());
                m.ItemState = DataType.ConvertToInt(dt.Rows[0]["ItemState"].ToString());
                m.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                m.SortID = DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
                m.Title = dt.Rows[0]["Title"].ToString();
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.Online = DataType.ConvertToInt(dt.Rows[0]["Online"].ToString());
                m.BranID = DataType.ConvertToInt(dt.Rows[0]["BranID"].ToString());
                m.WorkPrice = DataType.ConvertToDecimal(dt.Rows[0]["WorkPrice"].ToString());
                return m;
            }
        }
    }
}

