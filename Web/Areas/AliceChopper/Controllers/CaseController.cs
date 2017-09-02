using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Web.Units;
using Web.Areas.Models;
using System.Data;
using Models;

namespace Web.Areas.AliceChopper.Controllers
{
    public class CaseController : AdminBaseController
    {
        public ActionResult List(string txtTitle, int typeid = 0, int page = 1)
        {
            Load();
             ListModel model = new  ListModel();
            StringBuilder strsql = new StringBuilder();
            if (!String.IsNullOrEmpty(txtTitle)) AliceDAL.Common.SqlString(strsql, String.Format("Title like '%{0}%'", AliceDAL.Common.cutBadStr(txtTitle)));
            if (typeid > 0) AliceDAL.Common.SqlString(strsql, String.Format("TypeID={0}", typeid.ToString()));
            ListModel.CreatePage(model, "[Case]", strsql, page, WorkContext.PageSize);
            return View(model);
        }
        private List<Supplier> Load()
        {
            List<Supplier> list = new List<Supplier>();
            DataTable dt = DAL.PageModel.DateList("Supplier", "IsShow=1", "SortID", 0);
            if (dt != null)
            {
                list = (from f in dt.AsEnumerable()
                        select new Supplier()
                        {
                            Title = f.Field<string>("Title"),
                            CityName = f.Field<string>("CityName"),
                            ProvinceName = f.Field<string>("ProvinceName"),
                            ID = f.Field<int>("ID")
                        }).ToList();
                ViewData["supplierListchild"] = list;
            }
            return list;
        }
        public ActionResult Delete(int[] cid)
        {
            DAL.PageModel.Delete("[Case]", "ID", cid);
            AddAdminLog("删除案例", "删除案例,案例ID为:" + AliceDAL.Common.Ints2String(cid));
            return PromptView("案例删除成功");
        }
        [ValidateInput(false)]
        public ActionResult Add(Case model)
        {
            List<Supplier> list = Load();
            if (AliceDAL.Common.IsGet())
            {
                model.Author = WorkContext.NickName;
                model.Source = "本站";
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "案例标题不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("TypeID", "请选择分销商");
                return View(model);
            }
            Case pc = new Case()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hot = 0,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID
            };
            foreach (var item in list)
            {
                if (pc.TypeID == item.ID)
                {
                    pc.ProvinceName = item.ProvinceName;
                    pc.CityName = item.CityName;
                    pc.CompanyName = item.Title;
                }
            }
            if (DAL.Case.Add(pc) > 0)
            {
                AddAdminLog("添加案例", "添加案例，案例为：" + pc.Title);
                return PromptView("案例添加成功");
            }
            else
            {
                ModelState.AddModelError("Title", "案例添加失败，请稍后再试");
                return View(model);
            }
        }

        [ValidateInput(false)]
        public ActionResult Edit(Case model, int cid = -1)
        {
            List<Supplier> list = Load();
            Case m = new Case();
            System.Data.DataTable dt = DAL.PageModel.Table_Model("[Case]", String.Format("ID={0}", cid.ToString()));
            if (dt == null) return PromptView("此文章不存在");
            m.Author = dt.Rows[0]["Author"].ToString();
            m.Body = dt.Rows[0]["Body"].ToString();
            m.Description = dt.Rows[0]["Description"].ToString();
            m.Img = dt.Rows[0]["Img"].ToString();
            m.KeyWords = dt.Rows[0]["KeyWords"].ToString();
            m.Source = dt.Rows[0]["Source"].ToString();
            m.Title = dt.Rows[0]["Title"].ToString();
            m.TitleWeb = dt.Rows[0]["TitleWeb"].ToString();
            m.TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString());
            m.ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString());
            if (AliceDAL.Common.IsGet()) return View(m);
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "案例标题不能为空");
                return View(model);
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError("TypeID", "请选择分销商");
                return View(model);
            }
            Case pc = new Case()
            {
                Author = model.Author ?? "",
                Body = model.Body ?? "",
                Description = model.Description ?? "",
                Hit = 0,
                Hot = 0,
                Img = model.Img ?? "/Content/images/noimage.jpg",
                KeyWords = model.KeyWords ?? "",
                Source = model.Source ?? "",
                Title = model.Title ?? "",
                TitleSpell = AliceDAL.HzToPz.QuanPin(model.Title ?? ""),
                TitleWeb = model.TitleWeb ?? "",
                TypeID = model.TypeID,
                ID = m.ID
            };
            foreach (var item in list)
            {
                if (pc.TypeID == item.ID)
                {
                    pc.ProvinceName = item.ProvinceName;
                    pc.CityName = item.CityName;
                    pc.CompanyName = item.Title;
                }
            }
            if (DAL.Case.Edit(pc) > 0)
            {
                AddAdminLog("修改案例", "修改案例，案例ID为：" + pc.ID);
                return PromptView("/AliceChopper/Case/List", "案例修改成功");
            }
            else
            {
                ModelState.AddModelError("Title", "案例修改失败，请稍后再试");
                return View(model);
            }
        }
    }
}
