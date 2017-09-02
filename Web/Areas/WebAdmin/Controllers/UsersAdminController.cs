using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using Web.Areas.Models;
using System.Text;
using System.IO;
using Models;
using System.Data;
using Web.Areas.WebWorker.Models;
using Newtonsoft.Json;
using AliceDAL;

namespace Web.Areas.WebAdmin.Controllers
{
    public class UsersAdminController : WebAdminBaseController
    {
        public ActionResult Index(string txtSDate, string txtEDate)
        {
            StringBuilder strsql = new StringBuilder(String.Format("[StoreID]={0}", WebAdminContext.BranchID));

            if (!String.IsNullOrWhiteSpace(txtSDate) && !String.IsNullOrWhiteSpace(txtEDate) && DataType.ConvertToDateTime(txtEDate) >= DataType.ConvertToDateTime(txtSDate))
            {
                txtEDate = DataType.ConvertToDateTime(txtEDate).AddDays(1).ToString("yyyy/MM/dd");
            }
            else
            {
                txtSDate = DateTime.Now.ToString("yyyy/MM/dd");
                txtEDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            }
            DataSet ds = DAL.Orders.AccountingOrders(WebAdminContext.BranchID, txtSDate, txtEDate);
            return View(ds);
        }
    }
}
