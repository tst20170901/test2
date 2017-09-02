using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_Users
    {
        public int ID { get; set; }

        private string loginID;
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value.TrimEnd(); }
        }
        public string UserPwd { get; set; }
        public string Mobile { get; set; }

        public DateTime CDate { get; set; }

        private string data1;
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Data1
        {
            get { return data1; }
            set { data1 = value.TrimEnd(); }
        }
        private string data2;
        /// <summary>
        /// 角色 1是普通客户 2是大客户，只能清洗限定车牌号
        /// </summary>
        public string Data2
        {
            get { return data2; }
            set { data2 = value == null ? "" : value.TrimEnd(); }
        }
        private string data3;

        public string Data3
        {
            get { return data3; }
            set { data3 = value == null ? "" : value.TrimEnd(); }
        }
        private string data4;

        public string Data4
        {
            get { return data4; }
            set { data4 = value == null ? "" : value.TrimEnd(); }
        }
        private string data5;

        public string Data5
        {
            get { return data5; }
            set { data5 = value == null ? "" : value.TrimEnd(); }
        }
        private string data6;

        public string Data6
        {
            get { return data6; }
            set { data6 = value == null ? "" : value.TrimEnd(); }
        }
        private string data7;

        public string Data7
        {
            get { return data7; }
            set { data7 = value == null ? "" : value.TrimEnd(); }
        }
        private string data8;
        /// <summary>
        /// 上次检测时间
        /// </summary>
        public string Data8
        {
            get { return data8; }
            set { data8 = value == null ? "" : value.TrimEnd(); }
        }
        private string data9;
        /// <summary>
        /// 是否预备用户 1为预备用户其他为普通用户
        /// </summary>
        public string Data9
        {
            get { return data9; }
            set { data9 = value == null ? "" : value.TrimEnd(); }
        }
        private string data10;
        /// <summary>
        /// 验证码
        /// </summary>
        public string Data10
        {
            get { return data10; }
            set { data10 = value == null ? "" : value.TrimEnd(); }
        }
        private int _branchID;
        public int BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
        }
        private int _ischeck;
        public int IsCheck
        {
            get { return _ischeck; }
            set { _ischeck = value; }
        }
    }
}

