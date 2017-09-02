using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
namespace AliceDAL
{
    public class Common
    {

        #region 编码
        /// <summary>
        /// HTML解码
        /// </summary>
        /// <returns></returns>
        public static string HtmlDecode(string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <returns></returns>
        public static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <returns></returns>
        public static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        #endregion

        #region Cookie

        /// <summary>
        /// 删除指定名称的Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        public static void DeleteCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得指定名称的Cookie值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                return cookie.Value;
            return string.Empty;
        }

        /// <summary>
        /// 获得指定名称的Cookie中特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetCookie(string name, string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null && cookie.HasKeys)
            {
                string v = cookie[key];
                if (v != null)
                    return HttpUtility.UrlDecode(v, Encoding.GetEncoding("UTF-8")); ;
            }

            return string.Empty;
        }

        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                cookie.Value = value;
            else
                cookie = new HttpCookie(name, value);

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string key, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie[key] = HttpUtility.UrlEncode(value, Encoding.GetEncoding("UTF-8"));
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion


        /// <summary>
        /// 创建随机值
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="onlyNumber">是否只包含数字</param>
        /// <returns>随机值</returns>
        public static string CreateRandomValue(int length, bool onlyNumber)
        {
            Random _random = new Random();//随机发生器
            string s = "123456789abcdefghjkmnpqrstuvwxy";
            char[] _randomlibrary = s.ToCharArray();//随机库
            int index;
            StringBuilder randomValue = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                if (onlyNumber)
                    index = _random.Next(0, 9);
                else
                    index = _random.Next(0, _randomlibrary.Length);

                randomValue.Append(_randomlibrary[index]);
            }

