using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 优惠券状态
    /// </summary>
    public enum CouponState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Submitted = 10,
        /// <summary>
        /// 已使用
        /// </summary>
        Userd = 30,
        /// <summary>
        /// 废弃
        /// </summary>
        Discard = 50
    }
}
