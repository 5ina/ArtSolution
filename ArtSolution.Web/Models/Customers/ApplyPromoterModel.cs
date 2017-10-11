using System.ComponentModel;

namespace ArtSolution.Web.Models.Customers
{
    /// <summary>
    /// 申请推广人
    /// </summary>
    public class ApplyPromoterModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        [DisplayName("手机号")]
        public string NickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// 消费总金额
        /// </summary>
        [DisplayName("消费额")]
        public decimal CostTotal { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [DisplayName("订单数")]
        public int OrderCount { get; set; }

        /// <summary>
        /// 是否可以申请
        /// </summary>
        
        public bool MayBe { get; set; }
    }
}