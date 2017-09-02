using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace AliceDAL
{
    public class UrlParam
    {
        #region 获取URL中的参数
        /// <summary>
        /// 获取地址栏参数 返回数值型，如果没有key  返回-1
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果没有key  返回-1</returns>
        public static int GetIntValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return -1;
            return DataType.ConvertToInt(HttpContext.Current.Request[key].ToString());
        }
        /// <summary>
        /// 获取地址栏参数 返回字符串，如果没有key  返回Null
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果没有key  返回Null</returns>
        public static string GetStringValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return null;
            string temp = HttpContext.Current.Request[key].ToString().Split(new char[] { ',' })[0];
            return temp;
        }
        /// <summary>
        /// 获取地址栏参数 返回字符串，如果没有key  返回Null
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果没有key  返回Null</returns>
        public static string GetNalStringValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return null;
            string temp = HttpContext.Current.Request[key].ToString();
            return temp;
        }
        /// <summary>
        /// 获取地址栏参数 返回小数，如果没有key  返回0.0m
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果没有key  返回0.0m</returns>
        public static decimal GetDecimalValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return 0.0m;
            return DataType.ConvertToDecimal(HttpContext.Current.Request[key].ToString());
        }
        /// <summary>
        /// 获取地址栏参数 返回小数，如果没有key  返回Null
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果没有key  返回 false</returns>
        public static bool GetBoolValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return false;
            return DataType.ConvertToBoolean(HttpContext.Current.Request[key].ToString());
        }
        /// <summary>
        /// 获取地址栏参数 返回时间类型，如果没有key  返回 1900-1-1
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>如果失败返回1900-1-1 成功返回 地址栏时间</returns>
        public static string GetDateTimeValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return "1900-01-01";
            return DataType.ConvertToDateTime(HttpContext.Current.Request[key].ToString()).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取地址栏参数 返回GUID值， 如果没有key  返回  00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>失败：00000000-0000-0000-0000-000000000000；  成功：GUID</returns>
        public static Guid GetGuidValue(string key)
        {
            if (HttpContext.Current.Request[key] == null) return new Guid();
            return DataType.ConvertToGUID(HttpContext.Current.Request[key].ToString());
        }
        #endregion
    }
}