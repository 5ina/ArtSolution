using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;

namespace ArtSolution.Api.Models.Customers
{
    [AutoMap(typeof(CustomerAttribute))]
    public class CustomerAttributeModel : EntityDto
    {
        public int CustomerId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
