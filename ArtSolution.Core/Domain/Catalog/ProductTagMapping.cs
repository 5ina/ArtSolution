using Abp.Domain.Entities;

namespace ArtSolution.Domain.Catalog
{
    public class ProductTagMapping: Entity
    {
        public int TagId { get; set; }

        public int ProductId { get; set; }
    }
}
