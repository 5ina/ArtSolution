using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Category))]
    public class CategoryModel : EntityDto
    {
        public CategoryModel()
        {
            this.AvailableParentCategories = new List<SelectListItem>();
        }
        [DisplayName("类别名称")]
        public string Name { get; set; }

        [DisplayName("介绍")]
        public string Description { get; set; }


        [DisplayName("关键字")]
        public string MetaKeywords { get; set; }
        [DisplayName("标题")]
        public string MetaTitle { get; set; }

        [DisplayName("说明")]
        public string MetaDescription { get; set; }
        
        [DisplayName("父类")]
        public int ParentId { get; set; }
        [DisplayName("是否发布")]
        public bool Published { get; set; }
        [DisplayName("权重")]
        public int DisplayOrder { get; set; }

        public IList<SelectListItem> AvailableParentCategories { get; set; }
    }
}