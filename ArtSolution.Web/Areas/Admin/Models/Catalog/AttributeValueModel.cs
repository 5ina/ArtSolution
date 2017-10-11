using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(Domain.Catalog.AttributeValue))]
    public class AttributeValueModel : EntityDto
    {
        [DisplayName("属性值名称")]
        public string ValueName { get; set; }

        /// <summary>
        /// 所属属性
        /// </summary>
        public int AttributeId { get; set; }
        public int DisplayOrder { get; set; }
    }
}