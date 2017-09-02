using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_BusiAction_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// ActionID
        /// </summary>
        public int ActionID { get; set; }
        /// <summary>
        /// ItemID
        /// </summary>
        public int ItemID { get; set; }
        /// <summary>
        /// ItemTitle
        /// </summary>
        public string ItemTitle { get; set; }
        /// <summary>
        /// ItemPrice
        /// </summary>
        public decimal ItemPrice { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }
        public int Period { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
    }
}