using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Product))]
    public class ProductReviewModel : EntityDto
    {

        [DisplayName("商品类别")]
        public int CategoryId { get; set; }
        [DisplayName("商品编号")]
        public string ProductCode { get; set; }
        [DisplayName("品牌")]
        public int BrandId { get; set; }

        [DisplayName("商品名称")]
        public string Name { get; set; }
    }
}