using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using AliceDAL;
namespace DAL
{
    public partial class CarModels
    {
        public static List<Models.CarModels> GetList()
        {
            DataTable dt = DAL.PageModel.DateList("[CarModels]", "", "ID", "asc", 0);
            List<Models.CarModels> list = new List<Models.CarModels>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Models.CarModels m = new Models.CarModels();
                    m.Brand = row["Brand"].ToString();
                    m.BrandPinYin = row["BrandPinYin"].ToString();
                    m.ID = DataType.ConvertToInt(row["ID"].ToString());
                    m.Name = row["Name"].ToString();
                    m.ParentID = DataType.ConvertToInt(row["ParentID"].ToString());
                    m.PinYin = row["PinYin"].ToString();
                    list.Add(m);
                }
            }
            return list;
        }

        public static Models.CarModels GetModel(int cid)
        {
            DataTable dt = DAL.PageModel.Table_Model("[CarModels]", String.Format("ID={0}", cid.ToString()));
            if (dt == null) return null;
            var m = (from field in dt.AsEnumerable()
                     select new Models.CarModels()
                     {
                         Name = field.Field<string>("Name"),
                         PinYin = field.Field<string>("PinYin"),
                         ParentID = field.Field<int>("ParentID"),
                         Brand = field.Field<string>("Brand"),
                         BrandPinYin = field.Field<string>("BrandPinYin"),
                         ID = field.Field<int>("ID")
                     }).ToList().First();
            return m;
        }
    }
}

