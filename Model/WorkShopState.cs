using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 洗车工状态
    /// </summary>
    public enum WorkShopState
    {
        /// <summary>
        /// 停工
        /// </summary>
        Close = 0,
        /// <summary>
        /// 空闲
        /// </summary>
        Open = 10,
        /// <summary>
        /// 忙碌
        /// </summary>
        Busy = 30,
        /// <summary>
        /// 休息
        /// </summary>
        Rest = 50
    }
}
