using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public class WebWorkerContext
    {
        public bool IsHttpAjax;
        public int PageSize;
        public string IP;
        public int ID = -1;
        public string Title;
        public string Mobile;
        public string Phone;
        public string UserPwd;
        public int BranchID;
        public int TypeID;
        public int WorkState;
        public string Lat;
        public string Lng;
        public DateTime ReDate;
        public BD_Branch Branch;
    }
}