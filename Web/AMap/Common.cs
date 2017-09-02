using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using AliceDAL;
using Web.Models;

namespace Web.AMap
{
    public class Common
    {
        public static AddressModel GetAddress(string lng, string lat)
        {
            AddressModel model = new AddressModel() { Lng = lng, Lat = lat };
            try
            {
                string url = string.Format("http://restapi.amap.com/v3/geocode/regeo?key={0}&location={1},{2}&output={3}", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), lng, lat, "xml");
                string xmldata = RequestUrl(url);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmldata);
                XmlNode root = xml.DocumentElement;
                XmlNodeList nodelist = xml.SelectNodes("/response/regeocode");
                if (nodelist != null && nodelist.Count > 0)
                {
                    XmlElement xe = (XmlElement)nodelist[0];
                    XmlNodeList xnl0 = xe.ChildNodes;
                    model.FormatAddress = xnl0.Item(0).InnerText;
                    XmlNode xnl1 = xnl0.Item(1);
                    if (xnl1 != null)
                    {
                        model.Province = xnl1["province"].InnerText;
                        model.City = xnl1["city"].InnerText == "" ? model.Province : xnl1["city"].InnerText;
                        model.CityCode = xnl1["citycode"].InnerText;
                        model.District = xnl1["district"].InnerText;
                        model.AdCode = xnl1["adcode"].InnerText;
                    }
                }
                return model;
            }
            catch
            {
                return null;
            }
        }
        public static List<WorkShop> InitDistance(List<WorkShop> listws, string lng, string lat)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in listws)
            {
                sb.Append(item.Lng1 + "," + item.Lat1 + "|");
            }
            List<Distance> list = GetDistance(sb.ToString(), lng + "," + lat);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    listws[i].Distan = list[i].Distan;
                    listws[i].Dur = list[i].Dur;
                    listws[i].OrderTime += list[i].Dur / 60;
                }
            }
            return listws;
        }
        public static List<WorkShop> InitDistanceCarWash(List<WorkShop> listws, string lng, string lat)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in listws)
            {
                sb.Append(item.Lng + "," + item.Lat + "|");
            }
            List<Distance> list = GetDistance(sb.ToString(), lng + "," + lat);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    listws[i].Distan = GetDistance(DataType.ConvertToDoubile(listws[i].Lat), DataType.ConvertToDoubile(listws[i].Lng), DataType.ConvertToDoubile(lat), DataType.ConvertToDoubile(lng));// list[i].Distan;
                    listws[i].Dur = list[i].Dur;
                    listws[i].OrderTime += list[i].Dur / 60;
                }
            }
            return listws;
        }
        /// <summary>
        /// 高德地图规划路径测距
        /// </summary>
        /// <param name="flng"></param>
        /// <param name="flat"></param>
        /// <param name="tlng"></param>
        /// <param name="tlat"></param>
        /// <returns></returns>
        public static Distance GetDistance(string flng, string flat, string tlng, string tlat)
        {
            Distance model = new Distance() { FLng = flng, FLat = flat, TLng = tlng, TLat = tlat };
            try
            {
                string url = string.Format("http://restapi.amap.com/v3/distance?key={0}&origins={1},{2}&destination={3},{4}&output=xml", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), flng, flat, tlng, tlat);

                string xmldata = RequestUrl(url);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmldata);
                XmlNode root = xml.DocumentElement;
                XmlNodeList nodelist = xml.SelectNodes("/response/results/result");
                if (nodelist != null && nodelist.Count > 0)
                {
                    XmlElement xe = (XmlElement)nodelist[0];
                    model.Distan = AliceDAL.DataType.ConvertToInt(xe["distance"].InnerText);
                    model.Dur = AliceDAL.DataType.ConvertToInt(xe["duration"].InnerText);
                }
                return model;
            }
            catch
            {
                return null;
            }
        }

        public static List<Distance> GetDistance(string f, string t)
        {
            try
            {
                List<Distance> list = new List<Distance>();
                string url = string.Format("http://restapi.amap.com/v3/distance?key={0}&origins={1}&destination={2}&output=xml", AliceDAL.IniHelper.GetValue("WebInfo", "Data2"), f, t);

                string xmldata = RequestUrl(url);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmldata);
                XmlNode root = xml.DocumentElement;
                XmlNodeList nodelist = xml.SelectNodes("/response/results/result");
                if (nodelist != null && nodelist.Count > 0)
                {
                    foreach (XmlElement item in nodelist)
                    {
                        Distance model = new Distance();
                        model.Distan = AliceDAL.DataType.ConvertToInt(item["distance"].InnerText);
                        model.Dur = AliceDAL.DataType.ConvertToInt(item["duration"].InnerText);
                        list.Add(model);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        public static string RequestUrl(string url)
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

        public static string PostUrl(string url, string data)
        {

            byte[] dataArray = Encoding.Default.GetBytes(data);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //创建输入流
            Stream dataStream = null;
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败
            }

            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                return null;//连接服务器失败
            }
            return res;
        }
        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public static int GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000 * 1000;
            return (int)Math.Ceiling(s);
        }
        public static bool IsPtInPolys(double ALon, double ALat, string Points)
        {







            int iSum = 0, iCount;
            double dLon1, dLon2, dLat1, dLat2, dLon;
            string[] p = AliceDAL.Common.SplitString(Points, ";");
            if (p.Length < 3) return false;

            List<Point> APoints = new List<Point>();
            foreach (string item in p)
            {
                APoints.Add(new Point() { X = AliceDAL.DataType.ConvertToDoubile(AliceDAL.Common.SplitString(item)[0]), Y = AliceDAL.DataType.ConvertToDoubile(AliceDAL.Common.SplitString(item)[1]) });
            }

            iCount = p.Length;
            for (int i = 0; i < iCount - 1; i++)
            {
                if (i == iCount - 1)
                {
                    dLon1 = APoints[i].X;
                    dLat1 = APoints[i].Y;
                    dLon2 = APoints[0].X;
                    dLat2 = APoints[0].Y;
                }
                else
                {
                    dLon1 = APoints[i].X;
                    dLat1 = APoints[i].Y;
                    dLon2 = APoints[i + 1].X;
                    dLat2 = APoints[i + 1].Y;
                }
                //以下语句判断A点是否在边的两端点的水平平行线之间，在则可能有交点，开始判断交点是否在左射线上
                if (((ALat >= dLat1) && (ALat < dLat2)) || ((ALat >= dLat2) && (ALat < dLat1)))
                {
                    if (Math.Abs(dLat1 - dLat2) > 0)
                    {
                        //得到 A点向左射线与边的交点的x坐标：
                        dLon = dLon1 - ((dLon1 - dLon2) * (dLat1 - ALat)) / (dLat1 - dLat2);

                        // 如果交点在A点左侧（说明是做射线与 边的交点），则射线与边的全部交点数加一：
                        if (dLon < ALon)
                            iSum++;
                    }
                }
            }
            if (iSum % 2 != 0)
                return true;
            return false;
        }

        private static double isLeft(Point P0, Point P1, Point P2)
        {
            double abc = ((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
            return abc;


        }
        public static bool IsPtInPoly(double ALon, double ALat, string Points)
        {
            string[] p = AliceDAL.Common.SplitString(Points, ";");
            if (p.Length < 3) return false;
            Point pnt1 = new Point() { X = ALon, Y = ALat };

            List<Point> APoints = new List<Point>();
            foreach (string item in p)
            {
                APoints.Add(new Point() { X = AliceDAL.DataType.ConvertToDoubile(AliceDAL.Common.SplitString(item)[0]), Y = AliceDAL.DataType.ConvertToDoubile(AliceDAL.Common.SplitString(item)[1]) });
            }
            int wn = 0, j = 0; //wn 计数器 j第二个点指针
            for (int i = 0; i < APoints.Count; i++)
            {//开始循环
                if (i == APoints.Count - 1)
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点
                else
                    j = j + 1; //如果不是 ，则找下一点

                if (APoints[i].Y <= pnt1.Y) // 如果多边形的点 小于等于 选定点的 Y 坐标
                {
                    if (APoints[j].Y > pnt1.Y) // 如果多边形的下一点 大于于 选定点的 Y 坐标
                    {
                        if (isLeft(APoints[i], APoints[j], pnt1) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (APoints[j].Y <= pnt1.Y)
                    {
                        if (isLeft(APoints[i], APoints[j], pnt1) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
                return false;
            else
                return true;
        }
    }
}