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
    public class SupplierController : AdminBaseController
    {
        public ActionResult List(string txtTitle, string pcregion, string rdoState, int page = 1)
        {
             ListModel model = new  ListModel();
            StringBuilder strsql = new StringBuilder();
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (!String.IsNullOrEmpty(pcregion)) AliceDAL.Common.SqlString(strsql, String.Format("ProvinceName='{0}'", AliceDAL.Common.cutBadStr(pcregion.Split(',')[0])));
            if (!String.IsNullOrEmpty(pcregion) && pcregion.Contains(",")) AliceDAL.Common.SqlString(strsql, String.Format("CityName='{0}'", AliceDAL.Common.cutBadStr(pcregion.Split(',')[1])));
            if (!String.IsNullOrEmpty(rdoState)) AliceDAL.Common.SqlString(strsql, String.Format("IsShow={0}", AliceDAL.DataType.ConvertToInt(rdoState).ToString()));
            ListModel.CreatePage(model, "Supplier", strsql, page, WorkContext.PageSize, "IsShow asc,ID");
            return View(model);
        }

        public ActionResult Add(Supplier model)
        {
            if (AliceDAL.Common.IsGet()) return View(model);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.ProvinceName) || String.IsNullOrEmpty(model.CityName) || model.CityName.IndexOf(",") < 0)
            {
                ModelState.AddModelError("Region", "请选择所在区域");
                return View(model);
            }
            model.ProvinceName = model.CityName.Split(',')[0];
            model.CityName = model.CityName.Split(',')[1];
            model.Address = "";
            model.IsShow = 1;
            DAL.Supplier.Add(model);
            AliceDAL.CacheManager.Remove(AliceDAL.CacheKeys.SUPPLIERJSON);
            AddAdminLog("添加分销商", "添加分销商,名称为:" + model.Title);

            return PromptView("/AliceChopper/Supplier/List", "分销商添加成功");
        }
        public ActionResult Edit(Supplier model, int fid = -1)
        {
            System.Data.DataTable dt = DAL.PageModel.Table_Model("Supplier", String.Format("ID={0}", fid.ToString()));
            if (dt == null) return PromptView("此分销商不存在");
            var m = (from field in dt.AsEnumerable()
                     select new Supplier()
                     {
                         ActArea = field.Field<string>("ActArea"),
                         CityName = field.Field<string>("CityName"),
                         Address = field.Field<string>("Address"),
                         Phone = field.Field<string>("Phone"),
                         ProvinceName = field.Field<string>("ProvinceName"),
                         Title = field.Field<string>("Title"),
                         SortID = field.Field<int>("SortID"),
                         ID = field.Field<int>("ID"),
                         IsShow = field.Field<byte>("IsShow")
                     }).ToList().First();
            if (AliceDAL.Common.IsGet()) return View(m);

            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "名称不能为空");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.ProvinceName) || String.IsNullOrEmpty(model.CityName) || model.CityName.IndexOf(",") < 0)
            {
                ModelState.AddModelError("Region", "请选择所在区域");
                return View(model);
            }
            m.ProvinceName = model.CityName.Split(',')[0];
            m.CityName = model.CityName.Split(',')[1];
            m.Address = "";
            m.IsShow = model.IsShow;
            DAL.Supplier.Edit(m);
            AliceDAL.CacheManager.Remove(AliceDAL.CacheKeys.SUPPLIERJSON);
            AddAdminLog("修改分销商", "修改分销商,分销商为ID:" + m.ID);
            return PromptView("/AliceChopper/Supplier/List", "分销商修改成功");

        }

        public ActionResult Delete(int fid = -1)
        {
            AliceDAL.CacheManager.Remove(AliceDAL.CacheKeys.SUPPLIERJSON);
            DAL.PageModel.Delete("Supplier", String.Format("ID={0}", fid.ToString()));
            AddAdminLog("删除分销商", "删除分销商,分销商ID为:" + fid);
            return PromptView("分销商删除成功");
        }
    }
}
