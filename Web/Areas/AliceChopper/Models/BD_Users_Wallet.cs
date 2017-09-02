using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class BD_Users_Wallet
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }
        public decimal Money1 { get; set; }
        public decimal Money2 { get; set; }
        public string Remark { get; set; }
    }
}