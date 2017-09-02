using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{

    [Serializable]
    public class NoticeLogs
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
        /// WorkID
        /// </summary>
        public int WorkID { get; set; }
        /// <summary>
        /// WorkName
        /// </summary>
        public string WorkName { get; set; }
        /// <summary>
        /// BranchID
        /// </summary>
        public int BranchID { get; set; }
        /// <summary>
        /// SMSContent
        /// </summary>
        public string SMSContent { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }

    }
}