using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Models.Orders
{
    [AutoMap(typeof(ComBoProduct))]
    public class ComBoProductOrderModel:EntityDto
    {
        public ComBoProductOrderModel() {

            this.Items = new List<ProductModel>();
        }

        public string Name { get; set; }
        
        public string ComBoProductImage { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 原价
        /// </summary>
        public decimal Market { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        public List<ProductModel> Items { get; set; }


        [AutoMap(typeof(Product))]
        public class ProductModel: EntityDto
        {
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public string ProductImage { get; set; }
            public int Quantity { get; set; }

        }
    }
}