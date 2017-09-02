using System;
using System.Collections.Generic;
using System.Text;
namespace AliceDAL
{
    public class DataType
    {
        /// <summary>
        /// 字符串转换为整数
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <returns>失败：-1 ； 成功：int值 </returns>
        public static int ConvertToInt(string value)
        {   ///数据为空，返回-1
            if (string.IsNullOrEmpty(value)) return -1;
            int result = -1;

            ///执行转换操作
            Int32.TryParse(value, out result);

            return result;   ///返回结果
        }

        /// <summary>
        /// 字符串转换为时间
        /// </summary>
        /// <param name="value">转换时间类型</param>
        /// <returns>失败：返回 1900-01-01 ; 成功：时间类型值</returns>
        public static DateTime ConvertToDateTime(string value)
        {   ///定义初始化值
            DateTime result;
            if (string.IsNullOrEmpty(value)) return DateTime.Now;
            ///执行转换操作
            DateTime.TryParse(value, out result);
            return DateTime.Parse(value);   ///返回结果
        }
        public static string ConvertToDateTimeStr(object value)
        {   ///定义初始化值
            DateTime result;
            if (string.IsNullOrEmpty(value.ToString())) return DateTime.Now.ToString("yyyy/MM/dd");
            ///执行转换操作
            DateTime.TryParse(value.ToString(), out result);
            return DateTime.Parse(value.ToString()).ToString("yyyy/MM/dd");
        }
        public static string ConvertToDateTimeStrAll(object value)
        {   ///定义初始化值
            DateTime result;
            if (string.IsNullOrEmpty(value.ToString())) return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ///执行转换操作
            DateTime.TryParse(value.ToString(), out result);
            return DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");   ///返回结果
        }
        /// <summary>
        /// 字符串转换为实数
        /// </summary>
        /// <param name="value">转小数</param>
        /// <returns>失败：0.0M ；成功：小数值</returns>
        public static decimal ConvertToDecimal(string value)
        {   ///定义初始化值
            decimal result = 0.0M;
            if (string.IsNullOrEmpty(value)) return result;
            ///执行转换操作
            decimal.TryParse(value, out result);
            return result;   ///返回结果
        }
        public static double ConvertToDoubile(string value)
        {   ///定义初始化值
            double result = 0.0d;
            if (string.IsNullOrEmpty(value)) return result;
            ///执行转换操作
            double.TryParse(value, out result);
            return result;   ///返回结果
        }
        /// <summary>
        /// 字符串转换为GUID
        /// </summary>
        /// <param name="value">转小数</param>
        /// <returns>失败：00000000-0000-0000-0000-000000000000；  成功：GUID</returns>
        public static Guid ConvertToGUID(string value)
        {   ///定义初始化值
            Guid result = new Guid();
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }
            ///执行转换操作
            result = new Guid(value);
            return result;   ///返回结果
        }
        /// <summary>
        /// 字符串转换为布尔类型
        /// </summary>
        /// <param name="value">转bool值  1：true ； 0：false</param>
        /// <returns>转换失败返回 false 成功：bool值</returns>
        public static bool ConvertToBoolean(string value)
        {
            if (value == "1")
            {
                //如果1 表示true
                return true;
            }
            else if (value == "0")
            {
                //如果1 表示false
                return false;
            }
            ///定义初始化值
            bool result = false;
            if (string.IsNullOrEmpty(value)) return false;
            ///执行转换操作
            bool.TryParse(value, out result);
            return result;   ///返回结果
        }
    }
}