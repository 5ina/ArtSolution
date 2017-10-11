using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 商品拍卖列表
    /// </summary>
    public class ProductAuctionList : Entity, IHasCreationTime
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeal { get; set; }
    }
}
