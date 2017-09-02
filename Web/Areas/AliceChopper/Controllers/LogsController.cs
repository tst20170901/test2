using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Text;
using Web.Areas.Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class LogsController : AdminBaseController
    {
        public ActionResult List(string loginID, string nickname, string desc, string starttime, string endtime, int BranchID = -1, int page = 1)
        {

            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder();
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));

            if (!String.IsNullOrEmpty(loginID)) AliceDAL.Common.SqlString(strsql, String.Format("LoginID like '%{0}%'", AliceDAL.Common.cutBadStr(loginID)));
            if (!String.IsNullOrEmpty(nickname)) AliceDAL.Common.SqlString(strsql, String.Format("NickName like '%{0}%'", AliceDAL.Common.cutBadStr(nickname)));
            if (!String.IsNullOrEmpty(desc)) AliceDAL.Common.SqlString(strsql, String.Format("[Description] like '%{0}%'", AliceDAL.Common.cutBadStr(desc)));
            if (!String.IsNullOrEmpty(starttime)) AliceDAL.Common.SqlString(strsql, String.Format("CDate>='{0}'", AliceDAL.DataType.ConvertToDateTimeStrAll(starttime)));
            if (!String.IsNullOrEmpty(endtime)) AliceDAL.Common.SqlString(strsql, String.Format("CDate<='{0}'", AliceDAL.DataType.ConvertToDateTimeStrAll(endtime)));
            ListModel.CreatePage(model, "[BD_Logs]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
    }
}
