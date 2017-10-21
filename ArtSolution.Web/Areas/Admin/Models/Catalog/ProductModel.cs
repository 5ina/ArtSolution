using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Product))]
    public class ProductModel : EntityDto
    {
        public ProductModel()
        {
            this.AvailableCategories = new List<SelectListItem>();
            this.AvailablePictures = new List<ProductPictureModel>();
            this.AddPictureModel = new ProductPictureModel();
            this.AvailableAttributes = new List<ProductAttributeModel>();
            this.AvailableBrands = new List<SelectListItem>();
            this.ProductTagIds = new List<int>();
            this.AvailableProductTag = new List<SelectListItem>();
        }
        [DisplayName("商品类别")]
        public int CategoryId { get; set; }
        [DisplayName("商品编号")]
        public string ProductCode { get; set; }
        [DisplayName("品牌")]        
        [UIHint("MinimalSelect")]
        public int BrandId { get; set; }

        [DisplayName("商品名称")]
        public string Name { get; set; }
        [DisplayName("介绍")]
        public string ShortDescription { get; set; }

        [DisplayName("图文介绍")]
        [UIHint("RichEditor")]
        public string FullDescription { get; set; }


        [DisplayName("关键字")]
        public string MetaKeywords { get; set; }
        [DisplayName("说明")]
        public string MetaDescription { get; set; }
        [DisplayName("标题")]
        public string MetaTitle { get; set; }

        [DisplayName("库存")]
        public int StockQuantity { get; set; }

        [DisplayName("销售价")]
        public decimal Price { get; set; }
        [DisplayName("市场价")]
        public decimal Market { get; set; }
        [DisplayName("成本")]
        public decimal Cost { get; set; }

        [DisplayName("是否预售")]
        public bool PreSell { get; set; }
        [DisplayName("积分兑换")]
        public bool AllowReward { get; set; }
        /// <summary>
        /// 特殊价格
        /// </summary>
        [DisplayName("促销价")]
        public decimal? SpecialPrice { get; set; }

        [DisplayName("促销日期（开始）")]
        [UIHint("DateNullable")]
        public DateTime? SpecialPriceStartDateTime { get; set; }

        [DisplayName("促销日期（结束）")]
        [UIHint("DateNullable")]
        public DateTime? SpecialPriceEndDateTime { get; set; }


        [DisplayName("权重")]
        public int DisplayOrder { get; set; }
        [DisplayName("是否上架")]
        public bool Published { get; set; }

        [DisplayName("商品标签")]
        [UIHint("MultipleSelect")]
        public List<int> ProductTagIds { get; set; }

        public List<SelectListItem> AvailableProductTag { get; set; }
        public ProductPictureModel AddPictureModel { get; set; }
        public IList<ProductPictureModel> AvailablePictures { get; set; }


        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<ProductAttributeModel> AvailableAttributes { get; set; }

        public partial class ProductPictureModel
        {
            [DisplayName("图片")]
            public int Id { get; set; }

            public int ProductId { get; set; }

            [UIHint("PictureUrl")]
            [DisplayName("图片地址")]
            public string PictureUrl { get; set; }
        }


        public IList<SelectListItem> AvailableBrands { get; set; }
    
    }

}