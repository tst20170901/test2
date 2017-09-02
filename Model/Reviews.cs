using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class Reviews
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
        /// Uid
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// StoreID
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// SpeedStar
        /// </summary>
        public int SpeedStar { get; set; }
        /// <summary>
        /// ServiceStar
        /// </summary>
        public int ServiceStar { get; set; }
        /// <summary>
        /// AttitudeStar
        /// </summary>
        public int AttitudeStar { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
    }
}
