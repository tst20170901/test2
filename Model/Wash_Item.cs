using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class Wash_Item
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 工人提点
        /// </summary>
        public decimal WorkPrice { get; set; }
        /// <summary>
        /// IsMust 分组
        /// </summary>
        public int IsMust { get; set; }
        /// <summary>
        /// ItemState 0停用  10正常
        /// </summary>
        public int ItemState { get; set; }
        public int SortID { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
        /// <summary>
        /// 10线上 20线下 30通用
        /// </summary>
        public int Online { get; set; }
        /// <summary>
        /// 分公司ID
        /// </summary>
        public int BranID { get; set; }
    }
}
