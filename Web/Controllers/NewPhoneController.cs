using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using AliceDAL;
using Models;
using Web.Models;
using Newtonsoft.Json;
namespace Web.Controllers
{
    public class NewPhoneController : Controller
    {
        public ActionResult Result(int userID)
        {
            //int count = DAL.PageModel.DataCount("Coupons", String.Format("[Uid]={0} and [TypeID]={1}", userID, 12));//12为新注册用户5元券的ID
            //if (count == 6)
            //{
            //    ViewBag.Msg = "恭喜您已获得<br/>30元<br />优惠券";
            //}
            //else
            //{
            //    ViewBag.Msg = "未知错误";
            //}
            return Content("活动已结束");
        }
    }
}