using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.AliceChopper.Models
{
    public class ActionsAllotModel
    {
        /// <summary>
        /// 管理员组标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 动作列表
        /// </summary>
        public string[] ActionList { get; set; }
    }
}