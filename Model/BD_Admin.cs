using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_Admin
    {
        public int ID { get; set; }

        private string loginID;
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value.TrimEnd(); }
        }
        public string Password { get; set; }

        private string nickname;

        public string NickName
        {
            get { return nickname; }
            set { nickname = (value == null ? "" : value.TrimEnd()); }
        }
        private int _roleID;

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }
        //所属公司ID
        private int _branchID;
        public int BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
        }
        private string data1;

        public string Data1
        {
            get { return data1; }
            set { data1 = value.TrimEnd(); }
        }
        private string data2;

        public string Data2
        {
            get { return data2; }
            set { data2 = value.TrimEnd(); }
        }
        private string data3;

        public string Data3
        {
            get { return data3; }
            set { data3 = value.TrimEnd(); }
        }
        private string data4;

        public string Data4
        {
            get { return data4; }
            set { data4 = value.TrimEnd(); }
        }
        private string data5;

        public string Data5
        {
            get { return data5; }
            set { data5 = value.TrimEnd(); }
        }
        private string data6;

        public string Data6
        {
            get { return data6; }
            set { data6 = value.TrimEnd(); }
        }
        private string data7;

        public string Data7
        {
            get { return data7; }
            set { data7 = value.TrimEnd(); }
        }
        private string data8;

        public string Data8
        {
            get { return data8; }
            set { data8 = value.TrimEnd(); }
        }
        private string data9;

        public string Data9
        {
            get { return data9; }
            set { data9 = value.TrimEnd(); }
        }
        private string data10;

        public string Data10
        {
            get { return data10; }
            set { data10 = value.TrimEnd(); }
        }
    }
}

