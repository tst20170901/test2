using AliceDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class BD_BearActionsAllot
    {
        public static string Add(Models.BD_BearActionsAllot model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_BearActionsAllot] where [Title]=@Title) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  insert into [BD_BearActionsAllot] ([Title],[ActionList]) values (@Title,@ActionList);" +
                             "  SELECT SCOPE_IDENTITY();" +
                             "end";
                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@ActionList", SqlDbType.NVarChar, -1)
                                            };
                parameters[0].Value = model.Title;
                parameters[1].Value = model.ActionList;
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
        public static string Edit(Models.BD_BearActionsAllot model)
        {
            try
            {
                string sql = "if exists(select 1 from [BD_BearActionsAllot] where [Title]=@Title and ID<>@ID) " +
                             "begin " +
                             "  select '-2';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  update [BD_BearActionsAllot] set [Title]=@Title,[ActionList]=@ActionList where ID=@ID;" +
                             "  select '1';" +
                             "end";
                SqlParameter[] parameters = { new SqlParameter("@Title", SqlDbType.NVarChar, 50), new SqlParameter("@ActionList", SqlDbType.NVarChar, -1), new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = model.Title;
                parameters[1].Value = model.ActionList;
                parameters[2].Value = model.ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "-1";
            }
        }
        public static Models.BD_BearActionsAllot Model(int id)
        {
            DataTable dt = DAL.PageModel.Table_Model("[BD_BearActionsAllot]", String.Format("ID={0}", id.ToString()));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Models.BD_BearActionsAllot m = new Models.BD_BearActionsAllot();
                m.ID = DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
                m.ActionList = dt.Rows[0]["ActionList"].ToString();
                m.Title = dt.Rows[0]["Title"].ToString();
                return m;
            }
        }

        /// <summary>
        /// 检查当前动作的授权
        /// </summary>
        /// <param name="mallAGid">商城管理员组id</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">动作方法名称</param>
        /// <returns></returns>
        public static bool CheckAuthority(int RoleID, string pageKey)
        {
            //非管理员
            if (RoleID == 0) return false;
            //系统管理员具有一切权限
            if (RoleID == 1) return true;
            HashSet<string> AdminActionHashSet = BD_BearAdminActions.GetAdminActionHashSet();//系统所有的action集合 格式为String.Format("{0}{1}", item.Controller, item.Action) 包含参数
            HashSet<string> AdminGroupActionHashSet = GetAdminGroupActionHashSet(RoleID);//角色有的action集合 格式为item.Controller+item.Action 包含参数
            if ((AdminActionHashSet.Contains(pageKey) && AdminGroupActionHashSet.Contains(pageKey)))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获得管理员组操作HashSet,已经去掉参数
        /// </summary>
        /// <param name="mallAGid">管理员组id</param>
        /// <returns></returns>
        public static HashSet<string> GetAdminGroupActionHashSet(int RoleID)
        {
            HashSet<string> actionHashSet = CacheManager.Get(CacheKeys.MALL_MALLADMINGROUP_ACTIONHASHSET + RoleID) as HashSet<string>;
            if (actionHashSet == null)
            {
                Models.BD_BearActionsAllot model = Model(RoleID);
                if (model != null)
                {
                    actionHashSet = new HashSet<string>();
                    string[] actionList = Common.SplitString(model.ActionList);//将动作列表字符串分隔成动作列表
                    foreach (string action in actionList)
                    {
                        if (action.IndexOf("?") > 0)
                        {
                            actionHashSet.Add(Common.SubString(action, action.IndexOf("?")).ToLower());
                        }
                        else actionHashSet.Add(action.ToLower());
                    }
                    CacheManager.Insert(CacheKeys.MALL_MALLADMINGROUP_ACTIONHASHSET + RoleID, actionHashSet);
                }
            }
            return actionHashSet;
        }
    }
}

