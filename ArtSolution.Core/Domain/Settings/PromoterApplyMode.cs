using System.ComponentModel;

namespace ArtSolution.Domain.Settings
{
    /// <summary>
    /// 申请条件
    /// </summary>
    public enum PromoterApplyMode : int
    {
        /// <summary>
        /// 不限制
        /// </summary>
        [Description("不限制")]
        Unlimited = 0,
        /// <summary>
        /// 订单量
        /// </summary>
        [Description("订单量")]
        OrderCount = 1,
        /// <summary>
        /// 订单额
        /// </summary>
        [Description("订单额")]
        OrderTotal = 2,
    }
}
