using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ArtSolution.Web.Models.Orders
{

    [AutoMap(typeof(Order))]
    public class OrderDtailModel : EntityDto
    {
        public OrderDtailModel()
        {
            this.Products = new List<OrderItemsModel>();
        }
        [DisplayName("订单编号")]
        public string OrderSn { get; set; }

        public int CustomerId { get; set; }
        [DisplayName("所属用户")]
        public string CustomerName { get; set; }

        [DisplayName("配送方式")]
        public string DeliveryId { get; set; }

        [DisplayName("快递号")]
        public string DeliveryCode { get; set; }

        /// <summary>
        /// 配送地址
        /// </summary>
        public int Billing { get; set; }
        [DisplayName("配送地址")]
        public string BillingAddress { get; set; }
        public int OrderStatusId { get; set; }
        [DisplayName("订单状态")]
        public string OrderStatus { get; set; }

        [DisplayName("商品小计")]
        public decimal Subtotal { get; set; }
        [DisplayName("运费")]
        public decimal Freight { get; set; }
        [DisplayName("订单总计")]
        public decimal OrderTotal { get; set; }

        [DisplayName("备注")]
        public string OrderRemarks { get; set; }

        [DisplayName("是否删除")]
        public bool IsDeleted { get; set; }

        [DisplayName("下单时间")]
        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public IList<OrderItemsModel> Products { get; set; }

        [AutoMap(typeof(OrderItem))]
        public class OrderItemsModel : EntityDto
        {
            [DisplayName("编号")]
            public Guid OrderItemGuid { get; set; }

            public int OrderId { get; set; }
            public int ProductId { get; set; }
            [DisplayName("商品名称")]
            public string ProductName { get; set; }
            [DisplayName("图片")]
            public string ProductImage { get; set; }
            [DisplayName("数量")]
            public int Quantity { get; set; }
            [DisplayName("单价")]
            public decimal Price { get; set; }
            [DisplayName("小计")]
            public decimal TotalPrice { get; set; }
        }
    }
}