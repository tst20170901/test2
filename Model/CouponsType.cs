using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class CouponsType
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
        /// <summary>
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// OriginalPrice
        /// </summary>		
        private decimal _originalprice;
        public decimal OriginalPrice
        {
            get { return _originalprice; }
            set { _originalprice = value; }
        }
        /// <summary>
        /// 有效期
        /// </summary>		
        private int _period;
        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }
        private int _count;
        /// <summary>
        /// 多少张洗车券
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
        /// <summary>
        /// 1就是特殊
        /// </summary>		
        private int _isspecial;
        public int IsSpecial
        {
            get { return _isspecial; }
            set { _isspecial = value; }
        }
        private string _smsContent;
        public string SMSContent
        {
            get { return _smsContent; }
            set { _smsContent = value; }
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
    }
}