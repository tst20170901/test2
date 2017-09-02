using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class BD_BigUserModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public decimal Money { get; set; }

        public string Address { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}