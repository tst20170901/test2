using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using System.Net;
using System.IO;
using System.Text;
using Web.Models;
using Newtonsoft.Json;
using Models;
using AliceDAL;
namespace Web.Controllers
{
    public class UserCarController : UserBaseController
    {
        private static object _locker = new object();//锁对象
        public ActionResult AddCar()
        {
            lock (_locker)
            {
                MessageModel mm = new MessageModel();
                string plate = AliceDAL.Common.GetFormString("aliceplatea") + AliceDAL.Common.GetFormString("aliceplateb") + AliceDAL.Common.GetFormString("aliceplatec");
                string brand = AliceDAL.Common.GetFormString("alicebrand");
                string model = AliceDAL.Common.GetFormString("alicemodel");
                string color = AliceDAL.Common.GetFormString("alicecolor");
                int brandid = AliceDAL.Common.GetFormInt("alicebrandid");
                int modelid = AliceDAL.Common.GetFormInt("alicemodelid");

                if (String.IsNullOrWhiteSpace(plate))
                {
                    mm.code = "0";
                    mm.msg = "车牌号不能为空";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                else if (!AliceDAL.ValidateHelper.IsCarNo(plate))
                {
                    mm.code = "0";
                    mm.msg = "车牌号不正确";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                UserCar uc = new UserCar()
                {
                    BrandID = brandid,
                    BrandShow = brand + " " + model,
                    Color = color ?? "",
                    Data1 = "",
                    Data2 = "",
                    Data3 = "",
                    Data4 = "",
                    Data5 = "",
                    ModelID = modelid,
                    Plate = plate,
                    UserID = WorkContext.ID,
                    UserName = WorkContext.Data1
                };
                string result = DAL.UserCar.Add(uc);
                if (result == "exists")
                {
                    mm.code = "0";
                    mm.msg = "此车辆已经存在";
                    return Content(JsonConvert.SerializeObject(mm));
                }
                return Content(JsonConvert.SerializeObject(mm));
            }
        }
        public ActionResult DeleteCar()
        {
            MessageModel mm = new MessageModel();
            int aliceid = AliceDAL.Common.GetFormInt("alicecarid");
            UserCar uc = DAL.UserCar.GetModel(aliceid);
            if (uc == null || uc.UserID != WorkContext.ID)
            {
                mm.code = "0";
                mm.msg = "此车辆不存在";
                return Content(JsonConvert.SerializeObject(mm));
            }
            int result = DAL.PageModel.Delete("UserCar", "ID=" + aliceid.ToString());
            if (result <= 0)
            {
                mm.code = "0";
                mm.msg = "删除失败，请稍候再试";
                return Content(JsonConvert.SerializeObject(mm));
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult GetCarList()
        {
            int page = Common.GetFormInt("page");
            MessageModel mm = new MessageModel();
            mm.msg = "没有了";
            mm.code = "0";
            List<UserCarModel> list = Comm.GetCarList(WorkContext.ID, page);
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (UserCarModel item in list)
                {
                    sb.Append("<div class=\"caritem\">" +
                              "  <div class=\"caritembd\">" +
                              "      <h1 class=\"carno\">车牌：" + item.Plate + "</h1>" +
                              "  <div class=\"carxilie\">车型：" + item.BrandShow + "</div>" +
                              "  <div class=\"color\">颜色：" + item.Color + " <div class=\"del\"><a href=\"javascript:void(0)\" onclick=\"delcar(" + item.ID + ")\">删除</a></div></div>" +
                              "</div>" +
                              "  <div class=\"caritemcbx\">" +
                              "  <label class=\"weui_cell weui_check_label\" for=\"car" + item.ID + "\" onclick=\"select_car('[" + item.Plate + "]" + item.BrandShow + "','" + item.Color + "','" + item.Plate + "')\">" +
                              "      <div class=\"weui_cell_ft\">" +
                              "          <input type=\"radio\" class=\"weui_check\" name=\"carditem\" id=\"car" + item.ID + "\" value=\"" + item.ID + "\">" +
                              "          <span class=\"weui_icon_checked\">&nbsp;</span>" +
                              "      </div>" +
                              "  </label>" +
                              "</div>" +
                              "</div>");
                }

                mm.code = "1";
                mm.msg = "success";
                mm.data = sb.ToString();
            }
            else
            {
                mm.msg = "没有车辆";
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
    }
}