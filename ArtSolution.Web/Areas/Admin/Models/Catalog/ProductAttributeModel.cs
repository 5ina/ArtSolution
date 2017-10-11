using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// 商品属性实体
    /// </summary>
    [AutoMap(typeof(ProductAttribute))]
    public class ProductAttributeModel : EntityDto
    {

        public int ProductId { get; set; }

        [DisplayName("属性名")]
        public string ValueName { get; set; }
        /// <summary>
        /// 规格的价格
        /// </summary>
        [DisplayName("价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [DisplayName("库存")]
        [UIHint("DisplayOrder")]
        public int Stock { get; set; }
    }
}