using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AliceDAL;
namespace DAL
{
    public partial class BD_BusiAction
    {
        public static string Add(Models.BD_BusiAction model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_BusiAction] where [BranchID]=@BranchID and [ActionName]=@ActionName) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into [BD_BusiAction] ([ActionName],[Body],[SortID],[BranchID],[IsNewUser],[SMSContent],[DataType]) values (@ActionName,@Body,@SortID,@BranchID,@IsNewUser,@SMSContent,@DataType);" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@ActionName", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1),
                                                new SqlParameter("@SortID", SqlDbType.Int, 4), 
                                                new SqlParameter("@BranchID", SqlDbType.Int, 4), 
                                                new SqlParameter("@IsNewUser", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@SMSContent",SqlDbType.NVarChar,200),
                                                new SqlParameter("@DataType",SqlDbType.TinyInt,1)
                                            };
                parameters[0].Value = model.ActionName;
                parameters[1].Value = model.Body;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.BranchID;
                parameters[4].Value = model.IsNewUser;
                parameters[5].Value = model.SMSContent;
                parameters[6].Value = model.DataType;
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
        public static string Edit(Models.BD_BusiAction model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_BusiAction] where [BranchID]=@BranchID and [ID]<>@ID and [ActionName]=@ActionName) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update [BD_BusiAction] set [ActionName]=@ActionName,[Body]=@Body,[SortID]=@SortID,[IsNewUser]=@IsNewUser,[SMSContent]=@SMSContent,[DataType]=@DataType where ID=@ID;" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@ActionName", SqlDbType.NVarChar, 100), 
                                                new SqlParameter("@Body", SqlDbType.NVarChar, -1),
                                                new SqlParameter("@SortID", SqlDbType.Int, 4),
                                                new SqlParameter("@IsNewUser", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@ID", SqlDbType.Int, 4),
                                                new SqlParameter("@BranchID",SqlDbType.Int,4),
                                                new SqlParameter("@SMSContent",SqlDbType.NVarChar,200),
                                                new SqlParameter("@DataType",SqlDbType.TinyInt,1)
                                             };
                parameters[0].Value = model.ActionName;
                parameters[1].Value = model.Body;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.IsNewUser;
                parameters[4].Value = model.ID;
                parameters[5].Value = model.BranchID;
                parameters[6].Value = model.SMSContent;
                parameters[7].Value = model.DataType;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        public static Models.BD_BusiAction GetModel(int bid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[BD_BusiAction]", String.Format("ID={0}", bid.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.BD_BusiAction m = new Models.BD_BusiAction();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.ActionName = dt.Rows[0]["ActionName"].ToString();
                m.ActionState = DataType.ConvertToInt(dt.Rows[0]["ActionState"].ToString());
                m.Body = dt.Rows[0]["Body"].ToString();
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.IsNewUser = DataType.ConvertToInt(dt.Rows[0]["IsNewUser"].ToString());
                m.SortID = DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
                m.BranchID = DataType.ConvertToInt(dt.Rows[0]["BranchID"].ToString());
                m.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                m.DataType = DataType.ConvertToInt(dt.Rows[0]["DataType"].ToString());
                return m;
            }
        }
        public static Models.BD_BusiAction GetModelBySortID()
        {
            DataTable dt = DAL.PageModel.Table_Model("[BD_BusiAction]", String.Format("[SortID]={0}", "999"));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.BD_BusiAction m = new Models.BD_BusiAction();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.ActionName = dt.Rows[0]["ActionName"].ToString();
                m.ActionState = DataType.ConvertToInt(dt.Rows[0]["ActionState"].ToString());
                m.Body = dt.Rows[0]["Body"].ToString();
                m.CDate = DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString());
                m.IsNewUser = DataType.ConvertToInt(dt.Rows[0]["IsNewUser"].ToString());
                m.SortID = DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString());
                m.BranchID = DataType.ConvertToInt(dt.Rows[0]["BranchID"].ToString());
                m.SMSContent = dt.Rows[0]["SMSContent"].ToString();
                m.DataType = DataType.ConvertToInt(dt.Rows[0]["DataType"].ToString());
                return m;
            }
        }
        public static List<Models.BD_BusiAction> GetList(string where)
        {
            try
            {
                DataTable dt = DAL.PageModel.DateList("[BD_BusiAction]", where, "SortID", "desc", 0);
                List<Models.BD_BusiAction> list = new List<Models.BD_BusiAction>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.BD_BusiAction m = new Models.BD_BusiAction();
                        m.ID = DataType.ConvertToInt(row["ID"].ToString());
                        m.ActionName = row["ActionName"].ToString();
                        m.ActionState = DataType.ConvertToInt(row["ActionState"].ToString());
                        m.Body = row["Body"].ToString();
                        m.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                        m.IsNewUser = DataType.ConvertToInt(row["IsNewUser"].ToString());
                        m.SortID = DataType.ConvertToInt(row["SortID"].ToString());
                        m.BranchID = DataType.ConvertToInt(row["BranchID"].ToString());
                        m.SMSContent = row["SMSContent"].ToString();
                        m.DataType = DataType.ConvertToInt(row["DataType"].ToString());
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
        public static int ChangeState(int ID, int state)
        {
            try
            {
                string sql = "Update [BD_BusiAction] set [ActionState]=@ActionState where ID=@ID;";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@ActionState", SqlDbType.TinyInt, 1),
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
        public static List<Models.BD_BusiAction_Item> GetItemListByActionID(int actionID)
        {
            try
            {
                string sql = "select * from [BD_BusiAction_Item] where [ActionID]=@ActionID order by [CDate] desc";
                SqlParameter[] para = { new SqlParameter("@ActionID", SqlDbType.Int, 4) };
                para[0].Value = actionID;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
                List<Models.BD_BusiAction_Item> list = new List<Models.BD_BusiAction_Item>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.BD_BusiAction_Item model = new Models.BD_BusiAction_Item();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.ActionID = AliceDAL.DataType.ConvertToInt(item["ActionID"].ToString());
                        model.CDate = DataType.ConvertToDateTime(item["CDate"].ToString());
                        model.Count = AliceDAL.DataType.ConvertToInt(item["Count"].ToString());
                        model.ItemID = AliceDAL.DataType.ConvertToInt(item["ItemID"].ToString());
                        model.ItemPrice = AliceDAL.DataType.ConvertToDecimal(item["ItemPrice"].ToString());
                        model.ItemTitle = item["ItemTitle"].ToString();
                        model.Period = AliceDAL.DataType.ConvertToInt(item["Period"].ToString());
                        list.Add(model);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static string AddItem(Models.BD_BusiAction_Item model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_BusiAction_Item] where [ActionID]=@ActionID and [ItemID]=@ItemID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into [BD_BusiAction_Item] ([ActionID],[ItemID],[ItemTitle],[ItemPrice],[Count],[Period]) values (@ActionID,@ItemID,@ItemTitle,@ItemPrice,@Count,@Period);" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@ActionID", SqlDbType.Int, 4), 
                                                new SqlParameter("@ItemID", SqlDbType.Int, 4),
                                                new SqlParameter("@ItemTitle", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@ItemPrice", SqlDbType.Decimal, 9), 
                                                new SqlParameter("@Count", SqlDbType.Int, 4),
                                                new SqlParameter("@Period",SqlDbType.Int,4)
                                            };
                parameters[0].Value = model.ActionID;
                parameters[1].Value = model.ItemID;
                parameters[2].Value = model.ItemTitle;
                parameters[3].Value = model.ItemPrice;
                parameters[4].Value = model.Count;
                parameters[5].Value = model.Period;
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
    }
}

