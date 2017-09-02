using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class Edo_CheckLogs
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Plate
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// UserMobile
        /// </summary>
        public string UserMobile { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
    }
}