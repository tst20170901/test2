using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Alice.WebControls.Mvc;
using System.Text;

namespace Web.Areas.Models
{
    public class ListModel
    {
        public PagedList<DataRow> Page { get; set; }
        public static void CreatePage(ListModel model, string tbname, StringBuilder strsql, int page, int pagesize, string indexFields = "ID")
        {
            int total = 0;
            int startPageIndex = (page - 1) * pagesize + 1;
            DataTable dt = DAL.PageModel.DataByPage(tbname, strsql.ToString(), indexFields, startPageIndex, startPageIndex + pagesize - 1, out total);
            PagedList<DataRow> list = new PagedList<DataRow>(dt.Select(), page, pagesize, total);
            if (list.CurrentPageIndex > list.TotalPageCount)
            {
                list.CurrentPageIndex = list.TotalPageCount;
                dt = DAL.PageModel.DataByPage(tbname, strsql.ToString(), "ID", list.TotalPageCount, list.TotalPageCount + pagesize, out total);
                list = new PagedList<DataRow>(dt.Select(), list.TotalPageCount, pagesize, total);
            }
            model.Page = list;
        }
    }

}