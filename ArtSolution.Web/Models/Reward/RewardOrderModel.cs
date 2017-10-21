using Abp.Application.Services.Dto;
using ArtSolution.Web.Models.Orders;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Reward
{
    public class RewardOrderModel : EntityDto
    {
        public RewardOrderModel()
        {
            this.Item = new OrderItemModel();
        }

        public string OrderSn { get; set; }

        public int CustomerId { get; set; }

        /// <summary>
        /// 配送地址
        /// </summary>
        public int Billing { get; set; }

        public string BillingAddress { get; set; }
        public int OrderStatusId { get; set; }

        public int PaymentStatusId { get; set; }

        /// <summary>
        /// 商品小计
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// 运输费用
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal OrderTotal { get; set; }
        

        public OrderItemModel Item { get; set; }

        public string JsApiString { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Preferential { get; set; }
    }
}