using System.ComponentModel;

namespace ArtSolution.Domain.Settings
{
    /// <summary>
    /// 推广模式
    /// </summary>
    public enum RewardMode : int
    {
        /// <summary>
        /// 按照时间方式
        /// </summary>
        [Description("时间")]
        Date = 1,
        /// <summary>
        /// 按照订单方式
        /// </summary>
        [Description("订单")]
        OrderCount = 2,
    }
}
