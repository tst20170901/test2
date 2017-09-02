using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class OrderActions
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
        /// Oid
        /// </summary>		
        private int _oid;
        public int Oid
        {
            get { return _oid; }
            set { _oid = value; }
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
        /// RealName
        /// </summary>		
        private string _realname;
        public string RealName
        {
            get { return _realname; }
            set { _realname = value; }
        }
        /// <summary>
        /// ActionType
        /// </summary>		
        private string _actiontype;
        public string ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        /// <summary>
        /// ActionTime
        /// </summary>		
        private DateTime _actiontime;
        public DateTime ActionTime
        {
            get { return _actiontime; }
            set { _actiontime = value; }
        }
        /// <summary>
        /// ActionDes
        /// </summary>		
        private string _actiondes;
        public string ActionDes
        {
            get { return _actiondes; }
            set { _actiondes = value; }
        }
    }
}