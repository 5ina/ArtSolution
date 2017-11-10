using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{

    [AutoMap(typeof(Product))]
    public class ProductModel : EntityDto
    {
        public ProductModel()
        {
            this.SubPictures = new List<ProductPictureModel>();
            this.SubProductAttributes = new List<ProductAttributeModel>();
            this.ProductRelateds = new List<SimpleProductModel>();
        }

        public int CategoryId { get; set; }
        public string ProductCode { get; set; }
        
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        
        public string FullDescription { get; set; }

        
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        
        public int StockQuantity { get; set; }

        public decimal Price { get; set; }
        public bool PreSell { get; set; }
        public bool AllowReward { get; set; }

        public int RewardExchange { get; set; }
        public decimal Market { get; set; }
        public decimal Cost { get; set; }

        public string ProductImage { get; set; }


        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }

        public int BrandToProductCount { get; set; }

        /// <summary>
        /// 特殊价格
        /// </summary>
        public decimal? SpecialPrice { get; set; }
        
        public DateTime? SpecialPriceStartDateTime { get; set; }
        
        public DateTime? SpecialPriceEndDateTime { get; set; }

        public int SpecialQuantity { get; set; }


        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsFavorites { get; set; }

        public IList<ProductPictureModel> SubPictures { get; set; }

        public IList<ProductAttributeModel> SubProductAttributes { get; set; }

        public string RelatedProductIds { get; set; }
        public List<SimpleProductModel> ProductRelateds { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public partial class ProductPictureModel
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public string PictureUrl { get; set; }

            public bool IsDefault { get; set; }
        }


        /// <summary>
        /// 商品属性
        /// </summary>

        /// <summary>
        /// 商品属性实体
        /// </summary>
        [AutoMap(typeof(ProductAttribute))]
        public class ProductAttributeModel : EntityDto
        {

            public int ProductId { get; set; }            
            public string ValueName { get; set; }
            public decimal Price { get; set; }
            
            public int Stock { get; set; }
        }
        
    }
}