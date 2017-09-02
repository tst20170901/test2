using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Coupons
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
        /// Uid
        /// </summary>		
        private int _uid;
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// UserOid
        /// </summary>		
        private int _useroid;
        public int UserOid
        {
            get { return _useroid; }
            set { _useroid = value; }
        }
        /// <summary>
        /// TypeID
        /// </summary>		
        private int _typeid;
        public int TypeID
        {
            get { return _typeid; }
            set { _typeid = value; }
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
        /// CouponState
        /// </summary>		
        private int _couponstate;
        public int CouponState
        {
            get { return _couponstate; }
            set { _couponstate = value; }
        }
        /// <summary>
        /// TDate
        /// </summary>		
        private DateTime _tdate;
        public DateTime TDate
        {
            get { return _tdate; }
            set { _tdate = value; }
        }
        /// <summary>
        /// UseDate
        /// </summary>		
        private DateTime _usedate = new DateTime(1900, 1, 1);
        public DateTime UseDate
        {
            get { return _usedate; }
            set { _usedate = value; }
        }
        /// <summary>
        /// CDate
        /// </summary>		
        private DateTime _cdate;
        public DateTime CDate
        {
            get { return _cdate; }
            set { _cdate = value; }
        }
        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}