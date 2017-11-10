using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Orders
{
    public class OrderItem :Entity
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
        /// 是否预售商品
        /// </summary>
        public bool PreSell { get; set; }

        [Required, MaxLength(50)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string ProductImage { get; set; }

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
        
        /// <summary>
        /// 商品评论
        /// </summary>
        public bool Review { get; set; }
    }
}
