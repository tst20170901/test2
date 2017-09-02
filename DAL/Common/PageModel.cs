using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace DAL
{
    public static class PageModel
    {
        public static int DataCount(string tbName, string where)
        {
            StringBuilder sql = new StringBuilder("select count(1) from ");
            sql.Append(tbName);
            if (!String.IsNullOrEmpty(where)) sql.Append(String.Format(" where {0}", where));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return int.Parse(SQLHelper.ExecuteScalar(CommandType.StoredProcedure, "RunSQL", p).ToString().Trim());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static DataTable DataByPage(string tbName, string where, string indexFields, int startIndex, int endIndex)
        {
            StringBuilder sql = new StringBuilder(String.Format("with ooo as (select row_number() over (order by N.{0} desc) as row,* from {1} N", indexFields, tbName));
            if (!String.IsNullOrEmpty(where)) sql.Append(String.Format(" where {0}", where));
            sql.Append(String.Format(") select * from ooo where row between {0} and {1}", startIndex.ToString(), endIndex.ToString()));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static DataTable DataByPage(string tbName, string where, string indexFields, int startIndex, int endIndex, out int total)
        {
            StringBuilder sql = new StringBuilder(String.Format("with ooo as (select row_number() over (order by N.{0} desc) as row,* from {1} N", indexFields, tbName));
            StringBuilder sqlcount = new StringBuilder(String.Format(";select count(1) from {0}", tbName));

            if (!String.IsNullOrEmpty(where))
            {
                sql.Append(String.Format(" where {0}", where));
                sqlcount.Append(String.Format(" where {0}", where));
            }
            sql.Append(String.Format(") select * from ooo where row between {0} and {1}", startIndex.ToString(), endIndex.ToString()));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString() + sqlcount.ToString();
                DataSet ds = AliceDAL.Common.RetDataSet(SQLHelper.ExecuteDataSet(CommandType.StoredProcedure, "RunSQL", p));
                if (ds == null)
                {
                    total = 0;
                    return null;
                }
                else
                {
                    total = AliceDAL.DataType.ConvertToInt(ds.Tables[1].Rows[0][0].ToString());
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                total = 0;
                return null;
            }
        }
        public static DataTable DBPKeys(string tbName, string where, string keys, string indexFields, int startIndex, int endIndex, out int total)
        {
            StringBuilder sql = new StringBuilder(String.Format("with ooo as (select row_number() over (order by N.{0}) as row,{1} from {2} N", indexFields, keys, tbName));
            StringBuilder sqlcount = new StringBuilder(String.Format(";select count(1) from {0}", tbName));

            if (!String.IsNullOrEmpty(where))
            {
                sql.Append(String.Format(" where {0}", where));
                sqlcount.Append(String.Format(" where {0}", where));
            }
            sql.Append(String.Format(") select row,{0} from ooo where row between {1} and {2}", keys, startIndex.ToString(), endIndex.ToString()));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString() + sqlcount.ToString();
                DataSet ds = AliceDAL.Common.RetDataSet(SQLHelper.ExecuteDataSet(CommandType.StoredProcedure, "RunSQL", p));
                if (ds == null)
                {
                    total = 0;
                    return null;
                }
                else
                {
                    total = AliceDAL.DataType.ConvertToInt(ds.Tables[1].Rows[0][0].ToString());
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                total = 0;
                return null;
            }
        }

        public static DataTable DataListBySQL(string sql)
        {
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql;
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static DataTable DateList(string tbName, string where, string indexFields, int count)
        {
            StringBuilder sql = new StringBuilder(String.Format("select * from {0}", tbName));
            if (count != 0)
            {
                sql = new StringBuilder(String.Format("select top {0} * from {1}", count.ToString(), tbName));
            }
            if (!String.IsNullOrEmpty(where)) sql.Append(String.Format(" where {0}", where));

            sql.Append(String.Format(" order by {0} desc", indexFields));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static object RunSQL(string sql)
        {
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql;
                return SQLHelper.ExecuteScalar(CommandType.StoredProcedure, "RunSQL", p);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static DataTable DateList(string tbName, string where, string indexFields, string sort, int count)
        {
            StringBuilder sql = new StringBuilder(String.Format("select * from {0}", tbName));
            if (count != 0) sql = new StringBuilder(String.Format("select top {0} * from {1}", count.ToString(), tbName));
            if (!String.IsNullOrEmpty(where)) sql.Append(String.Format(" where {0}", where));
            sql.Append(String.Format(" order by {0} {1}", indexFields, sort));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static DataTable DataKeysList(string tbName, string keys, string where, string indexFields, int count)
        {
            StringBuilder sql = new StringBuilder(String.Format("select {0} from {1}", keys, tbName));
            if (count != 0)
            {
                sql = new StringBuilder(String.Format("select top {0} {1} from {2}", count.ToString(), keys, tbName));
            }
            if (!String.IsNullOrEmpty(where)) sql.Append(String.Format(" where {0}", where));

            sql.Append(String.Format(" order by {0} desc", indexFields));
            try
            {
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int Delete(string tbName, string where)
        {
            try
            {
                StringBuilder sql = new StringBuilder(String.Format("delete from {0} where {1}", tbName, where));

                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return SQLHelper.ExecuteNonQuery(CommandType.StoredProcedure, "RunSQL", p);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return 0;
            }
        }
        public static bool Delete(string tbName, string key, List<string> list)
        {
            try
            {
                StringBuilder sql = new StringBuilder(String.Format("delete from {0} where {1} in ({2})", tbName, key, AliceDAL.Common.List2String(list)));
                return SQLHelper.ExecuteTransactionSQL(sql.ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
        public static bool Delete(string tbName, string key, int[] list)
        {
            try
            {
                StringBuilder sql = new StringBuilder(String.Format("delete from {0} where {1} in ({2})", tbName, key, AliceDAL.Common.Ints2String(list)));
                return SQLHelper.ExecuteTransactionSQL(sql.ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
        public static DataTable Table_Model(string tbName, string where)
        {
            try
            {
                StringBuilder sql = new StringBuilder(String.Format("select * from {0} where {1}", tbName, where));

                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                return AliceDAL.Common.RetDataTable(SQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "RunSQL", p));
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static void Hit(string tbName, string set, string where)
        {
            try
            {
                StringBuilder sql = new StringBuilder(String.Format("update {0} set {1} where {2}", tbName, set, where));
                SqlParameter[] p = { new SqlParameter("@sql", SqlDbType.NVarChar, -1) };
                p[0].Value = sql.ToString();
                SQLHelper.ExecuteNonQuery(CommandType.StoredProcedure, "RunSQL", p);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
            }
        }
    }
}
