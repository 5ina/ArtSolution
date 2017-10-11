using Abp.Application.Services.Dto;

namespace ArtSolution.Api.Models.Customers
{
    public class CustomerOAuthModel:EntityDto
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }
    }
}
