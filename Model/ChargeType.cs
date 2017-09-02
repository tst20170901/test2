using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class ChargeType
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
        /// GivePrice
        /// </summary>		
        private decimal _giveprice;
        public decimal GivePrice
        {
            get { return _giveprice; }
            set { _giveprice = value; }
        }
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _state;
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}