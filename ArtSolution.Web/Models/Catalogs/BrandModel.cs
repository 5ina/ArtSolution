using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Web.Models.Catalogs
{

    [AutoMap(typeof(Brand))]
    public class BrandModel : EntityDto
    {
        
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int DisplayOrder { get; set; }

        public string BrandImage { get; set; }
    }
}