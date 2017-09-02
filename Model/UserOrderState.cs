using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 用户订单状态
    /// </summary>
    public enum UserOrderState
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitPaying = 0,
        /// <summary>
        /// 支付完成
        /// </summary>
        Payed = 10,
        /// <summary>
        /// 派单完成
        /// </summary>
        Assigned = 30,
        /// <summary>
        /// 洗车开始
        /// </summary>
        Started = 50,
        /// <summary>
        /// 洗车完成
        /// </summary>
        Finished = 70,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 90,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 110,
        /// <summary>
        /// 作废
        /// </summary>
        Void = 130
    }
}
