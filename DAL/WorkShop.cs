using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class WorkShop
    {
        public static int Edit(Models.WorkShop model)
        {
            try
            {
                string sql = "if exists (select 1 from [WorkShop] where [Mobile]=@Mobile and [ID]<>@ID) " +
                             "begin " +
                               "select 'exists mobile';" +
                             "end " +
                             "else " +
                             "begin " +
                             "  Update [WorkShop] set [Img]=@Img,[Title]=@Title,[Mobile]=@Mobile,[UserPwd]=@UserPwd,[WorkState]=@WorkState,[WorkRadius]=@WorkRadius,[Province]=@Province,[City]=@City,[CityCode]=@CityCode,[District]=@District,[Adcode]=@Adcode,[CostTime]=@CostTime,[Lng]=@Lng,[Lng1]=@Lng1,[Lat]=@Lat,[Lat1]=@Lat1,[StartTime]=@StartTime,[EndTime]=@EndTime,[Phone]=@Phone,[BranchID]=@BranchID where ID=@ID;" +
                             "  select 'ok';end";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@WorkState", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@WorkRadius", SqlDbType.Int, 4), 
                                                new SqlParameter("@Province", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@City", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@CityCode", SqlDbType.NVarChar,10), 
                                                new SqlParameter("@District", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Adcode", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CostTime", SqlDbType.Int,4 ), 
                                                new SqlParameter("@Lng", SqlDbType.NVarChar,50),
                                                new SqlParameter("@Lng1", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Lat", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Lat1", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@StartTime", SqlDbType.DateTime), 
                                                new SqlParameter("@EndTime", SqlDbType.DateTime),
                                                new SqlParameter("@ID",SqlDbType.Int,4),
                                                new SqlParameter("@Phone",SqlDbType.NVarChar,50),
                                                new SqlParameter("@BranchID",SqlDbType.Int,4),
                                                new SqlParameter("@Img",SqlDbType.NVarChar,100)
                                            };
                parameters[0].Value = model.Title;
                parameters[1].Value = model.Mobile;
                parameters[2].Value = model.UserPwd;
                parameters[3].Value = model.WorkState;
                parameters[4].Value = model.WorkRadius;
                parameters[5].Value = model.Province;
                parameters[6].Value = model.City;
                parameters[7].Value = model.CityCode;
                parameters[8].Value = model.District;
                parameters[9].Value = model.Adcode;
                parameters[10].Value = model.CostTime;
                parameters[11].Value = model.Lng;
                parameters[12].Value = model.Lng1;
                parameters[13].Value = model.Lat;
                parameters[14].Value = model.Lat1;
                parameters[15].Value = model.StartTime;
                parameters[16].Value = model.EndTime;
                parameters[17].Value = model.ID;
                parameters[18].Value = model.Phone;
                parameters[19].Value = model.BranchID;
                parameters[20].Value = model.Img;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static int ReSet(int wid)
        {
            try
            {
                string sql = "Update [WorkShop] set [Lng1]=[Lng],[Lat1]=[Lat] where ID=@ID;";

                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = wid;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static string Add(Models.WorkShop model)
        {
            try
            {
                string sql = "if exists (select 1 from WorkShop where Mobile=@Mobile) " +
                             "begin " +
                               "select 'exists mobile';" +
                             "end " +
                             "else " +
                             "begin " +
                                "DECLARE @oid int;INSERT INTO [WorkShop] ([Title],[Mobile],[UserPwd],[WorkState],[WorkRadius],[Province],[City],[CityCode],[District],[Adcode],[CostTime],[Lng],[Lng1],[Lat],[Lat1],[StartTime],[EndTime],[BranchID],[TypeID],[Phone],[Img])VALUES(@Title,@Mobile,@UserPwd,@WorkState,@WorkRadius,@Province,@City,@CityCode,@District,@Adcode,@CostTime,@Lng,@Lng1,@Lat,@Lat1,@StartTime,@EndTime,@BranchID,@TypeID,@Phone,@Img);" +
                                "SET @oid=SCOPE_IDENTITY();SELECT @oid AS 'oid';" +
                             "end";

                SqlParameter[] parameters = { 
                                                new SqlParameter("@Title", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Mobile", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@UserPwd", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@WorkState", SqlDbType.TinyInt, 1),
                                                new SqlParameter("@WorkRadius", SqlDbType.Int, 4), 
                                                new SqlParameter("@Province", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@City", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@CityCode", SqlDbType.NVarChar,10), 
                                                new SqlParameter("@District", SqlDbType.NVarChar, 50), 
                                                new SqlParameter("@Adcode", SqlDbType.NVarChar, 10), 
                                                new SqlParameter("@CostTime", SqlDbType.Int,4 ), 
                                                new SqlParameter("@Lng", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Lng1", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Lat", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@Lat1", SqlDbType.NVarChar,50), 
                                                new SqlParameter("@StartTime", SqlDbType.DateTime), 
                                                new SqlParameter("@EndTime", SqlDbType.DateTime),
                                                new SqlParameter("@BranchID",SqlDbType.Int,4),
                                                new SqlParameter("@TypeID",SqlDbType.TinyInt,1),
                                                new SqlParameter("@Phone", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Img",SqlDbType.NVarChar,100)
                                            };
                parameters[0].Value = model.Title;
                parameters[1].Value = model.Mobile;
                parameters[2].Value = model.UserPwd;
                parameters[3].Value = model.WorkState;
                parameters[4].Value = model.WorkRadius;
                parameters[5].Value = model.Province;
                parameters[6].Value = model.City;
                parameters[7].Value = model.CityCode;
                parameters[8].Value = model.District;
                parameters[9].Value = model.Adcode;
                parameters[10].Value = model.CostTime;
                parameters[11].Value = model.Lng;
                parameters[12].Value = model.Lng1;
                parameters[13].Value = model.Lat;
                parameters[14].Value = model.Lat1;
                parameters[15].Value = model.StartTime;
                parameters[16].Value = model.EndTime;
                parameters[17].Value = model.BranchID;
                parameters[18].Value = model.TypeID;
                parameters[19].Value = model.Phone;
                parameters[20].Value = model.Img;
                return SQLHelper.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return "error";
            }
        }
        public Models.WorkShop Login(string loginID, string password, out string result)
        {
            try
            {
                string str = "select * from [WorkShop] where Mobile=@Mobile;";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Mobile", loginID);
                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == reader["UserPwd"].ToString())
                    {
                        Models.WorkShop ws = new Models.WorkShop();
                        ws.Adcode = reader["Adcode"].ToString();
                        ws.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        ws.ReDate = DataType.ConvertToDateTime(reader["ReDate"].ToString());
                        ws.City = reader["City"].ToString();
                        ws.CityCode = reader["CityCode"].ToString();
                        ws.District = reader["District"].ToString();
                        ws.EndTime = DataType.ConvertToDateTime(reader["EndTime"].ToString());
                        ws.ID = DataType.ConvertToInt(reader["ID"].ToString());
                        ws.Lat = reader["Lat"].ToString();
                        ws.Lng = reader["Lng"].ToString();
                        ws.Lat1 = reader["Lat1"].ToString();
                        ws.Lng1 = reader["Lng1"].ToString();
                        ws.Province = reader["Province"].ToString();
                        ws.StartTime = DataType.ConvertToDateTime(reader["StartTime"].ToString());
                        ws.Title = reader["Title"].ToString();
                        ws.Mobile = reader["Mobile"].ToString();
                        ws.UserPwd = reader["UserPwd"].ToString();
                        ws.WorkState = DataType.ConvertToInt(reader["WorkState"].ToString());
                        ws.WorkRadius = DataType.ConvertToInt(reader["WorkRadius"].ToString());
                        ws.CostTime = DataType.ConvertToInt(reader["CostTime"].ToString());
                        ws.BranchID = DataType.ConvertToInt(reader["BranchID"].ToString());
                        ws.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        ws.Phone = reader["Phone"].ToString();
                        ws.Img = reader["Img"].ToString();
                        result = "";
                        reader.Close();
                        return ws;
                    }
                    else result = "密码不正确！";
                }
                else result = "用户名不存在！";
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                result = "系统异常！";
                return null;
            }
        }
        private static Models.WorkShop BuildWorkShop(DataRow row)
        {
            Models.WorkShop ws = new Models.WorkShop();
            if (row != null)
            {
                ws.Adcode = row["Adcode"].ToString();
                ws.CDate = DataType.ConvertToDateTime(row["CDate"].ToString());
                ws.ReDate = DataType.ConvertToDateTime(row["ReDate"].ToString());
                ws.City = row["City"].ToString();
                ws.CityCode = row["CityCode"].ToString();
                ws.District = row["District"].ToString();
                ws.EndTime = DataType.ConvertToDateTime(row["EndTime"].ToString());
                ws.ID = DataType.ConvertToInt(row["ID"].ToString());
                ws.Lat = row["Lat"].ToString();
                ws.Lng = row["Lng"].ToString();
                ws.Lat1 = row["Lat1"].ToString();
                ws.Lng1 = row["Lng1"].ToString();
                ws.Province = row["Province"].ToString();
                ws.StartTime = DataType.ConvertToDateTime(row["StartTime"].ToString());
                ws.Title = row["Title"].ToString();
                ws.Mobile = row["Mobile"].ToString();
                ws.UserPwd = row["UserPwd"].ToString();
                ws.WorkState = DataType.ConvertToInt(row["WorkState"].ToString());
                ws.WorkRadius = DataType.ConvertToInt(row["WorkRadius"].ToString());
                ws.CostTime = DataType.ConvertToInt(row["CostTime"].ToString());
                ws.BranchID = DataType.ConvertToInt(row["BranchID"].ToString());
                ws.TypeID = DataType.ConvertToInt(row["TypeID"].ToString());
                ws.Phone = row["Phone"].ToString();
                ws.Img = row["Img"].ToString();
            }
            return ws;
        }
        public static Models.WorkShop GetUserByApp(int Uid, string password)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                string str = "select * from [WorkShop] where ID=@ID;";
                param[0] = new SqlParameter("@ID", Uid);

                SqlDataReader reader = SQLHelper.ExecuteReader(CommandType.Text, str, param);
                if (reader.Read())
                {
                    if (password == AliceDAL.SecureHelper.MD5(reader["UserPwd"].ToString() + reader["ID"].ToString()))
                    {
                        Models.WorkShop ws = new Models.WorkShop();
                        ws.Adcode = reader["Adcode"].ToString();
                        ws.CDate = DataType.ConvertToDateTime(reader["CDate"].ToString());
                        ws.ReDate = DataType.ConvertToDateTime(reader["ReDate"].ToString());
                        ws.City = reader["City"].ToString();
                        ws.CityCode = reader["CityCode"].ToString();
                        ws.District = reader["District"].ToString();
                        ws.EndTime = DataType.ConvertToDateTime(reader["EndTime"].ToString());
                        ws.ID = DataType.ConvertToInt(reader["ID"].ToString());
                        ws.Lat = reader["Lat"].ToString();
                        ws.Lng = reader["Lng"].ToString();
                        ws.Lat1 = reader["Lat1"].ToString();
                        ws.Lng1 = reader["Lng1"].ToString();
                        ws.Province = reader["Province"].ToString();
                        ws.StartTime = DataType.ConvertToDateTime(reader["StartTime"].ToString());
                        ws.Title = reader["Title"].ToString();
                        ws.Mobile = reader["Mobile"].ToString();
                        ws.UserPwd = reader["UserPwd"].ToString();
                        ws.WorkState = DataType.ConvertToInt(reader["WorkState"].ToString());
                        ws.WorkRadius = DataType.ConvertToInt(reader["WorkRadius"].ToString());
                        ws.CostTime = DataType.ConvertToInt(reader["CostTime"].ToString());
                        ws.BranchID = DataType.ConvertToInt(reader["BranchID"].ToString());
                        ws.TypeID = DataType.ConvertToInt(reader["TypeID"].ToString());
                        ws.Phone = reader["Phone"].ToString();
                        ws.Img = reader["Img"].ToString();
                        reader.Close();
                        return ws;
                    }
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
        public static int PostLocation(int Uid, string Lat, string Lng)
        {
            try
            {
                string sql = "Update [WorkShop] set [Lng1]=@Lng1,[Lat1]=@Lat1,[ReDate]=getdate() where ID=@ID;";

                SqlParameter[] parameters = { new SqlParameter("@Lng1", SqlDbType.NVarChar, 50), new SqlParameter("@Lat1", SqlDbType.NVarChar, 50), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = Lng;
                parameters[1].Value = Lat;
                parameters[2].Value = Uid;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        public static Models.WorkShop WorkShopForCode(string co, string ad)
        {
            try
            {
                string sql = "select * from [WorkShop] where [TypeID]=1 and CityCode=@CityCode and Adcode=@Adcode;";
                SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10), new SqlParameter("@Adcode", SqlDbType.NVarChar, 10) };

                parameters[0].Value = co;
                parameters[1].Value = ad;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return BuildWorkShop(dt.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.WorkShop WorkShopForMobile(string mobile)
        {
            try
            {
                string sql = "select top 1 * from [WorkShop] where [Mobile]=@Mobile or [Phone]=@Mobile;";
                SqlParameter[] parameters = { new SqlParameter("@Mobile", SqlDbType.NVarChar, 50) };
                parameters[0].Value = mobile;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return BuildWorkShop(dt.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static int ChangeWorkShop(int wid, Models.WorkShopState shopState)
        {
            try
            {
                string sql = "UPDATE [WorkShop] SET [WorkState]=@WorkState WHERE [ID]=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@WorkState", SqlDbType.TinyInt, 1), new SqlParameter("@ID", SqlDbType.Int, 4) };
                parameters[0].Value = (int)shopState;
                parameters[1].Value = wid;
                return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 获取本城市可用并且在设定时间间隔内在线的洗车工列表
        /// </summary>
        /// <param name="co"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static List<Models.WorkShop> WorkShopForCode(string co, string ad, int state)
        {
            try
            {
                //string sql = "select top 100 * from [WorkShop] where CityCode=@CityCode and Adcode=@Adcode and (WorkState=10 or WorkState=30) and datediff(second, ReDate,GETDATE())<=@TS;";
                string sql = "select top 100 * from [WorkShop] where [TypeID]=1 and CityCode=@CityCode and Adcode=@Adcode and WorkState=@WorkState;";
                //SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10), new SqlParameter("@Adcode", SqlDbType.NVarChar, 10), new SqlParameter("@TS", SqlDbType.Int, 4) };
                SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10), new SqlParameter("@Adcode", SqlDbType.NVarChar, 10), new SqlParameter("@WorkState", SqlDbType.TinyInt, 1) };
                parameters[0].Value = co;
                parameters[1].Value = ad;
                parameters[2].Value = state;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Models.WorkShop> list = new List<Models.WorkShop>();
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(BuildWorkShop(row));
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }

        public static List<Models.WorkShop> CarWashForCode(string co, string ad)
        {
            try
            {
                string sql = "select ID,Title,Phone,Img,Mobile,District,Lng,BranchID,Lat,StartTime,EndTime,CostTime,(select COUNT(1) from Orders where WorkID=w.ID and(OrderState=10 or OrderState=30 or OrderState=50)) as OrderCount from WorkShop w where [TypeID]=2 and CityCode=@CityCode and Adcode=@Adcode and ([WorkState]=10 or [WorkState]=30)";
                SqlParameter[] parameters = { new SqlParameter("@CityCode", SqlDbType.NVarChar, 10), new SqlParameter("@Adcode", SqlDbType.NVarChar, 10) };
                parameters[0].Value = co;
                parameters[1].Value = ad;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Models.WorkShop> list = new List<Models.WorkShop>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Models.WorkShop ws = new Models.WorkShop();
                        ws.District = row["District"].ToString();
                        ws.ET = DataType.ConvertToDateTime(row["EndTime"].ToString()).ToString("HH:mm");
                        ws.ID = DataType.ConvertToInt(row["ID"].ToString());
                        ws.Lat = row["Lat"].ToString();
                        ws.Lng = row["Lng"].ToString();
                        ws.ST = DataType.ConvertToDateTime(row["StartTime"].ToString()).ToString("HH:mm");
                        ws.Title = row["Title"].ToString();
                        ws.Mobile = row["Mobile"].ToString();
                        ws.CostTime = DataType.ConvertToInt(row["CostTime"].ToString());
                        ws.OrderCount = DataType.ConvertToInt(row["OrderCount"].ToString());
                        ws.OrderTime = ws.CostTime * ws.OrderCount;
                        ws.Phone = row["Phone"].ToString();
                        ws.BranchID = DataType.ConvertToInt(row["BranchID"].ToString());
                        ws.Img = row["Img"].ToString();
                        list.Add(ws);
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
        public static Models.WorkShop WorkShopByID(int id)
        {
            try
            {
                string sql = "select * from [WorkShop] where ID=@ID;";
                SqlParameter[] parameters = { new SqlParameter("@ID", SqlDbType.Int, 4) };

                parameters[0].Value = id;
                DataTable dt = SQLHelper.ExecuteDataTable(CommandType.Text, sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return BuildWorkShop(dt.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                AliceDAL.Error.WriteErrorLog(ex);
                return null;
            }
        }
    }
}

