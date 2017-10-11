using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Domain.Catalog.Attribute))]
    public class AttributeModel : EntityDto
    {

        [DisplayName("属性名称")]
        [Required, MaxLength(20)]
        public string Name { get; set; }        

        [DisplayName("权重")]
        public int DisplayOrder { get; set; }
    }
}