using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ArtSolution.Web.Models.Orders
{
    [AutoMap(typeof(Order))]
    public class CreateOrderModel: EntityDto
    {
        public CreateOrderModel() {
            this.Items = new List<OrderItemModel>();
            this.AvailableCoupons = new List<SelectListItem>();
        }

        public string OrderSn { get; set; }

        public int CustomerId { get; set; }

        /// <summary>
        /// 配送地址
        /// </summary>
        public int Billing { get; set; }
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

        [DisplayName("备注")]
        public string OrderRemarks { get; set; }

        public List<OrderItemModel> Items { get; set; }

        public string JsApiString { get; set; }

        public IList<SelectListItem> AvailableCoupons { get; set; }

        [DisplayName("优惠券")]
        public int CouponId { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Preferential { get; set; }
    }
}