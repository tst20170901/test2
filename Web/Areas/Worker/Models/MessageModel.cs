using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Worker.Models
{
    public class MessageModel
    {
        public MessageModel()
        {
            code = "1";
            msg = "success";
        }
        /// <summary>
        /// 1成功，0不成功，2需要重新登录
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }
    }
}