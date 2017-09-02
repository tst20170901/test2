using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class Edo_ConfigKV
    {
        public static void Add(Models.Edo_ConfigKV model)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Keys", SqlDbType.NVarChar, 50), new SqlParameter("@Vals", SqlDbType.NVarChar, -1) };
                parameters[0].Value = model.Keys;
                parameters[1].Value = model.Vals;
                string sql = "insert into [Edo_ConfigKV]([Keys],[Vals]) values (@Keys,@Vals);";
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static void Edit(Models.Edo_ConfigKV model)
        {
            try
            {
                string sql = "UPDATE [Edo_ConfigKV] SET [Keys]=@Keys,[Vals]=@Vals where [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4), new SqlParameter("@Keys", SqlDbType.NVarChar, 50), new SqlParameter("@Vals", SqlDbType.NVarChar, -1) };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.Keys;
                parameters[2].Value = model.Vals;
                SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }

        public static void DeleteLinkById(string idList)
        {
            try
            {
                string commandText = String.Format("DELETE FROM [Edo_ConfigKV] WHERE [ID] IN ({0})", idList);
                SQLHelper.ExecuteNonQuery(CommandType.Text, commandText, null);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
        public static Models.Edo_ConfigKV GetModel(string keys)
        {
            try
            {
                string sql = String.Format("SELECT TOP 1 {0} FROM [Edo_ConfigKV] WHERE [Keys]=@Keys", "[ID],[Keys],[Vals]");
                SqlParameter[] para = { new SqlParameter("@Keys", SqlDbType.NVarChar, 50) };
                para[0].Value = keys;
                IDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, sql, para);
                if (reader == null) return null;
                if (reader.Read())
                {
                    Models.Edo_ConfigKV model = BuildLinksFromReader(reader);
                    reader.Close();
                    return model;
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
        public static Models.Edo_ConfigKV GetModel(int id)
        {
            try
            {
                string sql = String.Format("SELECT {0} FROM [Edo_ConfigKV] WHERE ID=@ID", "[ID],[Keys],[Vals]");
                SqlParameter[] para = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                para[0].Value = id;
                IDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, sql, para);
                if (reader == null) return null;
                if (reader.Read())
                {
                    Models.Edo_ConfigKV model = BuildLinksFromReader(reader);
                    reader.Close();
                    return model;
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
        private static Models.Edo_ConfigKV BuildLinksFromReader(IDataReader reader)
        {
            Models.Edo_ConfigKV model = new Models.Edo_ConfigKV();
            model.ID = AliceDAL.DataType.ConvertToInt(reader["ID"].ToString());
            model.Keys = reader["Keys"].ToString();
            model.Vals = reader["Vals"].ToString();
            return model;
        }
    }
}

