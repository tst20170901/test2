using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class UserCar
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Plate
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// BrandShow
        /// </summary>
        public string BrandShow { get; set; }
        /// <summary>
        /// BrandID
        /// </summary>
        public int BrandID { get; set; }
        /// <summary>
        /// ModelID
        /// </summary>
        public int ModelID { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Data1
        /// </summary>
        public string Data1 { get; set; }
        /// <summary>
        /// Data2
        /// </summary>
        public string Data2 { get; set; }
        /// <summary>
        /// Data3
        /// </summary>
        public string Data3 { get; set; }
        /// <summary>
        /// Data4
        /// </summary>
        public string Data4 { get; set; }
        /// <summary>
        /// Data5
        /// </summary>
        public string Data5 { get; set; }
        public string Mobile { get; set; }
    }
}
