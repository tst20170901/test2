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

namespace Web.Areas.UsersManger.Controllers
{
    public class UsersAdminController : BigUserBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 修改地址
        public ActionResult EditAddress()
        {
            if (AliceDAL.Common.IsGet())
            {
                ViewBag.Address = WorkContext.Data3;
                return View();
            }

            string address = AliceDAL.Common.GetFormString("address");

            if (AliceDAL.Common.cutBadStr(address) == "")
            {
                return PromptView("请填写地址！");
            }
            else
            {
                int result = DAL.BD_Users.EditAddress(WorkContext.ID, address);
                if (result > 0)
                {

                    AliceDAL.Common.SetCookie("UserLoginInfo", "Data3", address);
                    return PromptView("修改成功！");
                }
                else
                {
                    return PromptView("修改失败！");
                }
            }
        }
        #endregion
        #region 修改密码
        public ActionResult EditPassword()
        {
            if (AliceDAL.Common.IsGet()) return View();

            string password = AliceDAL.Common.GetFormString("password");
            string newpassword = AliceDAL.Common.GetFormString("newpassword");
            string repassword = AliceDAL.Common.GetFormString("repassword");

            if (AliceDAL.Common.cutBadStr(password) == "")
            {
                return PromptView("请填写旧密码！");
            }
            else if (AliceDAL.Common.cutBadStr(newpassword) == "")
            {
                return PromptView("请填写新密码！");

            }
            else if (AliceDAL.Common.cutBadStr(repassword) == "")
            {
                return PromptView("请确认新密码！");
            }
            else if (repassword != newpassword)
            {
                return PromptView("两次密码不一致，请重新填写！");
            }
            if (AliceDAL.SecureHelper.MD5(password) != WorkContext.UserPwd)
            {
                return PromptView("旧密码不正确！");
            }
            else
            {
                int result = DAL.BD_Users.EditPwd(WorkContext.LoginID, AliceDAL.SecureHelper.MD5(newpassword));
                if (result > 0)
                {

                    AliceDAL.Common.SetCookie("UserLoginInfo", "LoginID", WorkContext.LoginID);
                    AliceDAL.Common.SetCookie("UserLoginInfo", "UserPwd", AliceDAL.SecureHelper.MD5(newpassword));
                    return PromptView("修改成功，下次登录请使用新密码！");
                }
                else
                {
                    return PromptView("修改失败！");
                }
            }
        }
        #endregion
        public ActionResult WebInit()
        {
            return View();
        }
        public ViewResult CarList(string plate, string gp, int page = 1)
        {
            Load();
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[UserID]=" + WorkContext.ID.ToString());
            if (!String.IsNullOrWhiteSpace(plate)) AliceDAL.Common.SqlString(strsql, String.Format("[Plate] like '%{0}%'", AliceDAL.Common.cutBadStr(plate)));
            if (!String.IsNullOrWhiteSpace(gp)) AliceDAL.Common.SqlString(strsql, String.Format("[Data1] like '%{0}%'", AliceDAL.Common.cutBadStr(gp)));
            ListModel.CreatePage(model, "[UserCar]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        [HttpPost]
        public ViewResult CarList(HttpPostedFileBase uploadFile)
        {
            if (uploadFile != null)
            {
                if (uploadFile.ContentLength > 0)
                {
                    //获得保存路径
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/uploadfiles"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    string gp = String.IsNullOrWhiteSpace(AliceDAL.Common.GetFormString("cargroup")) ? "" : AliceDAL.Common.GetFormString("cargroup");
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
                                    if (!String.IsNullOrWhiteSpace(line) && line.Length == 7)
                                    {
                                        UserCar uc = new UserCar();
                                        uc.BrandID = -1;
                                        uc.BrandShow = "";
                                        uc.Color = "";
                                        uc.Data1 = gp;
                                        uc.Data2 = uc.Data3 = uc.Data4 = uc.Data5 = "";
                                        uc.ModelID = -1;
                                        uc.Plate = line.ToString();
                                        uc.UserID = WorkContext.ID;
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
            Load();
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[UserID]=" + WorkContext.ID.ToString());
            ListModel.CreatePage(model, "[UserCar]", strsql, 1, WorkContext.PageSize, "ID");
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult UserCarAdd(Models.BD_UserCarModel model)
        {
            Load();
            if (AliceDAL.Common.IsGet()) return View(model);

            if (String.IsNullOrWhiteSpace(model.Plate))
            {
                ModelState.AddModelError("errormsg", "车牌号不能为空");
                return View(model);
            }
            else if (model.Plate.Length != 7)
            {
                ModelState.AddModelError("errormsg", "车牌号不正确");
                return View(model);
            }

            UserCar uc = new UserCar()
            {
                BrandID = model.BrandID,
                BrandShow = model.Brand + " " + model.ModelStr,
                Color = model.Color ?? "",
                Data1 = model.CarGroup ?? "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = model.ModelID,
                Plate = model.Plate,
                UserID = WorkContext.ID,
                UserName = String.IsNullOrWhiteSpace(model.UserName) ? WorkContext.Data1 : model.UserName
            };
            string result = DAL.UserCar.Add(uc);
            if (result == "exists")
            {
                ModelState.AddModelError("errormsg", "车牌号已存在");
                return View(model);
            }
            else
            {
                return PromptView("添加成功");
            }
        }
        [ValidateInput(false)]
        public ActionResult UserCarEdit(Models.BD_UserCarModel model, int cid = -1, int page = 1)
        {
            Load();
            UserCar uc1 = DAL.UserCar.GetModel(cid);
            if (uc1 == null || uc1.UserID != WorkContext.ID)
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
                model.CarGroup = uc1.Data1;
                return View(model);
            }
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
                Data1 = model.CarGroup ?? "",
                Data2 = "",
                Data3 = "",
                Data4 = "",
                Data5 = "",
                ModelID = model.ModelID,
                Plate = model.Plate,
                UserID = WorkContext.ID,
                UserName = String.IsNullOrWhiteSpace(model.UserName) ? WorkContext.Data1 : model.UserName,
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
                return PromptView("/UsersManger/UsersAdmin/CarList?page=" + page.ToString(), "修改成功");
            }
        }
        public ActionResult DeleteUserCar(int cid)
        {
            UserCar uc = DAL.UserCar.GetModel(cid);
            if (uc == null || uc.UserID != WorkContext.ID)
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
                return PromptView("删除成功");
            }
        }
        public ActionResult CarTypeList(CarGroup model, int cid = -1, string act = "list")
        {
            switch (act)
            {
                case "list":
                    ViewData["list"] = DAL.PageModel.DateList("CarGroup", "UserID=" + WorkContext.ID, "SortID", 0);
                    if (AliceDAL.Common.IsGet()) return View(model);
                    if (String.IsNullOrEmpty(model.TypeName))
                    {
                        ModelState.AddModelError("TypeName", "分组名称不能为空");
                        return View(model);
                    }
                    model.UserID = WorkContext.ID;
                    model.ParentID = 1;//1为车辆分组
                    string result = DAL.CarGroup.Add(model);
                    if ("1" == result)
                    {
                        ModelState.AddModelError("Msg", "添加成功");
                        ViewData["list"] = DAL.PageModel.DateList("CarGroup", "UserID=" + WorkContext.ID, "SortID", 0);
                        return View();
                    }
                    else if ("-2" == result)
                    {
                        ModelState.AddModelError("Msg", "分组名称已经存在");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "添加失败，请稍候再试");
                        return View(model);
                    }
                case "edit":
                    ViewData["list"] = DAL.PageModel.DateList("CarGroup", "UserID=" + WorkContext.ID, "SortID", 0);
                    DataTable dt = DAL.PageModel.Table_Model("CarGroup", "ID=" + cid.ToString());
                    if (dt == null || dt.Rows[0]["UserID"].ToString() != WorkContext.ID.ToString()) return PromptView("此分组不存在");
                    CarGroup m = new CarGroup()
                    {
                        ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString()),
                        ParentID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ParentID"].ToString()),
                        SortID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["SortID"].ToString()),
                        TypeName = dt.Rows[0]["TypeName"].ToString()
                    };
                    if (AliceDAL.Common.IsGet()) return View(m);

                    model.ID = m.ID;
                    model.ParentID = m.ParentID;
                    string res = DAL.CarGroup.Edit(model);
                    if ("1" == res)
                    {
                        return PromptView("修改成功！");
                    }
                    else if ("-2" == res)
                    {
                        ModelState.AddModelError("Msg", "分组名称已经存在");
                        return View(m);
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "修改失败，请稍候再试");
                        return View(m);
                    }
                case "del":
                    if (DAL.CarGroup.Delete(cid))
                    {
                        return PromptView("删除成功！");
                    }
                    else
                    {
                        ModelState.AddModelError("Msg", "删除失败，请稍候再试");
                        return View();
                    }
                default:
                    return View();
            }
        }
        private void Load()
        {
            ViewData["cartypeList"] = DAL.PageModel.DateList("CarGroup", "UserID=" + WorkContext.ID, "SortID", 0);
        }
    }
}
