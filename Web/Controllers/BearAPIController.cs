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
namespace Web.Controllers
{
    public class BearAPIController : UserBaseController
    {
        public ActionResult geo(string address, string output = "json")
        {
            string url = string.Format("http://restapi.amap.com/v3/geocode/geo?key={0}&address={1}&output={2}", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), address, output);
            return Content(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(RequestUrl(url))));
        }
        public ActionResult regeo(string lng, string lat, string output = "json")
        {
            string url = string.Format("http://restapi.amap.com/v3/geocode/regeo?key={0}&location={1},{2}&output={3}", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), lng, lat, output);
            string jsondata=RequestUrl(url);
            return Content(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(jsondata)));
        }
        public ActionResult driving(string lng, string lat, string tlng, string tlat, string extensions = "base", string output = "json")
        {
            string url = string.Format("http://restapi.amap.com/v3/direction/driving?key={0}&origin={1},{2}&destination={3},{4}&extensions={5}&output={6}", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), lng, lat, tlng, tlat, extensions, output);
            return Content(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(RequestUrl(url))));
        }

        public ActionResult distance(string lng, string lat, string tlng, string tlat, string output = "json")
        {
            string url = string.Format("http://restapi.amap.com/v3/distance?key={0}&origins={1},{2}&destination={3},{4}&output={5}", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), lng, lat, tlng, tlat, output);
            return Content(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(RequestUrl(url))));

        }
        private static string RequestUrl(string url)
        {
            Uri requestUri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "GET";
            string result = "";
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            }
            return result;
        }
    }
}