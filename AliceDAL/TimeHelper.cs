using System;
using System.Text.RegularExpressions;

namespace AliceDAL
{
    public class TimeHelper
    {
        /// <summary>
        /// 当前时间是否在两个时间内
        /// </summary>
        /// <returns></returns>
        public static int IsInTime(DateTime dt1, DateTime dt2)
        {
            DateTime now = DateTime.Now;
            DateTime dt1tp = new DateTime(now.Year, now.Month, now.Day, dt1.Hour, dt1.Minute, dt1.Second);
            DateTime dt2tp = new DateTime(now.Year, now.Month, now.Day, dt2.Hour, dt2.Minute, dt2.Second);
            if (now >= dt1tp && (now <= dt2tp))
            {
                return 1;
            }
            else if (now < dt1tp)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }
        /// <summary>
        /// 根据当前时间获取时间点
        /// </summary>
        /// <returns></returns>
        public static int GetTimeNode()
        {
            int result = 8;
            if (DateTime.Now < DateTime.Parse("08:00"))
            {
                result = 8;
            }
            else if (DateTime.Now >= DateTime.Parse("19:00"))
            {
                result = 19;
            }
            else
            {
                result = DateTime.Now.Hour;
            }
            return result;
        }
    }
}
