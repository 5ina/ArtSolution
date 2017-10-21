using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Reward
{

    [AutoMap(typeof(Product))]
    public class RewardProductModel : EntityDto
    {
        public RewardProductModel()
        {
            this.SubProductAttributes = new List<ProductAttributeModel>();
        }        

        public string Name { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }
        public bool PreSell { get; set; }
        public bool AllowReward { get; set; }

        public int RewardExchange { get; set; }
        public string ProductImage { get; set; }
        
        
        public bool Published { get; set; }

        public decimal ShipFree { get; set; }

        /// <summary>
        /// 用户当前的积分
        /// </summary>
        public int CustomerReward { get; set; }

        public IList<ProductAttributeModel> SubProductAttributes { get; set; }

        

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