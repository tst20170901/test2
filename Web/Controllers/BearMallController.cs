using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Web.Units;
using AliceDAL;
using Models;
using System.Text;
using Newtonsoft.Json;
using Web.Models;
using System.Collections;
namespace Web.Controllers
{
    public class BearMallController : Controller
    {
        public ActionResult WeiXin()
        {
            if (AliceDAL.Common.IsGet())
            {
                return Content("");
            }
            JSSDK jssdk = new JSSDK(AliceDAL.IniHelper.GetValue("WeiXinInfo", "AppId"), AliceDAL.IniHelper.GetValue("WeiXinInfo", "AppSecret"));
            Hashtable ht = jssdk.getSignPackage(AliceDAL.Common.GetFormString("url"));
            Models.WeiXinModel m = new Models.WeiXinModel();
            m.appId = ht["appId"].ToString();
            m.timestamp = ht["timestamp"].ToString();
            m.nonceStr = ht["nonceStr"].ToString();
            m.signature = ht["signature"].ToString();
            MessageModel mm = new MessageModel();
            mm.data = m;
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult Index()
        {
            SeoInit.InitHead("", "", "", this);
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult BusinessProductList(int bid)
        {
            Load(bid);
            Web.Models.ListModel listmodel = new Models.ListModel();
            int typeid = AliceDAL.UrlParam.GetIntValue("typeID");
            StringBuilder sb = new StringBuilder("[ProState]=10 and [BusinessID]=" + bid);
            if (typeid > 0)
            {
                AliceDAL.Common.SqlString(sb, String.Format("TypeID={0}", typeid));
            }
            BD_Business bb = DAL.BD_Business.GetModel(bid);
            ViewBag.BusinessName = bb.BusinessName;
            Web.Models.ListModel.CreatePage(listmodel, "[Pro_Case]", sb, 1, 10);
            return View(listmodel);
        }
        public ActionResult HotNews(int id)
        {
            DataTable dt = DAL.PageModel.Table_Model("Art_Common", String.Format("ID={0}", id.ToString()));
            if (dt != null)
            {
                Art_Common model = new Art_Common()
                {
                    Author = dt.Rows[0]["Author"].ToString(),
                    Body = dt.Rows[0]["Body"].ToString(),
                    CDate = AliceDAL.DataType.ConvertToDateTime(dt.Rows[0]["CDate"].ToString()),
                    Description = dt.Rows[0]["Description"].ToString(),
                    Hit = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hit"].ToString()),
                    Hot = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["Hot"].ToString()),
                    KeyWords = dt.Rows[0]["KeyWords"].ToString(),
                    Img = dt.Rows[0]["Img"].ToString(),
                    Source = dt.Rows[0]["Source"].ToString(),
                    Title = dt.Rows[0]["Title"].ToString(),
                    TitleSpell = dt.Rows[0]["TitleSpell"].ToString(),
                    TitleWeb = dt.Rows[0]["TitleWeb"].ToString(),
                    TypeID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["TypeID"].ToString()),
                    ID = AliceDAL.DataType.ConvertToInt(dt.Rows[0]["ID"].ToString())
                };
                Art_Common_Types act = DAL.Art_Common_Types.GetCateIdByID(model.TypeID);
                ViewBag.ID = act.ID;
                ViewBag.TypeName = act.TypeName;
                SeoInit.InitHead(String.IsNullOrWhiteSpace(model.TitleWeb) ? model.Title : model.TitleWeb, model.KeyWords, model.Description, this);
                DAL.PageModel.Hit("Art_Common", "Hit=Hit+1", "ID=" + model.ID.ToString());
                return View(model);
            }
            else return View("404");
        }
        public ActionResult AjaxBusinessProductList()
        {
            int typeid = AliceDAL.Common.GetFormInt("typeid");
            int bid = AliceDAL.Common.GetFormInt("bid");
            int page = AliceDAL.Common.GetFormInt("page");
            Web.Models.MessageModel mm = new Models.MessageModel();
            mm.msg = "没有了";
            mm.code = "0";
            Web.Models.ListModel listmodel = new Models.ListModel();
            StringBuilder sb = new StringBuilder("[ProState]=10 and [BusinessID]=" + bid);
            if (typeid > 0)
                AliceDAL.Common.SqlString(sb, String.Format("TypeID={0}", typeid));
            if (page < 1)
                page = 2;
            Web.Models.ListModel.CreatePage(listmodel, "Pro_Case", sb, page, 10);
            if (page > listmodel.Page.TotalPageCount)
                return Content(JsonConvert.SerializeObject(mm));
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                StringBuilder sb1 = new StringBuilder();
                foreach (var item in listmodel.Page)
                {
                    sb1.Append("<div class=\"gooditem\">" +
                               "  <div class=\"gooditem_bor\">" +
                               "    <div class=\"gooditem_img\">" +
                               "      <a href=\"" + Url.Action("Product", "BearMall", new { pid = item["ID"].ToString() }) + "\">" +
                               "        <img style=\"width: 100%; display: block;height:100px\" src=\"" + item["Img"].ToString() + "\">" +
                               "      </a>" +
                               "    </div>" +
                               "    <a href=\"" + Url.Action("Product", "BearMall", new { pid = item["ID"].ToString() }) + "\">" + item["Title"].ToString() + "</a>" +
                               "    <p>价格：" + item["Price"].ToString() + "元 <i>原价：" + item["Price1"].ToString() + "元</i></p>" +
                               "    <div class=\"gooditem_buy\">" +
                               "      <a href=\"" + Url.Action("ProductOrder", "BearMall", new { pid = item["ID"].ToString() }) + "\" class=\"edo_btn1\">" + (item["ProType"].ToString() == "1" ? "立刻查看" : "立刻购买") + "</a>" +
                               "    </div>" +
                               "  </div>" +
                               "</div>");
                }
                mm.code = "1";
                mm.msg = "success";
                mm.data = sb1.ToString();
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult Product()
        {
            Pro_Case model = DAL.Pro_Case.GetModel(AliceDAL.UrlParam.GetIntValue("pid"));
            return View(model);
        }
        public void Load(int bid)
        {
            ViewData["categoryList"] = DAL.Pro_Types.GetList("[BusinessID]=" + bid.ToString());
        }
        public void LoadBusiType(int id)
        {
            ViewData["businessList"] = DAL.Business_Types.GetChildTypesList(id, 1, true);
        }

        public ActionResult BusinessType(int bid)
        {
            LoadBusiType(bid);
            Business_Types bt = DAL.Business_Types.GetCateIdByID(bid);
            ViewBag.TypeName = bt.TypeName;
            return View();
        }
        public ActionResult GetHotNews()
        {
            MessageModel mm = new MessageModel();
            string lng = UrlParam.GetStringValue("a");
            string lat = UrlParam.GetStringValue("b");
            mm.msg = "暂无优惠信息";
            mm.code = "0";
            AddressModel m = AMap.Common.GetAddress(lng, lat);
            if (m != null && m.CityCode != "" && m.AdCode != "")
            {
                DataTable dt = new DataTable();
                //城市为邯郸不考虑区
                if (m.CityCode == "0310")
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode);//先不考虑缓存
                }
                else
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode, m.AdCode);//先不考虑缓存
                }
                bool result = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result = Web.AMap.Common.IsPtInPoly(AliceDAL.DataType.ConvertToDoubile(lng), AliceDAL.DataType.ConvertToDoubile(lat), row["Points"].ToString());
                        if (result)
                        {
                            mm.code = "1";
                            mm.data = row["ID"].ToString();

                            DataTable dtnews = DAL.PageModel.DateList("[Art_Common]", "[TypeID]=8 and [BranchID]=" + row["ID"].ToString(), "Hot desc,ID", 30);
                            if (dtnews != null && dtnews.Rows.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (DataRow item in dtnews.Rows)
                                {
                                    sb.Append("<li><a href=\"" + (item["Description"].ToString() == "" ? "/BearMall/HotNews/" + item["ID"].ToString() : item["Description"].ToString()) + "\" target=\"" + (item["Description"].ToString() == "" ? "_self" : "_blank") + "\">" +
                                              "<img src=\"" + item["Img"].ToString() + "\" />" +
                                              "<h3>" + item["Title"].ToString() + "</h3></a></li>");
                                }
                                mm.msg = sb.ToString();
                            }
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                    }
                    if (!result)
                    {
                        mm.msg = "暂无优惠信息";
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult GetBusiness()
        {
            MessageModel mm = new MessageModel();
            int typeid = AliceDAL.Common.GetFormInt("typeid");
            int bid = AliceDAL.Common.GetFormInt("bid");

            string lng = AliceDAL.Common.GetFormString("a");
            string lat = AliceDAL.Common.GetFormString("b");
            mm.msg = "暂无入驻商家";
            mm.code = "0";
            AddressModel m = AMap.Common.GetAddress(lng, lat);
            if (m != null && m.CityCode != "" && m.AdCode != "")
            {
                DataTable dt = new DataTable();
                //城市为邯郸不考虑区
                if (m.CityCode == "0310")
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode);//先不考虑缓存
                }
                else
                {
                    dt = DAL.BD_Branch.BranchForCode(m.CityCode, m.AdCode);//先不考虑缓存
                }
                bool result = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result = Web.AMap.Common.IsPtInPoly(AliceDAL.DataType.ConvertToDoubile(lng), AliceDAL.DataType.ConvertToDoubile(lat), row["Points"].ToString());
                        if (result)
                        {
                            mm.code = "1";
                            mm.msg = row["ID"].ToString();

                            Web.Models.ListModel listmodel = new Models.ListModel();
                            StringBuilder sb = new StringBuilder("[TypeID]>0 and [BusinessState]=10 and [BranchID]=" + row["ID"].ToString());
                            if (typeid > 0)
                            {
                                AliceDAL.Common.SqlString(sb, String.Format("TypeID={0}", typeid));
                            }
                            else
                            {
                                List<int> list = DAL.Business_Types.GetChildTypesID(bid, 1);
                                AliceDAL.Common.SqlString(sb, String.Format("TypeID in ({0})", AliceDAL.Common.List2String(list)));
                            }
                            Web.Models.ListModel.CreatePage(listmodel, "[BD_Business]", sb, 1, 10, "SortID desc,ID");

                            if (listmodel.Page != null && listmodel.Page.Count > 0)
                            {
                                StringBuilder sb1 = new StringBuilder();
                                foreach (var item in listmodel.Page)
                                {
                                    sb1.Append("<div class=\"gooditem\">" +
                                               "  <div class=\"gooditem_bor\">" +
                                               "    <div class=\"gooditem_img\">" +
                                               "      <a href=\"" + Url.Action("BusinessProductList", "BearMall", new { bid = item["ID"].ToString() }) + "\">" +
                                               "        <img style=\"width: 100%; display: block;height:100px\" src=\"" + item["Data1"].ToString() + "\">" +
                                               "      </a>" +
                                               "    </div>" +
                                               "    <a href=\"" + Url.Action("BusinessProductList", "BearMall", new { bid = item["ID"].ToString() }) + "\" style=\"padding-left:20px\">" + item["BusinessName"].ToString() + "</a>" +
                                               "  </div>" +
                                               "</div>");
                                }
                                mm.code = row["ID"].ToString();
                                mm.msg = "success";
                                mm.data = sb1.ToString();
                            }
                            return Content(JsonConvert.SerializeObject(mm));
                        }
                    }
                    if (!result)
                    {
                        mm.msg = "暂无入驻商家";
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(mm));
        }
        public ActionResult AjaxBusinessList()
        {
            int typeid = AliceDAL.Common.GetFormInt("typeid");
            int page = AliceDAL.Common.GetFormInt("page");
            int branchid = AliceDAL.Common.GetFormInt("branchid");
            int bid = AliceDAL.Common.GetFormInt("bid");

            Web.Models.MessageModel mm = new Models.MessageModel();
            mm.msg = "没有了";
            mm.code = "0";
            Web.Models.ListModel listmodel = new Models.ListModel();
            StringBuilder sb = new StringBuilder("[TypeID]>0 and [BusinessState]=10 and [BranchID]=" + branchid.ToString());
            if (typeid > 0)
                AliceDAL.Common.SqlString(sb, String.Format("TypeID={0}", typeid));
            if (page < 1)
                page = 2;
            Web.Models.ListModel.CreatePage(listmodel, "[BD_Business]", sb, page, 10, "SortID desc,ID");
            if (page > listmodel.Page.TotalPageCount)
                return Content(JsonConvert.SerializeObject(mm));
            if (listmodel.Page != null && listmodel.Page.Count > 0)
            {
                StringBuilder sb1 = new StringBuilder();
                foreach (var item in listmodel.Page)
                {
                    sb1.Append("<div class=\"gooditem\">" +
                                                "  <div class=\"gooditem_bor\">" +
                                                "    <div class=\"gooditem_img\">" +
                                                "      <a href=\"" + Url.Action("BusinessProductList", "BearMall", new { bid = item["ID"].ToString() }) + "\">" +
                                                "        <img style=\"width: 100%; display: block;height:100px\" src=\"" + item["Data1"].ToString() + "\">" +
                                                "      </a>" +
                                                "    </div>" +
                                                "    <a href=\"" + Url.Action("BusinessProductList", "BearMall", new { bid = item["ID"].ToString() }) + "\" style=\"padding-left:20px\">" + item["BusinessName"].ToString() + "</a>" +
                                                "  </div>" +
                                                "</div>");
                }
                mm.code = "1";
                mm.msg = "success";
                mm.data = sb1.ToString();
            }
            return Content(JsonConvert.SerializeObject(mm));
        }

    }
}