using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(ComBoProduct))]
    public class ComBoProductModel : EntityDto
    {
        public ComBoProductModel()
        {
            this.ComBoProducts = new List<ProductMapping>();
            this.NewProduct = new ProductMapping();
        }
        /// <summary>
        /// 组合名称
        /// </summary>
        [DisplayName("套餐名称")]
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 组合图片
        /// </summary>
        [DisplayName("套餐图片")]
        [UIHint("PictureUrl")]
        [MaxLength(500)]
        public string ComBoProductImage { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [DisplayName("库存")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        [DisplayName("销售价")]
        public decimal Price { get; set; }


        /// <summary>
        /// 原价
        /// </summary>
        [DisplayName("原价")]
        public decimal Market { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [DisplayName("是否上架")]
        public bool Published { get; set; }

        public List<ProductMapping> ComBoProducts { get; set; }

        public ProductMapping NewProduct { get; set; }

        public class ProductMapping
        {

            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }
        }
    }
}