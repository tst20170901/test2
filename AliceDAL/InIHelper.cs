using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
namespace AliceDAL
{
    public static class IniHelper
    {

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        //读取 
        public static string GetValue(string Section, string key)
        {
            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\WebSet.ini";
            if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
                return temp.ToString();
            }
            return "";
        }
        //设置值
        public static void SetValue(string Section, string key, string value)
        {
            WritePrivateProfileString(Section, key, value, AppDomain.CurrentDomain.BaseDirectory + "App_Data\\WebSet.ini");
        }
    }
}