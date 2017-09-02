using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Units;
using System.Text;
using Web.Areas.Models;
using System.Data;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class WorkShopController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int BranchID = -1, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[TypeID]=1");
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("[Title] like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_WorkShop]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult ShopList(string txtTitle, int BranchID = -1, int page = 1)
        {
            ListModel model = new ListModel();
            StringBuilder strsql = new StringBuilder("[TypeID]=2");
            if (WorkContext.RoleID == 1)
            {
                LoadBranch();
                if (BranchID > 0)
                {
                    AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", BranchID));
                }
            }
            else AliceDAL.Common.SqlString(strsql, String.Format("[BranchID]={0}", WorkContext.BranchID));
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("[Title] like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            ListModel.CreatePage(model, "[uv_WorkShop]", strsql, page, WorkContext.PageSize, "ID");
            return View(model);
        }
        public ActionResult Add(WorkShop model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "姓名不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            int s = AliceDAL.Common.GetFormInt("StartTime");
            int e = AliceDAL.Common.GetFormInt("EndTime");
            if (s > e)
            {
                ModelState.AddModelError("Title", "营业时间设置错误");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.Lng, model.Lat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "选取坐标有误");
                return View(model);
            }
            WorkShop ws = new WorkShop();
            ws.Adcode = am.AdCode;
            ws.City = am.City;
            ws.CityCode = am.CityCode;
            ws.BranchID = model.BranchID;
            ws.TypeID = 1;
            ws.CostTime = model.CostTime <= 0 ? 20 : model.CostTime;
            ws.District = am.District;
            ws.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e, 0, 0);
            ws.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, s, 0, 0);
            ws.Lat = ws.Lat1 = am.Lat;
            ws.Lng = ws.Lng1 = am.Lng;
            ws.Province = am.Province;
            ws.Title = model.Title;
            ws.Mobile = model.Mobile;
            ws.UserPwd = AliceDAL.SecureHelper.MD5(model.UserPwd);
            ws.WorkState = 10;
            ws.WorkRadius = model.WorkRadius;
            ws.Phone = model.Phone;
            string result = DAL.WorkShop.Add(ws);
            if (result == "exists mobile")
            {
                ModelState.AddModelError("Title", "帐号已存在");
                return View(model);
            }
            else if (result == "error")
            {
                ModelState.AddModelError("Title", "系统错误");
                return View(model);
            }
            else
            {
                AddAdminLog("添加洗车工", "添加洗车工,洗车工姓名为:" + ws.Title + ";帐号为:" + ws.Mobile + ";电话为:" + ws.Phone + ";服务半径为:" + ws.WorkRadius);
                return PromptView(Url.Action("List"), "洗车工添加成功");
            }
        }
        public ActionResult ShopAdd(WorkShop model)
        {
            LoadBranch();
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            int s = AliceDAL.Common.GetFormInt("StartTime");
            int e = AliceDAL.Common.GetFormInt("EndTime");
            if (s > e)
            {
                ModelState.AddModelError("Title", "营业时间设置错误");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.Lng, model.Lat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "选取坐标有误");
                return View(model);
            }
            WorkShop ws = new WorkShop();
            ws.Adcode = am.AdCode;
            ws.City = am.City;
            ws.CityCode = am.CityCode;
            ws.BranchID = model.BranchID;
            ws.TypeID = 2;
            ws.CostTime = model.CostTime <= 0 ? 20 : model.CostTime;
            ws.District = String.IsNullOrEmpty(model.District) ? am.FormatAddress : model.District;
            ws.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e, 0, 0);
            ws.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, s, 0, 0);
            ws.Lat = ws.Lat1 = model.Lat;
            ws.Lng = ws.Lng1 = model.Lng;
            ws.Province = am.Province;
            ws.Title = model.Title;
            ws.Phone = model.Phone;
            ws.Mobile = model.Mobile;
            ws.UserPwd = AliceDAL.SecureHelper.MD5(model.UserPwd);
            ws.WorkState = 10;
            ws.WorkRadius = 0;
            ws.Img = model.Img;
            string result = DAL.WorkShop.Add(ws);
            if (result == "exists mobile")
            {
                ModelState.AddModelError("Title", "名称已存在");
                return View(model);
            }
            else if (result == "error")
            {
                ModelState.AddModelError("Title", "系统错误");
                return View(model);
            }
            else
            {
                AddAdminLog("添加洗车店", "添加洗车店,洗车店名称为:" + ws.Title + ";帐号为:" + ws.Mobile + ";电话为:" + ws.Phone);
                return PromptView(Url.Action("ShopList"), "洗车店添加成功");
            }
        }
        public ActionResult Edit(WorkShop model, int fid = -1)
        {
            LoadBranch();
            WorkShop ws = DAL.WorkShop.WorkShopByID(fid);
            if (ws == null) return PromptView("此洗车工不存在");
            if (AliceDAL.Common.IsGet()) { ws.UserPwd = ""; return View(ws); }

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            int s = AliceDAL.Common.GetFormInt("StartTime");
            int e = AliceDAL.Common.GetFormInt("EndTime");
            if (s > e)
            {
                ModelState.AddModelError("Title", "营业时间设置错误");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.Lng, model.Lat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "选取坐标有误");
                return View(model);
            }
            WorkShop ws1 = new WorkShop();
            ws1.Adcode = am.AdCode;
            ws1.City = am.City;
            ws1.BranchID = model.BranchID;
            ws1.TypeID = 1;
            ws1.CityCode = am.CityCode;
            ws1.CostTime = model.CostTime <= 0 ? 20 : model.CostTime;
            ws1.District = am.District;
            ws1.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e, 0, 0);
            ws1.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, s, 0, 0);
            ws1.Lat = ws1.Lat1 = model.Lat;
            ws1.Lng = ws1.Lng1 = model.Lng;
            ws1.Province = am.Province;
            ws1.Title = model.Title;
            ws1.Mobile = model.Mobile;
            ws1.UserPwd = String.IsNullOrEmpty(model.UserPwd) ? ws.UserPwd : AliceDAL.SecureHelper.MD5(model.UserPwd);
            ws1.WorkState = model.WorkState;
            ws1.WorkRadius = model.WorkRadius;
            ws1.Phone = model.Phone;
            ws1.ID = ws.ID;
            DAL.WorkShop.Edit(ws1);
            AddAdminLog("修改洗车工", "修改洗车工,洗车工原姓名为:" + ws.Title + ";原帐号为:" + ws.Mobile + ";原电话为:" + ws.Phone + ";原服务半径为:" + ws.WorkRadius +
                        "洗车工现姓名为:" + ws1.Title + ";现帐号为:" + ws1.Mobile + ";现电话为:" + ws1.Phone + ";现服务半径为:" + ws1.WorkRadius);
            return PromptView(Url.Action("List"), "洗车工修改成功");

        }
        public ActionResult ShopEdit(WorkShop model, int fid = -1)
        {
            LoadBranch();
            WorkShop ws = DAL.WorkShop.WorkShopByID(fid);
            if (ws == null) return PromptView("此洗车店不存在");
            if (AliceDAL.Common.IsGet()) { ws.UserPwd = ""; return View(ws); }

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            else if (model.BranchID <= 0)
            {
                ModelState.AddModelError("Title", "请选择分公司");
                return View(model);
            }
            int s = AliceDAL.Common.GetFormInt("StartTime");
            int e = AliceDAL.Common.GetFormInt("EndTime");
            if (s > e)
            {
                ModelState.AddModelError("Title", "营业时间设置错误");
                return View(model);
            }
            AddressModel am = AMap.Common.GetAddress(model.Lng, model.Lat);
            if (am == null || String.IsNullOrEmpty(am.Province))
            {
                ModelState.AddModelError("Title", "选取坐标有误");
                return View(model);
            }
            WorkShop ws1 = new WorkShop();
            ws1.Adcode = am.AdCode;
            ws1.City = am.City;
            ws1.BranchID = model.BranchID;
            ws1.TypeID = 1;
            ws1.CityCode = am.CityCode;
            ws1.CostTime = model.CostTime <= 0 ? 20 : model.CostTime;
            ws1.District = String.IsNullOrEmpty(model.District) ? am.FormatAddress : model.District;
            ws1.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e, 0, 0);
            ws1.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, s, 0, 0);
            ws1.Lat = ws1.Lat1 = am.Lat;
            ws1.Lng = ws1.Lng1 = am.Lng;
            ws1.Phone = model.Phone;
            ws1.Province = am.Province;
            ws1.Title = model.Title;
            ws1.Mobile = model.Mobile;
            ws1.UserPwd = String.IsNullOrEmpty(model.UserPwd) ? ws.UserPwd : AliceDAL.SecureHelper.MD5(model.UserPwd);
            ws1.WorkState = model.WorkState;
            ws1.WorkRadius = model.WorkRadius;
            ws1.ID = ws.ID;
            ws1.Img = model.Img;
            DAL.WorkShop.Edit(ws1);
            AddAdminLog("修改洗车店", "修改洗车店,洗车店原名称为:" + ws.Title + ";原帐号为:" + ws.Mobile + ";原电话为:" + ws.Phone +
                        "现名称为:" + ws1.Title + ";现帐号为:" + ws1.Mobile + ";现电话为:" + ws1.Phone);
            return PromptView(Url.Action("ShopList"), "洗车店修改成功");
        }
        public ActionResult Delete(int fid = -1)
        {
            DAL.PageModel.Delete("[WorkShop]", String.Format("ID={0}", fid.ToString()));
            AddAdminLog("删除服务点", "删除服务点,服务点ID为:" + fid);
            return PromptView("服务点删除成功");
        }
        public ActionResult ReSet(int fid = -1)
        {
            WorkShop ws = DAL.WorkShop.WorkShopByID(fid);
            if (ws == null) return PromptView("此洗车工不存在");
            DAL.WorkShop.ReSet(fid);
            AddAdminLog("复位洗车工", "复位洗车工,洗车工位置经度为:" + ws.Lng1 + ";纬度为:" + ws.Lat1);
            return PromptView(Url.Action("List"), "洗车工复位成功");
        }
    }
}