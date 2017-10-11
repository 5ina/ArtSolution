using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Orders
{
    [AutoMap(typeof(Delivery))]
    public class DeliveryModel : EntityDto
    {
        [DisplayName("配送名称")]
        public string Name { get; set; }

        [DisplayName("是否启用")]
        public bool Active { get; set; }

        [DisplayName("权重")]
        [UIHint("DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [DisplayName("说明")]
        public string Description { get; set; }
    }
}