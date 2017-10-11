using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{
    [AutoMap(typeof(Category))]
    public class CatalogModel : EntityDto
    {
        public CatalogModel()
        {
            this.Childs = new List<CatalogModel>();
        }

        public string Name { get; set; }

        public string Description { get; set; }


        public string MetaKeywords { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDescription { get; set; }

        public List<CatalogModel> Childs { get; set; }
    }
}