using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AliceDAL;
namespace DAL
{
    public partial class Pro_Types
    {
        public static List<Models.Pro_Types> List()
        {
            List<Models.Pro_Types> categorylist = CacheManager.Get(CacheKeys.CATEGORYLIST) as List<Models.Pro_Types>;
            if (categorylist == null)
            {
                categorylist = new List<Models.Pro_Types>();
                List<Models.Pro_Types> sourceList = GetList();
                CreateCategoryTree(sourceList, categorylist, 0);
                CacheManager.Insert(CacheKeys.CATEGORYLIST, categorylist);
            }
            return categorylist;
        }
        public static int GetCateIdByName(string name)
        {
            //foreach (Models.Pro_Types categoryInfo in List())
            //{
            //    if (categoryInfo.TypeName.Trim() == name)
            //        return categoryInfo.ID;
            //}
            return 0;
        }
        public static Models.Pro_Types GetCateIdByID(int ID)
        {
            foreach (Models.Pro_Types categoryInfo in List())
            {
                if (categoryInfo.ID == ID)
                    return categoryInfo;
            }
            return null;
        }
        public static List<int> GetChildTypesID(int ID, int layer)
        {
            List<Models.Pro_Types> list = GetChildTypesList(ID, layer, true);
            List<int> result = new List<int>();
            foreach (Models.Pro_Types item in list)
            {
                result.Add(item.ID);
            }
            result.Add(ID);
            return result;
        }

