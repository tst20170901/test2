using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Web.Units;
using Alice.WebControls.Mvc;
using Web.Areas.Models;
using Models;
using System.IO;
using Newtonsoft.Json;

namespace Web.Areas.AliceChopper.Controllers
{
    public class MemberController : AdminBaseController
    {
        public ActionResult List(string mobile, string OrderCount, string btnSearch, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("Data2<>'2'");
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(mobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(mobile)));
                if (AliceDAL.DataType.ConvertToInt(OrderCount) > 0) AliceDAL.Common.SqlString(strsql, String.Format("OrderCount>={0}", AliceDAL.Common.cutBadStr(OrderCount)));
                ListModel.CreatePage(model, "[uv_BD_Users_Wallet]", strsql, page, WorkContext.PageSize, "Money1");
            }
            else
            {
                if (!String.IsNullOrEmpty(mobile))
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("Mobile ='{0}'", AliceDAL.Common.cutBadStr(mobile)));
                    if (AliceDAL.DataType.ConvertToInt(OrderCount) > 0) AliceDAL.Common.SqlString(strsql, String.Format("OrderCount>={0}", AliceDAL.Common.cutBadStr(OrderCount)));
                    ListModel.CreatePage(model, "[uv_BD_Users_Wallet]", strsql, page, WorkContext.PageSize, "Money1");
                }
            }

            switch (btnSearch)
            {
                case "导出表格":
                    string strHeader = "ID,手机号,姓名,钱包余额,赠送余额,订单数量,注册日期";
                    string strCol = "ID,LoginID,Data1,Money1,Money2,OrderCount,CDate";

                    DataTable dtt = new DataTable();
                    DataTable dt = DAL.PageModel.DateList("[uv_BD_Users_Wallet]", strsql.ToString(), "CDate desc,ID", 0);
                    string[] HeaderArr = strHeader.Split(new char[] { ',' });
                    string[] ColArr = strCol.Split(new char[] { ',' });

                    foreach (string col in HeaderArr)
                    {
                        dtt.Columns.Add(col);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dgvr in dt.Rows)
                        {
                            DataRow dr = dtt.NewRow();
                            for (int i = 0; i < HeaderArr.Length; i++)
                            {
                                dr[HeaderArr[i]] = dgvr[ColArr[i]];
                            }
                            dtt.Rows.Add(dr);
                        }
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("会员卡使用记录" + DateTime.Now.ToString("yyyyMMdd")) + ".xls");
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(ExcelRender.RenderToExcel(dtt).GetBuffer());
                    }
                    break;
                default:
                    break;
            }
            return View(model);
        }
        public ActionResult ChangeUserCheck(int uid = -1)
        {
            BD_Users bb = DAL.BD_Users.GetUserInfoByID(uid);
            if (bb == null) return PromptView("此用户不存在");
            DAL.BD_Users.ChangeUserIsCheck(uid, bb.IsCheck <= 0 ? 10 : 0);
            AddAdminLog("用户变更服务", (bb.IsCheck <= 0 ? "禁用" : "启用") + "挪车服务,用户ID为:" + uid + ";LoginID为:" + bb.LoginID);
            return RedirectToAction("List");
        }
        public ActionResult BigList(string mobile, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("Data2='2' and [BranchID]=" + WorkContext.BranchID);
            if (WorkContext.RoleID == 1)
            {
                if (!String.IsNullOrEmpty(mobile)) AliceDAL.Common.SqlString(strsql, String.Format("Mobile like '%{0}%'", AliceDAL.Common.cutBadStr(mobile)));
                ListModel.CreatePage(model, "[uv_BD_Users_Wallet]", strsql, page, WorkContext.PageSize, "Money1");
            }
            else
            {
                if (!String.IsNullOrEmpty(mobile))
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("Mobile ='{0}'", AliceDAL.Common.cutBadStr(mobile)));
                    ListModel.CreatePage(model, "[uv_BD_Users_Wallet]", strsql, page, WorkContext.PageSize, "Money1");
                }
            }
            return View(model);
        }
        //public ViewResult Delete(int[] uid)
        //{
        //    DAL.PageModel.Delete("BD_User", "ID", uid);
        //    AddAdminLog("删除会员", "删除会员,会员ID为:" + AliceDAL.Common.Ints2String(uid));
        //    return PromptView("会员删除成功");
        //}

        public ViewResult EditPwd(int uid)
        {
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(uid);
            if (bu == null) return PromptView("此会员不存在");
            DAL.BD_Users.EditPwd(bu.LoginID, AliceDAL.SecureHelper.MD5("123456"));
            AddAdminLog("重置会员密码", "重置会员密码,会员ID为:" + uid + ";手机号为:" + bu.LoginID);
            return PromptView("重置会员密码成功，密码为123456");
        }
        public ViewResult EditUserCar(string plate, int uid = -1, int p = 1, int page = 1)
        {
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(uid);
            if (bu == null) return PromptView("/AliceChopper/Member/List", "此会员不存在");
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[UserID]=" + uid.ToString());
            if (!String.IsNullOrEmpty(plate)) AliceDAL.Common.SqlString(strsql, String.Format("[Plate] like '%{0}%'", AliceDAL.Common.cutBadStr(plate)));
            ListModel.CreatePage(model, "[UserCar]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        [HttpPost]
        public ViewResult EditUserCar(HttpPostedFileBase uploadFile, int uid = -1)
        {
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(uid);
            if (bu == null) return PromptView("/AliceChopper/Member/List", "此会员不存在");
            if (uploadFile != null)
            {
                if (uploadFile.ContentLength > 0)
                {
                    //获得保存路径
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/uploadfiles"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);

                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            List<string> list = new List<string>();

                            String line;
                            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (!String.IsNullOrWhiteSpace(line) && AliceDAL.ValidateHelper.IsCarNo(line))
                                    {
                                        UserCar uc = new UserCar();
                                        uc.BrandID = -1;
                                        uc.BrandShow = "";
                                        uc.Color = "";
                                        uc.Data1 = uc.Data2 = uc.Data3 = uc.Data4 = uc.Data5 = "";
                                        uc.ModelID = -1;
                                        uc.Plate = line.ToString();
                                        uc.UserID = uid;
                                        uc.UserName = uc.UserName ?? "";
                                        DAL.UserCar.Add(uc);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return PromptView("导入失败！" + ex.Message);
                    }
                }
                else return PromptView("请选择正确导入源");
            }
            else return PromptView("请选择正确导入源");

            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[UserID]=" + uid.ToString());
            ListModel.CreatePage(model, "[UserCar]", strsql, 1, WorkContext.PageSize, "ID");
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult UserCarAdd(Models.BD_UserCarModel model, int uid)
        {
            if (AliceDAL.Common.IsGet()) return View(model);
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(uid);
            if (bu == null) return PromptView("此会员不存在");
            if (String.IsNullOrWhiteSpace(model.Plate))
            {
                ModelState.AddModelError("errormsg", "车牌号不能为空");
                return View(model);
            }
            else if (!AliceDAL.ValidateHelper.IsCarNo(model.Plate))
            {
                ModelState.AddModelError("errormsg", "车牌号不正确");
                return View(model);
            }

            UserCar uc = new UserCar()
            {
                BrandID = model.BrandID,
                BrandShow = model.Brand + " " + model.ModelStr,
                Color = model.Color ?? "",
                Data1 = "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = model.ModelID,
                Plate = model.Plate,
                UserID = uid,
                UserName = String.IsNullOrWhiteSpace(model.UserName) ? bu.Data1 : model.UserName
            };
            string result = DAL.UserCar.Add(uc);
            if (result == "exists")
            {
                ModelState.AddModelError("errormsg", "车牌号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("添加车辆", String.Format("添加车辆,会员ID为:{0},车牌号为:{1}", uid, model.Plate));
                return PromptView("添加成功");
            }
        }
        [ValidateInput(false)]
        public ActionResult UserCarEdit(Models.BD_UserCarModel model, int cid = -1, int userid = -1, int page = 1)
        {
            UserCar uc1 = DAL.UserCar.GetModel(cid);
            if (uc1 == null || uc1.UserID != userid)
            {
                return PromptView("此车辆不存在");
            }

            if (AliceDAL.Common.IsGet())
            {
                model.BrandID = uc1.BrandID;
                model.Color = uc1.Color;
                model.ModelID = uc1.ModelID;
                model.Plate = uc1.Plate;
                model.UserName = uc1.UserName;
                return View(model);
            }
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(userid);
            if (bu == null) return PromptView("此会员不存在");
            if (String.IsNullOrWhiteSpace(model.Plate))
            {
                ModelState.AddModelError("errormsg", "车牌号不能为空");
                return View(model);
            }
            else if (!AliceDAL.ValidateHelper.IsCarNo(model.Plate))
            {
                ModelState.AddModelError("errormsg", "车牌号不正确");
                return View(model);
            }

            UserCar uc = new UserCar()
            {
                BrandID = model.BrandID,
                BrandShow = model.Brand + " " + model.ModelStr,
                Color = model.Color ?? "",
                Data1 = "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = model.ModelID,
                Plate = model.Plate,
                UserID = userid,
                UserName = String.IsNullOrWhiteSpace(model.UserName) ? bu.Data1 : model.UserName,
                ID = cid
            };
            string result = DAL.UserCar.Edit(uc);
            if (result == "exists")
            {
                ModelState.AddModelError("errormsg", "车牌号已存在");
                return View(model);
            }
            else
            {
                AddAdminLog("修改车辆", String.Format("修改车辆,会员ID为:{0},原车牌号为:{1};现车牌号为:{2}", userid, uc1.Plate, uc.Plate));
                return PromptView(Url.Action("EditUserCar", new { uid = userid, page = page.ToString() }), "修改成功");
            }
        }
        public ActionResult DeleteUserCar(int cid, int userid)
        {
            UserCar uc = DAL.UserCar.GetModel(cid);
            if (uc == null || uc.UserID != userid)
            {
                return PromptView("此车辆不存在");
            }
            int result = DAL.PageModel.Delete("UserCar", "ID=" + cid.ToString());
            if (result <= 0)
            {
                return PromptView("删除失败，请稍候再试");
            }
            else
            {
                AddAdminLog("删除车辆", String.Format("删除车辆,会员ID为:{0},车牌号为:{1}", userid, uc.Plate));
                return PromptView("删除成功");
            }
        }
        public ViewResult EditMoney(Models.BD_Users_Wallet model, int uid = -1, int page = 1)
        {
            BD_Users bu = DAL.BD_Users.GetUserInfoByID(uid);
            if (bu == null) return PromptView("此会员不存在");
            BD_Wallet bw = DAL.BD_Users.GetUserWalletByUserID(uid);
            if (bw == null) return PromptView("会员钱包初始化失败");
            if (AliceDAL.Common.IsGet())
            {
                model.ID = bu.ID;
                model.LoginID = bu.LoginID;
                model.Money1 = bw.Money1;
                model.Money2 = bw.Money2;
                model.Remark = "";
                return View(model);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(model.Remark))
                {
                    ModelState.AddModelError("Remark", "修改原因必须填写");
                    return View(model);
                }
                if (DAL.BD_Users.SetUserWalletByUserID(uid, model.Money1, model.Money2) > 0)
                {
                    AddAdminLog("余额变更", String.Format("余额变更,会员ID为:{0},会员帐号{1},原余额{2},原赠送余额{3},当前余额{4},当前赠送余额{5},变更原因:{6}", bu.ID, bu.LoginID, bw.Money1, bw.Money2, model.Money1, model.Money2, model.Remark));
                    if (bu.Data2 == "2")
                    {
                        return PromptView("/AliceChopper/Member/BigList?page=" + page.ToString(), "余额变更成功");
                    }
                    else
                    {
                        return PromptView("/AliceChopper/Member/List?page=" + page.ToString(), "余额变更成功");
                    }

                }
                else
                {
                    return PromptView("余额变更失败");
                }
            }
        }
        //public ActionResult Edit(BD_UserModel model, int uid = -1, int page = 1)
        //{
        //    Model.BD_User bu = DAL.BD_User.GetUserInfoByID(uid);
        //    if (bu == null) return PromptView("会员不存在");
        //    else
        //    {
        //        BD_UserModel bum = new BD_UserModel();
        //        bum.CompanyName = bu.CompanyName;
        //        bum.LoginID = bu.LoginID;
        //        bum.Mobile = bu.Mobile;
        //        bum.UserDate = bu.UserDate;
        //        bum.UserName = bu.UserName;
        //        bum.UserState = bu.UserState;
        //        if (AliceDAL.Common.IsGet())
        //        {
        //            return View(bum);
        //        }
        //        else
        //        {
        //            bu.CompanyName = model.CompanyName;
        //            bu.Password = !String.IsNullOrWhiteSpace(model.Password) ? AliceDAL.SecureHelper.MD5(model.Password) : bu.Password;
        //            bu.UserDate = model.UserDate;
        //            bu.UserName = model.UserName;
        //            bu.UserState = model.UserState;

        //            if (new DAL.BD_User().Edit(bu))
        //            {
        //                AddAdminLog("修改会员", String.Format("修改会员,会员ID为:{0},原姓名{1},原公司名{2},原过期时间{3},原状态{4}", bu.ID, bum.UserName, bum.CompanyName, bum.UserDate, bum.UserState));
        //                return PromptView("/IDoDo/Member/List?page=" + page.ToString(), "会员修改成功");
        //            }
        //            else
        //            {
        //                return PromptView("会员修改失败");
        //            }

        //        }
        //    }
        //}
        public ActionResult Add(Models.BD_UserModel model)
        {
            if (AliceDAL.Common.IsGet()) return View(model);
            BD_Users user = new BD_Users();
            user.Data1 = model.UserName;
            user.Data2 = "1";
            user.Data3 = user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";
            user.LoginID = model.LoginID;
            user.Mobile = model.LoginID;
            user.UserPwd = AliceDAL.SecureHelper.MD5(model.Password);
            string result = DAL.BD_Users.Add(user);
            if (result == "exists loginid" || result == "exists mobile")
            {
                ModelState.AddModelError("LoginID", "手机号码已经存在");
            }
            else
            {
                AddAdminLog("添加会员", String.Format("添加会员,会员手机号为:{0}", user.LoginID));
                return PromptView("会员添加成功");
            }
            return View(model);
        }
        public ActionResult BigAdd(Models.BD_BigUserModel model)
        {
            if (AliceDAL.Common.IsGet()) return View(model);
            BD_Users user = new BD_Users();
            user.Data1 = model.UserName;
            user.Data2 = "2";
            user.Data3 = model.Address;//常用地址
            user.Data4 = user.Data5 = user.Data6 = user.Data7 = user.Data8 = user.Data9 = user.Data10 = "";
            user.LoginID = model.LoginID;
            user.Mobile = model.LoginID;
            user.UserPwd = AliceDAL.SecureHelper.MD5(model.Password);
            user.BranchID = WorkContext.BranchID;

            string result = DAL.BD_Users.Add(user);
            if (result == "exists loginid" || result == "exists mobile")
            {
                ModelState.AddModelError("LoginID", "手机号码已经存在");
            }
            else
            {
                DAL.BD_Users.SetUserWalletByUserID(int.Parse(result), model.Money, 0);
                AddAdminLog("添加大客户", String.Format("添加大客户,客户手机号为:{0},客户名称为:{1},初始金额为:{2}", user.LoginID, user.Data1, model.Money));
                return PromptView("大客户添加成功");
            }
            return View(model);
        }
        public ActionResult PreAdd()
        {
            if (AliceDAL.Common.IsGet()) return View();
            Models.MessageModel mm = new Models.MessageModel();
            string plate = AliceDAL.Common.GetFormString("carplate");
            string brand = AliceDAL.Common.GetFormString("alicebrand");
            string model = AliceDAL.Common.GetFormString("alicemodel");
            string color = AliceDAL.Common.GetFormString("alicecolor");
            int brandid = AliceDAL.Common.GetFormInt("alicebrandid");
            int modelid = AliceDAL.Common.GetFormInt("alicemodelid");
            string mobile = AliceDAL.Common.GetFormString("mobile");
            string name = AliceDAL.Common.GetFormString("name");

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

            string result = DAL.BD_Users.PreAdd(mobile, name, plate, brand == "品牌" ? "" : brand, model == "车型" ? "" : model, brandid.ToString(), modelid.ToString(), color);
            if (result == "exists loginid" || result == "exists mobile")
            {
                mm.code = "0";
                mm.msg = "用户已存在";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
            else
            {
                AddAdminLog("添加预备会员", String.Format("添加预备会员,会员手机号为:{0},姓名为:{1},车牌号为:{2},车品牌为:{3},车型为:{4},车颜色为:{5}", result, mobile, name, plate, brand, model, color));
                mm.code = "0";
                mm.msg = "添加成功";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
        }
        public ActionResult AjaxUserInfo()
        {
            Models.MessageModel mm = new Models.MessageModel();
            string mobile = AliceDAL.Common.GetFormString("mobile");
            if (!AliceDAL.ValidateHelper.IsMobile(mobile))
            {
                mm.code = "0";
                mm.msg = "手机号码格式不正确！";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
            BD_Users bu = DAL.BD_Users.GetUserInfoByMobile(mobile);
            if (bu == null)
            {
                mm.code = "0";
                mm.msg = "<span style='color:#047548'>可添加</span>";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
            else
            {
                mm.code = "0";
                mm.msg = "已存在";
                string res = JsonConvert.SerializeObject(mm);
                return Content(res);
            }
        }
    }
}