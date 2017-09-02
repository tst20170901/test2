using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class BD_BusinessModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }
        public string BusinessName { get; set; }
        public decimal Wallet { get; set; }
        /// <summary>
        /// 正值为充值，负值为减少
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Remark { get; set; }
    }
}