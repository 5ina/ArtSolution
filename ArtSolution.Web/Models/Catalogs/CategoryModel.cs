using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Web.Models.Catalogs
{
    [AutoMap(typeof(Category))]
    public class CategoryModel : EntityDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        
        public string MetaKeywords { get; set; }

        public string MetaTitle { get; set; }
        
        public string MetaDescription { get; set; }
        
    }
}