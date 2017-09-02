using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AliceDAL
{
    public class Error : Exception
    {
        /// <summary>
        /// 把错误日志写在txt文件里
        /// </summary>
        /// <param name="errorMessage">错误消息：错误的消息需要包括：报错的页面(或类)和方法</param>
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                string logFileName = "bin\\ErrlogFile\\ErrLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFileName, true, System.Text.Encoding.Default);
                if (sw != null)
                {
                    sw.Write(DateTime.Now.ToString() + ": ___" + string.Format("Class:{0}___MethodName:{1}___ErrorMessage:{2}", ex.TargetSite.DeclaringType.FullName, ex.TargetSite.Name, ex.Message) + "\r\n");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch { }
        }
        public static void WriteErrorLog(string msg)
        {
            try
            {
                string logFileName = "bin\\ErrlogFile\\ErrLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFileName, true, System.Text.Encoding.Default);
                if (sw != null)
                {
                    sw.Write(DateTime.Now.ToString() + ": ___" + string.Format("ErrorMessage:{0}", msg) + "\r\n");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch { }
        }
        public static void WriteErrorLogTemp(string msg)
        {
            try
            {
                string logFileName = "bin\\ErrlogFile\\Messages_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFileName, true, System.Text.Encoding.Default);
                if (sw != null)
                {
                    sw.Write(DateTime.Now.ToString() + ": ___" + string.Format("ErrorMessage:{0}", msg) + "\r\n");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch { }
        }
    }
}
