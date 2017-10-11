using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;

namespace ArtSolution.Api.Models.Catalog
{

    [AutoMap(typeof(Product))]
    public class ProductModel : EntityDto
    {
        public int CategoryId { get; set; }
        
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        
        public string FullDescription { get; set; }
        
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        
        public int StockQuantity { get; set; }
        
        public decimal Price { get; set; }
        public bool MarkAsNew { get; set; }
        
        public decimal? SpecialPrice { get; set; }
        
        public DateTime? SpecialPriceStartDateTime { get; set; }
        
        public DateTime? SpecialPriceEndDateTime { get; set; }
        
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        

    }
}