        public static List<Models.Pro_Types> GetChildTypesList(int ID, int layer, bool isAllChildren)
        {
            List<Models.Pro_Types> categoryList = new List<Models.Pro_Types>();

            if (ID > 0)
            {
                bool flag = false;
                if (isAllChildren)
                {
                    foreach (Models.Pro_Types categoryInfo in List())
                    {
                        if (flag && categoryInfo.Layer == layer)
                        {
                            flag = false;
                        }
                        if (flag || categoryInfo.ParentID == ID)
                        {
                            flag = true;
                            categoryList.Add(categoryInfo);
                        }
                    }
                }
                else
                {
                    foreach (Models.Pro_Types categoryInfo in List())
                    {
                        if (categoryInfo.ParentID == ID)
                        {
                            flag = true;
                            categoryList.Add(categoryInfo);
                        }
                        else if (flag && categoryInfo.Layer == layer)
                        {
                            break;
                        }
                    }
                }
            }
            return categoryList;
        }
        private static void CreateCategoryTree(List<Models.Pro_Types> source, List<Models.Pro_Types> result, int parentID)
        {
            foreach (Models.Pro_Types item in source)
            {
                if (item.ParentID == parentID)
                {
                    result.Add(item);
                    CreateCategoryTree(source, result, item.ID);
                }
            }
        }
        private static List<Models.Pro_Types> GetList()
        {
            List<Models.Pro_Types> list = new List<Models.Pro_Types>();
            DataTable dt = PageModel.DateList("Pro_Types", "", "SortID", 0);
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Models.Pro_Types m = new Models.Pro_Types();
                    m.Haschild = DataType.ConvertToInt(item["Haschild"].ToString());
                    m.ID = DataType.ConvertToInt(item["ID"].ToString());
                    m.Layer = DataType.ConvertToInt(item["Layer"].ToString());
                    m.ParentID = DataType.ConvertToInt(item["ParentID"].ToString());
                    m.Path = item["Path"].ToString();
                    m.SortID = DataType.ConvertToInt(item["SortID"].ToString());
                    m.TypeName = item["TypeName"].ToString();
                    m.BusinessID = DataType.ConvertToInt(item["BusinessID"].ToString());
                    list.Add(m);
                }
            }
            return list;
        }
        public static List<Models.Pro_Types> GetList(string where)
        {
            List<Models.Pro_Types> list = new List<Models.Pro_Types>();
            DataTable dt = PageModel.DateList("Pro_Types", where, "SortID", 0);
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Models.Pro_Types m = new Models.Pro_Types();
                    m.Haschild = DataType.ConvertToInt(item["Haschild"].ToString());
                    m.ID = DataType.ConvertToInt(item["ID"].ToString());
                    m.Layer = DataType.ConvertToInt(item["Layer"].ToString());
                    m.ParentID = DataType.ConvertToInt(item["ParentID"].ToString());
                    m.Path = item["Path"].ToString();
                    m.SortID = DataType.ConvertToInt(item["SortID"].ToString());
                    m.TypeName = item["TypeName"].ToString();
                    m.BusinessID = DataType.ConvertToInt(item["BusinessID"].ToString());
                    list.Add(m);
                }
            }
            return list;
        }
        public static void Add(Models.Pro_Types model)
        {
            if (model.ParentID > 0)
            {
                List<Models.Pro_Types> categoryList = List();
                Models.Pro_Types parentCategoryInfo = categoryList.Find(x => x.ID == model.ParentID);//父分类
                model.Layer = parentCategoryInfo.Layer + 1;
                model.Haschild = 0;
                model.Path = "";
                int categoryId = Create(model);

                model.ID = categoryId;
                model.Path = parentCategoryInfo.Path + "," + categoryId;
                Edit(model);

                if (parentCategoryInfo.Haschild == 0)
                {
                    parentCategoryInfo.Haschild = 1;
                    Edit(parentCategoryInfo);
                }
            }
            else
            {
                model.Layer = 1;
                model.Haschild = 0;
                model.Path = "";
                int categoryId = Create(model);
                model.ID = categoryId;
                model.Path = categoryId.ToString();
                Edit(model);
            }
            CacheManager.Remove(CacheKeys.CATEGORYLIST);
        }
        /// <summary>
        /// 更新分类
        /// </summary>
        public static void Edit(Models.Pro_Types model, int oldParentID)
        {
            if (model.ParentID != oldParentID)//父分类改变时
            {
                //int changeLayer = 0;
                List<Models.Pro_Types> categoryList = List();
                Models.Pro_Types oldParentCategoryInfo = categoryList.Find(x => x.ID == oldParentID);//旧的父分类
                if (model.ParentID > 0)//非顶层分类时
                {
                    Models.Pro_Types newParentCategoryInfo = categoryList.Find(x => x.ID == model.ParentID);//新的父分类
                    //if (oldParentCategoryInfo == null)
                    //{
                    //    changeLayer = newParentCategoryInfo.Layer + 1;
                    //}
                    //else
                    //{
                    //    changeLayer = newParentCategoryInfo.Layer - oldParentCategoryInfo.Layer;
                    //}
                    model.Layer = newParentCategoryInfo.Layer + 1;
                    model.Path = newParentCategoryInfo.Path + "," + model.ID;

                    if (newParentCategoryInfo.Haschild == 0)
                    {
                        newParentCategoryInfo.Haschild = 1;
                        Edit(newParentCategoryInfo);
                    }
                }
                else
                {
                    model.Layer = 1;
                    model.Path = model.ID.ToString();
                }

                if (oldParentID > 0 && categoryList.FindAll(x => x.ParentID == oldParentID).Count == 0)
                {
                    oldParentCategoryInfo.Haschild = 0;
                    Edit(oldParentCategoryInfo);
                }
                foreach (Models.Pro_Types info in categoryList.FindAll(x => x.ParentID == model.ID))
                {
                    UpdateChildCategoryLayerAndPath(categoryList, info, model.Layer, model.Path);
                }
            }
            Edit(model);
            CacheManager.Remove(CacheKeys.CATEGORYLIST);
        }
        private static void UpdateChildCategoryLayerAndPath(List<Models.Pro_Types> categoryList, Models.Pro_Types categoryInfo, int changeLayer, string parentPath)
        {
            categoryInfo.Layer = changeLayer + 1;
            categoryInfo.Path = parentPath + "," + categoryInfo.ID;
            Edit(categoryInfo);
            foreach (Models.Pro_Types info in categoryList.FindAll(x => x.ParentID == categoryInfo.ID))
            {
                UpdateChildCategoryLayerAndPath(categoryList, info, categoryInfo.Layer, categoryInfo.Path);
            }
        }
        private static int Create(Models.Pro_Types model)
        {
            try
            {
                string sql = "insert into Pro_Types (TypeName,ParentID,SortID,Layer,Haschild,[Path],[BusinessID]) values (@TypeName,@ParentID,@SortID,@Layer,@Haschild,@Path,@BusinessID);SELECT SCOPE_IDENTITY();";
                SqlParameter[] parameters = { new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@Layer", SqlDbType.TinyInt, 1), new SqlParameter("@Haschild", SqlDbType.TinyInt, 4), new SqlParameter("@Path", SqlDbType.Char, 100), new SqlParameter("@BusinessID", SqlDbType.Int, 4) };
                parameters[0].Value = model.TypeName;
                parameters[1].Value = model.ParentID;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.Layer;
                parameters[4].Value = model.Haschild;
                parameters[5].Value = model.Path;
                parameters[6].Value = model.BusinessID;
                return int.Parse(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        private static int Edit(Models.Pro_Types model)
        {
            try
            {
                string sql = "update Pro_Types set TypeName=@TypeName,ParentID=@ParentID,SortID=@SortID,Layer=@Layer,Haschild=@Haschild,[Path]=@Path where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@Layer", SqlDbType.TinyInt, 1), new SqlParameter("@Haschild", SqlDbType.TinyInt, 4), new SqlParameter("@Path", SqlDbType.Char, 100), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = model.TypeName;
                parameters[1].Value = model.ParentID;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.Layer;
                parameters[4].Value = model.Haschild;
                parameters[5].Value = model.Path;
                parameters[6].Value = model.ID;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static int DeleteByID(int ID)
        {
            List<Models.Pro_Types> categoryList = List();
            Models.Pro_Types categoryInfo = categoryList.Find(x => x.ID == ID);
            if (categoryInfo == null)
                return -3;
            if (categoryInfo.Haschild == 1)
                return -2;
            if (PageModel.DataCount("Pro_Case", String.Format("TypeID={0}", ID)) > 0)
                return 0;
            Delete(ID);
            if (categoryInfo.Layer > 1 && categoryList.FindAll(x => x.ParentID == categoryInfo.ParentID).Count == 1)
            {
                Models.Pro_Types parentCategoryInfo = categoryList.Find(x => x.ID == categoryInfo.ParentID);
                parentCategoryInfo.Haschild = 0;
                Edit(parentCategoryInfo);
            }
            CacheManager.Remove(CacheKeys.CATEGORYLIST);
            return 1;
        }
        private static bool Delete(int ID)
        {
            try
            {
                string sql = "delete from Pro_Types where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = ID;
                return SQLHelper.ExecuteTransactionSQL(sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return false;
            }
        }
    }
}

