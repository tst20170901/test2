using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class VipType
    {
        /// <summary>
        /// ID
        /// </summary>		
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Title
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _vipDes;
        public string VipDes
        {
            get { return _vipDes; }
            set { _vipDes = value; }
        }
        private int branchID;
        public int BranchID
        {
            get { return branchID; }
            set { branchID = value; }
        }
        private string branchName;
        public string BranchName
        {
            get { return branchName; }
            set { branchName = value; }
        }
        private string businessName;
        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; }
        }
        /// <summary>
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private int _count;
        /// <summary>
        /// 多少张
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        private int _businessID;
        public int BusinessID
        {
            get { return _businessID; }
            set { _businessID = value; }
        }
        private int _freeID;
        public int FreeID
        {
            get { return _freeID; }
            set { _freeID = value; }
        }
        private int _vipTypeState;
        public int VipTypeState
        {
            get { return _vipTypeState; }
            set { _vipTypeState = value; }
        }
        private string _smsContent;
        public string SMSContent
        {
            get { return _smsContent; }
            set { _smsContent = value; }
        }
        private int _period;
        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }
        private DateTime _cdate;
        public DateTime CDate
        {
            get { return _cdate; }
            set { _cdate = value; }
        }
        private string _data1;
        public string Data1
        {
            get { return _data1; }
            set { _data1 = value; }
        }
        private string _data2;
        public string Data2
        {
            get { return _data2; }
            set { _data2 = value; }
        }
        private string _data3;
        public string Data3
        {
            get { return _data3; }
            set { _data3 = value; }
        }
        private string _data4;
        public string Data4
        {
            get { return _data4; }
            set { _data4 = value; }
        }

        private string _data5;
        public string Data5
        {
            get { return _data5; }
            set { _data5 = value; }
        }
        private string _data6;
        public string Data6
        {
            get { return _data6; }
            set { _data6 = value; }
        }
        private string _data7;
        public string Data7
        {
            get { return _data7; }
            set { _data7 = value; }
        }
        private string _data8;
        public string Data8
        {
            get { return _data8; }
            set { _data8 = value; }
        }
        private string _data9;
        public string Data9
        {
            get { return _data9; }
            set { _data9 = value; }
        }
        private string _data10;
        public string Data10
        {
            get { return _data10; }
            set { _data10 = value; }
        }
        /// <summary>
        /// 1线下卡；2线上卡
        /// </summary>
        private int _online;
        public int Online
        {
            get { return _online; }
            set { _online = value; }
        }
        /// <summary>
        /// 1不绑定，2绑定
        /// </summary>
        private int _lockPlate;
        public int LockPlate
        {
            get { return _lockPlate; }
            set { _lockPlate = value; }
        }
        /// <summary>
        /// 卡数量，等于零时自动下架，仅限线上卡
        /// </summary>
        private int cardCount;
        public int CardCount
        {
            get { return cardCount; }
            set { cardCount = value; }
        }
    }
}