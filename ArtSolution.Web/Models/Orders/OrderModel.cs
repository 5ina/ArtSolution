using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Web.Models.Orders
{
    [AutoMap(typeof(Order))]
    public class OrderModel : EntityDto
    {
        public string OrderSn { get; set; }

        public int CustomerId { get; set; }
        
        public string DeliveryAddress { get; set; }
        public decimal OrderTotal { get; set; }
        
        public string OrderRemarks { get; set; }
        
    }
}