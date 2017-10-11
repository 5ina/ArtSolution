using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Api.Models.Catalog
{

    [AutoMap(typeof(Category))]
    public class CategoryModel : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }


        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }

        public int ParentId { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }

        public bool IsDeleted { get; set; }
    }
}
