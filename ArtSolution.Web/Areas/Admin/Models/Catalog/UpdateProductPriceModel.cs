using Abp.Application.Services.Dto;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    public class UpdateProductPriceModel : EntityDto
    {
        public decimal Price { get; set; }
    }
}