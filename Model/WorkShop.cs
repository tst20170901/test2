using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class WorkShop
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string UserPwd { get; set; }
        public int BranchID { get; set; }
        public int TypeID { get; set; }
        public int WorkState { get; set; }
        public int WorkRadius { get; set; }
        public int CostTime { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string District { get; set; }
        public string Adcode { get; set; }
        public string Lng { get; set; }
        public string Lng1 { get; set; }
        public string Lat { get; set; }
        public string Lat1 { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReDate { get; set; }
        public DateTime CDate { get; set; }
        public int OrderCount { get; set; }
        public int OrderTime { get; set; }
        /// <summary>
        /// 距离目标多远
        /// </summary>
        public int Distan { get; set; }
        /// <summary>
        /// 需要多少秒
        /// </summary>
        public int Dur { get; set; }
        public string ST { get; set; }
        public string ET { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// 实体店头像
        /// </summary>
        public string Img { get; set; }
    }
}
