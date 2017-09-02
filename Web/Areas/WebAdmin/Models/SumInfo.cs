using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.WebAdmin.Models
{
    public class SumInfo
    {
        /// <summary>
        /// 总单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 未付款
        /// </summary>
        public int OrderCountNo { get; set; }
        /// <summary>
        /// 已付款
        /// </summary>
        public int OrderCountYes { get; set; }
    }
}