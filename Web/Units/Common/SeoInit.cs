using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Units
{
    public class SeoInit
    {
        public static void InitHead(string title, string keyword, string desc, Controller c)
        {
            string mp = c.RouteData.Values["map"] == null ? "" : c.RouteData.Values["map"].ToString();
            string dq = AliceDAL.CityData.GetCity(mp);

            c.ViewBag.dq = dq;
            if (String.IsNullOrEmpty(dq))
            {
                c.ViewBag.Title = String.IsNullOrWhiteSpace(title) ? AliceDAL.IniHelper.GetValue("WebInfo", "WebName") : title;
                c.ViewBag.KeyWords = String.IsNullOrWhiteSpace(keyword) ? AliceDAL.IniHelper.GetValue("WebInfo", "KeyWords") : keyword;
                c.ViewBag.Description = String.IsNullOrWhiteSpace(desc) ? AliceDAL.IniHelper.GetValue("WebInfo", "Description") : desc;
            }
            else
            {
                string tit = String.IsNullOrWhiteSpace(title) ? AliceDAL.IniHelper.GetValue("WebInfo", "WebName") : title;
                string[] titles = tit.Split('_');
                for (int i = 0; i < titles.Length; i++)
                {
                    titles[i] = dq + titles[i];
                }
                c.ViewBag.Title = string.Join("_", titles);
                string[] key = (String.IsNullOrWhiteSpace(keyword) ? AliceDAL.IniHelper.GetValue("WebInfo", "KeyWords") : keyword).Split(',');
                if (key != null)
                {
                    for (int i = 0; i < key.Length; i++)
                    {
                        key[i] = dq + key[i];
                    }
                }
                c.ViewBag.KeyWords = String.Join(",", key);
                c.ViewBag.Description = dq + (String.IsNullOrWhiteSpace(desc) ? AliceDAL.IniHelper.GetValue("WebInfo", "Description") : desc) + c.ViewBag.Title;
            }
        }
    }
}