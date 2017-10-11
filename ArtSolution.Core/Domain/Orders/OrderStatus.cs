using System.ComponentModel;

namespace ArtSolution.Domain.Orders
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus:int
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        Pending = 10,
        /// <summary>
        /// 支付
        /// </summary>
        [Description("已支付")]
        Paid = 20,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Delivered = 30,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 40,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancel = -10,


        [Description("退单申请")]
        ReturnOrdering = 90,

        [Description("退单完成")]
        ReturnOrdered = 100,
    }
}