            return randomValue.ToString();
        }
        public static string CreatePwd(int length, int rand)
        {
            Random random = new Random(rand);
            string chars = "ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678";/****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/

            char[] _randomlibrary = chars.ToCharArray();//随机库
            int index;
            StringBuilder pwd = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                index = random.Next(0, _randomlibrary.Length);
                pwd.Append(_randomlibrary[index]);
            }
            return pwd.ToString();
        }
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        public static string SaveImg(HttpPostedFileBase img)
        {
            if (img == null) return "-1";
            string fileName = img.FileName;
            string extension = Path.GetExtension(fileName).ToLower();
            if (".jpg" != extension && ".gif" != extension && ".png" != extension && ".bmp" != extension && ".jpeg" != extension) return "-2";
            string path = GetMapPath("/uploadfiles/");
            string name = "idodo" + DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string newname = "/uploadfiles/" + name + extension;
            img.SaveAs(path + name + extension);
            return newname;
        }
        /// <summary>
        /// 隐藏手机
        /// </summary>
        public static string HideMobile(string mobile)
        {
            if (mobile != null && mobile.Length > 10)
                return mobile.Substring(0, 3) + "*****" + mobile.Substring(8);
            return string.Empty;
        }
        /// <summary>
        /// 隐藏车牌号
        /// </summary>
        public static string HidePlate(string plate)
        {
            if (plate != null && plate.Length > 6)
                return plate.Substring(0, 2) + "**" + plate.Substring(4);
            return string.Empty;
        }
        public static string GetHtmlBS(int count)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < count; i++)
                result.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            return result.ToString();
        }
        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">分隔字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
                return new string[0] { };

            if (sourceStr.IndexOf(splitStr) == -1)
                return new string[] { sourceStr };

            if (splitStr.Length == 1)
                return sourceStr.Split(splitStr[0]);
            else
                return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr)
        {
            return SplitString(sourceStr, ",");
        }

        #endregion
        public static string SubString(string sourceStr, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.Length >= (startIndex + length))
                    return sourceStr.Substring(startIndex, length);
                else
                    return sourceStr.Substring(startIndex);
            }

            return "";
        }

        public static string SubString(string sourceStr, int length)
        {
            return SubString(sourceStr, 0, length);
        }
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        public static string GetUrlReferrer()
        {
            Uri uri = HttpContext.Current.Request.UrlReferrer;
            if (uri == null)
                return string.Empty;
            return uri.ToString();
        }
        public static long GetSecond(string obj)
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            DateTime dt2 = DateTime.Parse(obj);
            long Sticks = (dt2.Ticks - dt1.Ticks) / 10000000;
            return Sticks;
        }
        public static bool IsEmail(string email)
        {
            Regex reg = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }
        public static bool IsMobile(string mobile)
        {
            Regex reg = new Regex(@"^1\d{10}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(mobile);
        }
        public static string GetIP()
        {
            string ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            if (string.IsNullOrEmpty(ip) || !ValidateHelper.IsIP(ip))
                ip = "127.0.0.1";
            return ip;
        }
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod == "GET";
        }
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod == "POST";
        }
        public static bool IsAjax()
        {
            return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        public static string GetFormString(string key, string defaultValue)
        {
            string value = HttpContext.Current.Request.Form[key];
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            else
                return defaultValue;
        }
        public static string GetFormString(string key)
        {
            return GetFormString(key, "");
        }
        public static int GetFormInt(string key, int defaultValue)
        {
            return DataType.ConvertToInt(HttpContext.Current.Request.Form[key]);
        }
        public static int GetFormInt(string key)
        {
            return GetFormInt(key, 0);
        }
        public static decimal GetFormDecimal(string key, int defaultValue)
        {
            return DataType.ConvertToDecimal(HttpContext.Current.Request.Form[key]);
        }
        public static decimal GetFormDecimal(string key)
        {
            return GetFormDecimal(key, 0);
        }
        public static string List2String(List<int> lst)
        {
            if (lst == null || lst.Count <= 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (int str in lst)
            {
                sb.Append("'" + str.ToString() + "',");
            }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }
        public static string List2String(List<string> lst)
        {
            if (lst == null || lst.Count <= 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (string str in lst)
            {
                sb.Append("'" + str + "',");
            }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }
        public static string Ints2String(int[] lst)
        {
            if (lst == null || lst.Length <= 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (int str in lst)
            {
                sb.Append("'" + str + "',");
            }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }
        public static string Strings2String(string[] str)
        {
            if (str == null || str.Length <= 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (string s in str)
            {
                sb.Append(s + ",");
            }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }
        public static string DelHTML(string Htmlstring)//将HTML去除
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;

        }
        public static void SqlString(ref string strsql, string strCom)
        {
            strsql += strsql.Length > 0 ? " and " : "";
            strsql += strCom;
        }
        public static void SqlString(StringBuilder sb, string strCom)
        {
            sb.Append(sb.Length > 0 ? " and " : "");
            sb.Append(strCom);
        }
        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="inputStr">字符串</param>
        /// <returns>string</returns>
        public static string cutBadStr(string str)
        {
            if (String.IsNullOrWhiteSpace(str)) return str;
            string[] pattern = { "select", "insert", "delete", "from", "count\\(", "drop table", "update", "truncate", "asc\\(", "mid\\(", "char\\(", "xp_cmdshell", "exec   master", "netlocalgroup administrators", "net user", "or", "and" };
            for (int i = 0; i < pattern.Length; i++)
            {
                str = str.Replace(pattern[i].ToString(), "");
            }
            return str;
        }
        #endregion
        public static DataTable RetDataTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0) return null;
            return dt;
        }
        public static DataRow RetDateRow(DataTable dt)
        {
            if (null == dt || dt.Rows.Count <= 0) return null;
            return dt.Rows[0];
        }
        public static DataTable RetDataTable(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return null;
            return ds.Tables[0];
        }
        public static DataSet RetDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0) return null;
            return ds;
        }
        public static string CutString(string str, int max, string end)
        {
            if (str.Length > max) str = str.Substring(0, max) + end;
            return str;
        }
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="key">要加密的字符串</param>
        /// <returns>加密完的字符串</returns>
        public static string String2MD5(string key)
        {
            string cl = key;
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x");
            }
            return pwd.Substring(8, 16);
        }
        public static string RedFont(string str, string keys)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (String.IsNullOrEmpty(keys)) return str;
            return str.ToLower().Replace(AliceDAL.Common.DelHTML(keys).ToLower(), "<span style=\"color:#F00;\">" + AliceDAL.Common.DelHTML(keys).ToLower() + "</span>");
        }

        public static string UpLoad(FileUpload fu, System.Web.UI.Page pager)
        {
            Random rand = new Random();
            string dirtemp = "/uploadfiles/";
            #region 保存图片Img
            if (fu.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fu.FileName).ToLower();
                if (".jpg" == extension || ".gif" == extension || ".png" == extension)
                {
                    string datepath = Guid.NewGuid().ToString();
                    string path = HttpContext.Current.Server.MapPath(dirtemp);
                    string pathname = datepath + extension;
                    fu.SaveAs(path + pathname);
                    return dirtemp + pathname;
                }
                else return "";
            }
            else return "";
            #endregion
        }
        public static string UpLoad(FileUpload fu, System.Web.UI.Page pager, HiddenField hf)
        {
            string dirtemp = "/uploadfiles/";
            #region 保存图片Img
            if (fu.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fu.FileName).ToLower();
                if (".jpg" == extension || ".gif" == extension || ".png" == extension)
                {
                    string guid = Guid.NewGuid().ToString();
                    string path = HttpContext.Current.Server.MapPath(dirtemp);
                    string pathname = guid + extension;
                    fu.SaveAs(path + pathname);
                    return dirtemp + pathname;
                }
                else return hf.Value;
            }
            else return hf.Value;
            #endregion
        }
    }
}
