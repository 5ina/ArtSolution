using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;

namespace ArtSolution.Web.Models.Catalogs
{

    [AutoMap(typeof(Product))]
    public class SimpleProductModel : EntityDto
    {
        public string ProductCode { get; set; }
        public int SaleModeId { get; set; }
        
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public string ProductImage { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int StockQuantity { get; set; }
        
        public decimal Price { get; set; }
        
        public bool MarkAsNew { get; set; }
        
        public decimal? SpecialPrice { get; set; }
        
        public DateTime? SpecialPriceStartDateTime { get; set; }
        
        public DateTime? SpecialPriceEndDateTime { get; set; }

        /// <summary>
        /// 是否预售商品
        /// </summary>
        public bool PreSell { get; set; }


        public int DisplayOrder { get; set; }
        
    }
}