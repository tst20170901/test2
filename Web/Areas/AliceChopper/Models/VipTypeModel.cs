using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class VipTypeModel
    {
        public string Title { get; set; }
        public string VipDes { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public int FreeID { get; set; }
        public int BusinessID { get; set; }
        public string SMSContent { get; set; }
        /// <summary>
        /// 1线下卡；2线上卡
        /// </summary>
        public int Online { get; set; }
        /// <summary>
        /// 1不绑定，2绑定
        /// </summary>
        public int LockPlate { get; set; }
        public int Period { get; set; }
        /// <summary>
        /// 限购数量
        /// </summary>
        public int CardCount { get; set; }
    }
}