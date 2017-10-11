using Abp.Application.Services.Dto;

namespace ArtSolution.Web.Models.Customers
{
    public class CustomerAddressModel : EntityDto
    {
        public string userName { get; set; }

        public string telNumber { get; set; }
        public string provinceName { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string detailInfo { get; set; }

    }
}