using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class VipCard
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// CardNo
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// CardPwd
        /// </summary>
        public string CardPwd { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Uid
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// UserCount
        /// </summary>
        public int UserCount { get; set; }
        public int CardCount { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Plate
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// TypeID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// BusinessID
        /// </summary>
        public int BusinessID { get; set; }
        /// <summary>
        /// CardState
        /// </summary>
        public int CardState { get; set; }
        /// <summary>
        /// SMSContent
        /// </summary>
        public string SMSContent { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
        /// <summary>
        /// TDate
        /// </summary>
        public DateTime TDate { get; set; }
        /// <summary>
        /// UseDate
        /// </summary>
        public DateTime UseDate { get; set; }
        public int FreeID { get; set; }
        public string ItemTitle { get; set; }
        public decimal ItemPrice { get; set; }
        public string BusinessName { get; set; }
        public string VipDes { get; set; }
    }
}
