using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class BD_Branch
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
        /// Mobile
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// BranchState
        /// </summary>
        public int BranchState { get; set; }
        public string StateMsg { get; set; }
        public int OrderCount { get; set; }
        /// <summary>
        /// Province
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// CityCode
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// District
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// Adcode
        /// </summary>
        public string Adcode { get; set; }
        /// <summary>
        /// CenterLng
        /// </summary>
        public string CenterLng { get; set; }
        /// <summary>
        /// CenterLat
        /// </summary>
        public string CenterLat { get; set; }
        /// <summary>
        /// Points
        /// </summary>
        public string Points { get; set; }
        /// <summary>
        /// CDate
        /// </summary>
        public DateTime CDate { get; set; }
    }
}