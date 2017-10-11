using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Brand))]
    public class BrandModel : EntityDto
    {
        public BrandModel()
        {
            this.AvailableCategories = new List<SelectListItem>();
        }
        
        [DisplayName("品牌名称")]
        public string Name { get; set; }
        [DisplayName("品牌介绍")]
        public string Description { get; set; }
        [DisplayName("品牌图片")]
        [UIHint("PictureUrl")]
        public string BrandImage { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        [DisplayName("权重")]
        [UIHint("DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public IList<SelectListItem> AvailableCategories { get; set; }
        
    }
}