using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System;

namespace ArtSolution.Web.Models.Orders
{

    [AutoMap(typeof(OrderItem))]
    public class OrderItemModel : EntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid OrderItemGuid { get; set; }

        /// <summary>
        /// 订单主键
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 商品主键
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        public string ProductImage { get; set; }
        public string ProductName { get; set; }
    }
}