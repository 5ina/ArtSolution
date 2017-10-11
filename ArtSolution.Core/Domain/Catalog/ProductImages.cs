using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    public class ProductImage : Entity
    {
        public int ProductId { get; set; }

        [MaxLength(500)]
        public string Url { get; set; }
    }
}
