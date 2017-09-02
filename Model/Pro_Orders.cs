using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Pro_Orders
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
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        private string _discountDes;
        /// <summary>
        /// 优惠方式
        /// </summary>
        public string DiscountDes
        {
            get { return _discountDes; }
            set { _discountDes = value; }
        }
        private decimal _discountAmount;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmount
        {
            get { return _discountAmount; }
            set { _discountAmount = value; }
        }
        private int _vipCardID;
        /// <summary>
        /// 选用会员卡的ID
        /// </summary>
        public int VipCardID
        {
            get { return _vipCardID; }
            set { _vipCardID = value; }
        }
        private string _couponsID;
        /// <summary>
        /// 选用优惠券的ID
        /// </summary>
        public string CouponsID
        {
            get { return _couponsID; }
            set { _couponsID = value; }
        }
        /// <summary>
        /// Osn
        /// </summary>		
        private string _osn;
        public string Osn
        {
            get { return _osn; }
            set { _osn = value; }
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
        /// OrderState
        /// </summary>		
        private int _orderstate;
        public int OrderState
        {
            get { return _orderstate; }
            set { _orderstate = value; }
        }
        /// <summary>
        /// UserName
        /// </summary>		
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// Mobile
        /// </summary>		
        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        /// <summary>
        /// Plate
        /// </summary>		
        private string _plate;
        public string Plate
        {
            get { return _plate; }
            set { _plate = value; }
        }
        /// <summary>
        /// BrandShow
        /// </summary>		
        private string _brandshow;
        public string BrandShow
        {
            get { return _brandshow; }
            set { _brandshow = value; }
        }
        /// <summary>
        /// BrandID
        /// </summary>		
        private int _brandid;
        public int BrandID
        {
            get { return _brandid; }
            set { _brandid = value; }
        }
        /// <summary>
        /// ModelID
        /// </summary>		
        private int _modelid;
        public int ModelID
        {
            get { return _modelid; }
            set { _modelid = value; }
        }
        /// <summary>
        /// StoreID
        /// </summary>		
        private int _storeid;
        public int StoreID
        {
            get { return _storeid; }
            set { _storeid = value; }
        }
        private int _businessID;

        public int BusinessID
        {
            get { return _businessID; }
            set { _businessID = value; }
        }
        private int _workid;
        public int WorkID
        {
            get { return _workid; }
            set { _workid = value; }
        }
        /// <summary>
        /// Color
        /// </summary>		
        private string _color;
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        /// <summary>
        /// Lat
        /// </summary>		
        private string _lat;
        public string Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        /// <summary>
        /// Lng
        /// </summary>		
        private string _lng;
        public string Lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        /// <summary>
        /// IP
        /// </summary>		
        private string _ip;
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private DateTime _FinishDate;

        public DateTime FinishDate
        {
            get { return _FinishDate; }
            set { _FinishDate = value; }
        }
        public string PaySn { get; set; }
        public string PayName { get; set; }
        public DateTime PayTime { get; set; }
        /// <summary>
        /// CDate
        /// </summary>		
        private DateTime _cdate;
        public DateTime CDate
        {
            get { return _cdate; }
            set { _cdate = value; }
        }
        /// <summary>
        /// 洗车前照片
        /// </summary>
        public string Data1 { get; set; }
        /// <summary>
        /// 洗车后照片
        /// </summary>
        public string Data2 { get; set; }
        /// <summary>
        /// 线上/线下
        /// </summary>
        public string Data3 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
    }
}