using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Worker.Models
{
    public class BD_UserModel
    { /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}