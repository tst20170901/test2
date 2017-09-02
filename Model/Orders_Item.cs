using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Orders_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Oid
        /// </summary>
        public int Oid { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// ItemID
        /// </summary>
        public int ItemID { get; set; }
        /// <summary>
        /// ItemName
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// StoreID
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        public decimal Money { get; set; }
        public decimal WorkPrice { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
    }
}