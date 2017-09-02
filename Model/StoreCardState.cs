using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public enum StoreCardState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 10,
        /// <summary>
        /// 已使用
        /// </summary>
        Userd = 30,
        /// <summary>
        /// 丢弃
        /// </summary>
        Discard = 50
    }
    public enum CouponsTypeState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 10,
        /// <summary>
        /// 禁用
        /// </summary>
        Discard = 30
    }
    public enum VipTypeState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 10,
        /// <summary>
        /// 禁用
        /// </summary>
        Discard = 30
    }
    public enum VipCardState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 10,
        /// <summary>
        /// 已使用
        /// </summary>
        Userd = 30,
        /// <summary>
        /// 丢弃
        /// </summary>
        Discard = 50
    }
}
