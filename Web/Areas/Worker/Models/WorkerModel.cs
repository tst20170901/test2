using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Worker.Models
{
    public class WorkerModel
    {
        public int Uid { get; set; }
        public string Mobile { get; set; }
        public string EncryptPassword { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Lng { get; set; }
        /// <summary>
        /// 0停工10空闲30忙碌50休息
        /// </summary>
        public int WorkState { get; set; }
        public DateTime ReDate { get; set; }
    }
}