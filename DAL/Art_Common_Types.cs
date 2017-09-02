using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AliceDAL;
namespace DAL
{
    public class Art_Common_Types
    {
        public static List<Models.Art_Common_Types> List()
        {
            List<Models.Art_Common_Types> newstypelist = CacheManager.Get(CacheKeys.NEWSTYPELIST) as List<Models.Art_Common_Types>;
            if (newstypelist == null)
            {
                newstypelist = new List<Models.Art_Common_Types>();
                List<Models.Art_Common_Types> sourceList = GetList();
                CreateCategoryTree(sourceList, newstypelist, 0);
                CacheManager.Insert(CacheKeys.NEWSTYPELIST, newstypelist);
            }
            return newstypelist;
        }
        public static int GetCateIdByName(string name)
        {
            foreach (Models.Art_Common_Types categoryInfo in List())
            {
                if (categoryInfo.TypeName.Trim() == name)
                    return categoryInfo.ID;
            }
            return 0;
        }
        public static Models.Art_Common_Types GetCateIdByID(int ID)
        {
            foreach (Models.Art_Common_Types categoryInfo in List())
            {
                if (categoryInfo.ID == ID)
                    return categoryInfo;
            }
            return null;
        }
        public static List<Models.Art_Common_Types> GetChildTypesList(int ID, int layer, bool isAllChildren)
        {
            List<Models.Art_Common_Types> categoryList = new List<Models.Art_Common_Types>();

            if (ID > 0)
            {
                bool flag = false;
                if (isAllChildren)
                {
                    foreach (Models.Art_Common_Types categoryInfo in List())
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
                    foreach (Models.Art_Common_Types categoryInfo in List())
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
        private static void CreateCategoryTree(List<Models.Art_Common_Types> source, List<Models.Art_Common_Types> result, int parentID)
        {
            foreach (Models.Art_Common_Types item in source)
            {
                if (item.ParentID == parentID)
                {
                    result.Add(item);
                    CreateCategoryTree(source, result, item.ID);
                }
            }
        }
        private static List<Models.Art_Common_Types> GetList()
        {
            List<Models.Art_Common_Types> list = new List<Models.Art_Common_Types>();
            DataTable dt = PageModel.DateList("Art_Common_Types", "", "SortID", 0);
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Models.Art_Common_Types m = new Models.Art_Common_Types();
                    m.Haschild = DataType.ConvertToInt(item["Haschild"].ToString());
                    m.ID = DataType.ConvertToInt(item["ID"].ToString());
                    m.Layer = DataType.ConvertToInt(item["Layer"].ToString());
                    m.ParentID = DataType.ConvertToInt(item["ParentID"].ToString());
                    m.Path = item["Path"].ToString();
                    m.SortID = DataType.ConvertToInt(item["SortID"].ToString());
                    m.TypeName = item["TypeName"].ToString();
                    list.Add(m);
                }
            }
            return list;
        }
        public static void Add(Models.Art_Common_Types model)
        {
            if (model.ParentID > 0)
            {
                List<Models.Art_Common_Types> categoryList = List();
                Models.Art_Common_Types parentCategoryInfo = categoryList.Find(x => x.ID == model.ParentID);//父分类
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
            CacheManager.Remove(CacheKeys.NEWSTYPELIST);
        }
        /// <summary>
        /// 更新分类
        /// </summary>
        public static void Edit(Models.Art_Common_Types model, int oldParentID)
        {
            if (model.ParentID != oldParentID)//父分类改变时
            {
                //int changeLayer = 0;
                List<Models.Art_Common_Types> categoryList = List();
                Models.Art_Common_Types oldParentCategoryInfo = categoryList.Find(x => x.ID == oldParentID);//旧的父分类
                if (model.ParentID > 0)//非顶层分类时
                {
                    Models.Art_Common_Types newParentCategoryInfo = categoryList.Find(x => x.ID == model.ParentID);//新的父分类
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
                foreach (Models.Art_Common_Types info in categoryList.FindAll(x => x.ParentID == model.ID))
                {
                    UpdateChildCategoryLayerAndPath(categoryList, info, model.Layer, model.Path);
                }
            }
            Edit(model);
            CacheManager.Remove(CacheKeys.NEWSTYPELIST);
        }
        private static void UpdateChildCategoryLayerAndPath(List<Models.Art_Common_Types> categoryList, Models.Art_Common_Types categoryInfo, int changeLayer, string parentPath)
        {
            categoryInfo.Layer = changeLayer + 1;
            categoryInfo.Path = parentPath + "," + categoryInfo.ID;
            Edit(categoryInfo);
            foreach (Models.Art_Common_Types info in categoryList.FindAll(x => x.ParentID == categoryInfo.ID))
            {
                UpdateChildCategoryLayerAndPath(categoryList, info, categoryInfo.Layer, categoryInfo.Path);
            }
        }
        private static int Create(Models.Art_Common_Types model)
        {
            try
            {
                string sql = "insert into Art_Common_Types (TypeName,ParentID,SortID,Layer,Haschild,[Path]) values (@TypeName,@ParentID,@SortID,@Layer,@Haschild,@Path);SELECT SCOPE_IDENTITY();";
                SqlParameter[] parameters = { new SqlParameter("@TypeName", SqlDbType.NVarChar, 100), new SqlParameter("@ParentID", SqlDbType.Int, 4), new SqlParameter("@SortID", SqlDbType.Int, 4), new SqlParameter("@Layer", SqlDbType.TinyInt, 1), new SqlParameter("@Haschild", SqlDbType.TinyInt, 4), new SqlParameter("@Path", SqlDbType.Char, 100) };
                parameters[0].Value = model.TypeName;
                parameters[1].Value = model.ParentID;
                parameters[2].Value = model.SortID;
                parameters[3].Value = model.Layer;
                parameters[4].Value = model.Haschild;
                parameters[5].Value = model.Path;
                return int.Parse(SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString());
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        private static int Edit(Models.Art_Common_Types model)
        {
            try
            {
                string sql = "update Art_Common_Types set TypeName=@TypeName,ParentID=@ParentID,SortID=@SortID,Layer=@Layer,Haschild=@Haschild,[Path]=@Path where ID=@ID;";
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
            List<Models.Art_Common_Types> categoryList = List();
            Models.Art_Common_Types categoryInfo = categoryList.Find(x => x.ID == ID);
            if (categoryInfo == null)
                return -3;
            if (categoryInfo.Haschild == 1)
                return -2;
            if (PageModel.DataCount("Art_Common", String.Format("TypeID={0}", ID)) > 0)
                return 0;
            Delete(ID);
            if (categoryInfo.Layer > 1 && categoryList.FindAll(x => x.ParentID == categoryInfo.ParentID).Count == 1)
            {
                Models.Art_Common_Types parentCategoryInfo = categoryList.Find(x => x.ID == categoryInfo.ParentID);
                parentCategoryInfo.Haschild = 0;
                Edit(parentCategoryInfo);
            }
            CacheManager.Remove(CacheKeys.NEWSTYPELIST);
            return 1;
        }
        private static bool Delete(int ID)
        {
            try
            {
                string sql = "delete from Art_Common_Types where ID=@ID;";
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
