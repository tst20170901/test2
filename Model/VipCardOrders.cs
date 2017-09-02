using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class VipCardOrders
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
        private int _branchID;
        public int BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
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
        /// VipTypeID
        /// </summary>		
        private int _viptypeid;
        public int VipTypeID
        {
            get { return _viptypeid; }
            set { _viptypeid = value; }
        }
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        /// <summary>
        /// OrderAmount
        /// </summary>		
        private decimal _orderamount;
        public decimal OrderAmount
        {
            get { return _orderamount; }
            set { _orderamount = value; }
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
        /// <summary>
        /// PaySn
        /// </summary>		
        private string _paysn = "";
        public string PaySn
        {
            get { return _paysn; }
            set { _paysn = value; }
        }
        /// <summary>
        /// PayName
        /// </summary>		
        private string _payname = "支付宝";
        public string PayName
        {
            get { return _payname; }
            set { _payname = value; }
        }
        /// <summary>
        /// PayTime
        /// </summary>		
        private DateTime _paytime = new DateTime(1900, 1, 1);
        public DateTime PayTime
        {
            get { return _paytime; }
            set { _paytime = value; }
        }
        private string workno = "";
        public string WorkNo
        {
            get { return workno; }
            set { workno = value; }
        }
    }
}