using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class ChargeType
    {
        public static int Edit(Models.ChargeType model)
        {
            try
            {
                string sql = "Update [ChargeType] set [Title]=@Title,[Price]=@Price,[GivePrice]=@GivePrice,[Remark]=@Remark where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@GivePrice", SqlDbType.Decimal, 9),
                                                new SqlParameter("@Remark", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@ID",SqlDbType.Int,4)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Price;
                parameters[2].Value = model.GivePrice;
                parameters[3].Value = model.Remark;
                parameters[4].Value = model.ID;
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
                string sql = "Update [ChargeType] set [State]=@State where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@State", SqlDbType.Int, 4),
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
        public static int Add(Models.ChargeType model)
        {
            try
            {
                string sql = "INSERT INTO [ChargeType] ([Title],[Price],[GivePrice],[State],[Remark])VALUES(@Title,@Price,@GivePrice,@State,@Remark);";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Price", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@GivePrice", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@State", SqlDbType.Int, 4),
                                                new SqlParameter("@Remark",SqlDbType.NVarChar,100)
                                            };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.Price;
                parameters[2].Value = model.GivePrice;
                parameters[3].Value = model.State;
                parameters[4].Value = model.Remark;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static List<Models.ChargeType> GetList()
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[ChargeType]", "", "ID", "asc", 0);
                List<Models.ChargeType> list = new List<Models.ChargeType>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.ChargeType m = new Models.ChargeType();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.GivePrice = DataType.ConvertToDecimal(row["GivePrice"].ToString());
                        m.State = DataType.ConvertToInt(row["State"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.Title = row["Title"].ToString();
                        m.Remark = row["Remark"].ToString();
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
        public static List<Models.ChargeType> GetList(string where)
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[ChargeType]", where, "ID", "asc", 0);
                List<Models.ChargeType> list = new List<Models.ChargeType>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.ChargeType m = new Models.ChargeType();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.GivePrice = DataType.ConvertToDecimal(row["GivePrice"].ToString());
                        m.State = DataType.ConvertToInt(row["State"].ToString());
                        m.Price = DataType.ConvertToDecimal(row["Price"].ToString());
                        m.Title = row["Title"].ToString();
                        m.Remark = row["Remark"].ToString();
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
        public static Models.ChargeType GetModel(int cid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[ChargeType]", String.Format("ID={0}", cid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.ChargeType m = new Models.ChargeType();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.GivePrice = DataType.ConvertToDecimal(dt.Rows[0]["GivePrice"].ToString());
                m.State = DataType.ConvertToInt(dt.Rows[0]["State"].ToString());
                m.Price = DataType.ConvertToDecimal(dt.Rows[0]["Price"].ToString());
                m.Title = dt.Rows[0]["Title"].ToString();
                m.Remark = dt.Rows[0]["Remark"].ToString();
                return m;
            }
        }
    }
}

