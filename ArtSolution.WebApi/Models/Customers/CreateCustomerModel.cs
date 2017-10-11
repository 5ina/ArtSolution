using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Api.Models.Customers
{
    public class CreateCustomerModel :EntityDto
    {
        [Required, MaxLength(11)]
        public string Mobile { get; set; }

        public int CustomerRole { get; set; }
    }
}
