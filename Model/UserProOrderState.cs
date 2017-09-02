using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 用户商品订单状态
    /// </summary>
    public enum UserProOrderState
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
        /// 备货完成
        /// </summary>
        Assigned = 30,
        /// <summary>
        /// 发货中
        /// </summary>
        Started = 50,
        /// <summary>
        /// 已签收
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
