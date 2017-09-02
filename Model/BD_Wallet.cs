using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class BD_Wallet
    { 
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Money1
        /// </summary>
        public decimal Money1 { get; set; }
        /// <summary>
        /// Money2
        /// </summary>
        public decimal Money2 { get; set; }
    }
}
