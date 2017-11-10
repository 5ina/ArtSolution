using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace ArtSolution.Domain.Orders
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCartItem:Entity, IHasCreationTime
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        public DateTime CreationTime { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 是否预售商品
        /// </summary>
        public bool PreSell { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStartDateTime { get; set; }

        public DateTime? SpecialPriceEndDateTime { get; set; }
        
    }
}
