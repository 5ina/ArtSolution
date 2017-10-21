using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product : Entity, ISoftDelete, ICreationAudited, IModificationAudited
    {
        public int CategoryId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string ProductCode { get; set; }

        [MaxLength(500)]
        public string ProductImage { get; set; }
        [MaxLength(500)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }


        [MaxLength(200)]
        public string MetaKeywords { get; set; }
        [MaxLength(500)]
        public string MetaDescription { get; set; }
        [MaxLength(200)]
        public string MetaTitle { get; set; }

        public int BrandId { get; set; }

        public int StockQuantity { get; set; }

        /// <summary>
        /// 已售数量
        /// </summary>
        public int WithOrder { get; set; }

        /// <summary>
        /// 基础价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal Market { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal Cost { get; set; }
        
        /// <summary>
        /// 特殊价格
        /// </summary>
        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStartDateTime { get; set; }

        public DateTime? SpecialPriceEndDateTime { get; set; }

        /// <summary>
        /// 是否预售
        /// </summary>
        public bool PreSell { get; set; }

        /// <summary>
        /// 允许使用积分兑换
        /// </summary>
        public bool AllowReward { get; set; }

        public int DisplayOrder { get; set; }
        public bool Published { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? CreatorUserId { get; set; }
        
    }
}
