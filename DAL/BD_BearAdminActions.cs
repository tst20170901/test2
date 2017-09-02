using AliceDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace DAL
{
    public partial class BD_BearAdminActions
    {
        public static List<Models.BD_BearAdminActions> List()
        {
            List<Models.BD_BearAdminActions> AdminActionsList = CacheManager.Get(CacheKeys.ADMINACTIONSLIST) as List<Models.BD_BearAdminActions>;
            if (AdminActionsList == null)
            {
                AdminActionsList = new List<Models.BD_BearAdminActions>();
                DataTable dt = DAL.PageModel.DateList("[BD_BearAdminActions]", "", "SortID asc,ID", 0);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Models.BD_BearAdminActions model = new Models.BD_BearAdminActions();
                        model.Action = item["Action"].ToString();
                        model.ActionImg = item["ActionImg"].ToString();
                        model.Controller = item["Controller"].ToString();
                        model.ID = AliceDAL.DataType.ConvertToInt(item["ID"].ToString());
                        model.IsMenu = AliceDAL.DataType.ConvertToInt(item["IsMenu"].ToString());
                        model.ParentID = AliceDAL.DataType.ConvertToInt(item["ParentID"].ToString());
                        model.SortID = AliceDAL.DataType.ConvertToInt(item["SortID"].ToString());
                        model.Title = item["Title"].ToString();
                        AdminActionsList.Add(model);
                    }
                    CacheManager.Insert(CacheKeys.ADMINACTIONSLIST, AdminActionsList);
                }
            }
            return AdminActionsList;
        }
        public static List<Models.BD_BearAdminActions> GetAdminActionTree()
        {
            List<Models.BD_BearAdminActions> AdminActionTree = new List<Models.BD_BearAdminActions>();
            List<Models.BD_BearAdminActions> AdminActionList = List();
            CreateAdminActionTree(AdminActionList, AdminActionTree, 0);
            return AdminActionTree;
        }
        private static void CreateAdminActionTree(List<Models.BD_BearAdminActions> ActionList, List<Models.BD_BearAdminActions> ActionTree, int parentId)
        {
            foreach (Models.BD_BearAdminActions item in ActionList)
            {
                if (item.ParentID == parentId)
                {
                    ActionTree.Add(item);
                    CreateAdminActionTree(ActionList, ActionTree, item.ID);
                }
            }
        }

        /// <summary>
        /// 获得后台操作HashSet
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetAdminActionHashSet()
        {
            HashSet<string> actionHashSet = CacheManager.Get(CacheKeys.MALL_MALLADMINACTION_HASHSET) as HashSet<string>;
            if (actionHashSet == null)
            {
                actionHashSet = new HashSet<string>();
                List<Models.BD_BearAdminActions> AdminActionList = List();
                foreach (Models.BD_BearAdminActions item in AdminActionList)
                {
                    if (item.ParentID != 0 && !String.IsNullOrWhiteSpace(item.Action) && !String.IsNullOrWhiteSpace(item.Controller))
                    {
                        if (item.Action.IndexOf("?") > 0)
                        {
                            actionHashSet.Add(String.Format("{0}{1}", item.Controller, Common.SubString(item.Action, item.Action.IndexOf("?"))).ToLower());
                        }
                        else actionHashSet.Add(String.Format("{0}{1}", item.Controller, item.Action).ToLower());
                    }
                }
                CacheManager.Insert(CacheKeys.MALL_MALLADMINACTION_HASHSET, actionHashSet);
            }
            return actionHashSet;
        }
    }
}

