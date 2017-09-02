using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Submitted = 10,
        /// <summary>
        /// 等待付款
        /// </summary>
        WaitPaying = 30,
        /// <summary>
        /// 已付款
        /// </summary>
        Confirming = 50
    }
}
